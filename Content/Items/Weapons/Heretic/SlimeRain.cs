using fourClassesMod.Common.Classes.Heretic;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Heretic
{
    internal class SlimeRain : ModItem
    {

        private int lifeCost; // Add our custom resource cost
        private string deathMessage;
        private int messageHelper;

        public override string Texture => $"fourClassesMod/Sprites/Weapons/Slime_Rain";

        public override void SetDefaults()
        {
            Item.damage = 85;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.RaiseLamp; // Makes the player do the proper arm motion
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.autoReuse = false;
            Item.noMelee = true; // The projectile will do the damage and not the item
            lifeCost = 50;
            Item.rare = ItemRarityID.White;
            Item.shoot = ProjectileID.PurificationPowder;

            Item.shootSpeed = 8f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        }

        // Make sure you can't use the item if you don't have enough resource


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            messageHelper = Main.rand.Next(0, 4);
            if (messageHelper == 0)
            {
                deathMessage = $"{player.name}'s blood became dry of mana, unable to sustain its body.";
            }
            if (messageHelper == 1)
            {
                deathMessage = $"{player.name} overdrew their own life essence.";
            }
            if (messageHelper == 2)
            {
                deathMessage = $"{player.name} got greedy.";
            }
            if (messageHelper == 4)
            {
                deathMessage = $"{player.name} was consumed by their own magiks.";
            }
            type = ModContent.ProjectileType<SlimeGunCloneStream>();
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            player.Hurt(PlayerDeathReason.ByCustomReason(NetworkText.FromKey(deathMessage)), lifeCost, 0, false, false, -1, false, 500, 500, 0f);
            return false;
        }
    }


    public class SlimeGunCloneStream : ModProjectile
    {

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SlimeGun}";

        public override void SetDefaults()
        {
            // This method right here is the backbone of what we're doing here; by using this method, we copy all of
            // the Meowmere Projectile's SetDefault stats (such as projectile.friendly and projectile.penetrate) on to our projectile,
            // so we don't have to go into the source and copy the stats ourselves. It saves a lot of time and looks much cleaner;
            // if you're going to copy the stats of a projectile, use CloneDefaults().

            Projectile.CloneDefaults(ProjectileID.SlimeGun);

            // To further the Cloning process, we can also copy the ai of any given projectile using AIType, since we want
            // the projectile to essentially behave the same way as the vanilla projectile.
            AIType = ProjectileID.SlimeGun;

            // After CloneDefaults has been called, we can now modify the stats to our wishes, or keep them as they are.
            // For the sake of Cultist, lets make our projectile penetrate enemies a few more times than the vanilla projectile.
            // This can be done by modifying projectile.penetrate
            Projectile.penetrate += 50;
            Projectile.timeLeft = 120;

        }

        // While there are several different ways to change how our projectile could behave differently, lets make it so
        // when our projectile finally dies, it will explode into 4 regular Meowmere projectiles.
        public override void OnKill(int timeLeft)
        {
            Vector2 launchVelocity = new Vector2(0, -2); // Create a velocity moving the left.
            for (int i = 0; i < 50; i++)
            {
                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity = launchVelocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(90, 270)));
                launchVelocity *= Main.rand.Next(1, 4);

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<slimeSpikeClone>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                launchVelocity = new Vector2(0, -2);
            }
        }

        // Now, using CloneDefaults() and aiType doesn't copy EVERY aspect of the projectile. In Vanilla, several other methods
        // are used to generate different effects that aren't included in AI. For the case of the Meowmere projectile, since the
        // ricochet sound is not included in the AI, we must add it ourselves:
    }


    public class slimeSpikeClone : ModProjectile
    {

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SpikedSlimeSpike}";

        public override void SetDefaults()
        {
            // This method right here is the backbone of what we're doing here; by using this method, we copy all of
            // the Meowmere Projectile's SetDefault stats (such as projectile.friendly and projectile.penetrate) on to our projectile,
            // so we don't have to go into the source and copy the stats ourselves. It saves a lot of time and looks much cleaner;
            // if you're going to copy the stats of a projectile, use CloneDefaults().

            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);

            // To further the Cloning process, we can also copy the ai of any given projectile using AIType, since we want
            // the projectile to essentially behave the same way as the vanilla projectile.
            AIType = ProjectileID.SpikedSlimeSpike;

            // After CloneDefaults has been called, we can now modify the stats to our wishes, or keep them as they are.
            // For the sake of Cultist, lets make our projectile penetrate enemies a few more times than the vanilla projectile.
            // This can be done by modifying projectile.penetrate
            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.glowMask = 5;
            Projectile.timeLeft = 600;
        }


    }
}
