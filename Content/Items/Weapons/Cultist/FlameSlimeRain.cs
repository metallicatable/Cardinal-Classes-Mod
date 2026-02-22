using fourClassesMod.Common.Classes.Cultist;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Cultist
{
    internal class FlamingSlimeRain : ModItem
    {

        private int faithCost; // Add our custom resource cost

        public override string Texture => $"Terraria/Images/Item_{ItemID.PalmWoodDoor}";

        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.RaiseLamp; // Makes the player do the proper arm motion
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<CultistDamageClass>();
            Item.autoReuse = false;
            Item.noMelee = true; // The projectile will do the damage and not the item
            faithCost = 75;
            Item.rare = ItemRarityID.White;
            Item.shoot = ProjectileID.PurificationPowder;

            Item.shootSpeed = 8f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        }

        // Make sure you can't use the item if you don't have enough resource


        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            var faithPlayer = player.GetModPlayer<FaithPlayer>();

            if (faithPlayer.FaithCurrent >= faithCost)
            {
                type = ModContent.ProjectileType<flamingSlimeGunCloneStream>();
                faithPlayer.FaithCurrent -= faithCost;
            }
            else
            {
                type = ModContent.ProjectileType<flamingSlimeSpikeClone>();
                damage = damage / 8;
                knockback = knockback * 1.2f;
                velocity = velocity * 2;
            }
        }
    }


    public class flamingSlimeGunCloneStream : ModProjectile
    {

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SlimeGun}";

        public override void SetDefaults()
        {
            AIType = ProjectileID.SlimeGun;
            Projectile.CloneDefaults(ProjectileID.SlimeGun);

            Projectile.penetrate += 50;
            Projectile.timeLeft = 120;
        }

        public override void OnKill(int timeLeft)
        {
            Vector2 launchVelocity = new Vector2(0, -2); // Create a velocity moving down
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileID.Flames, Projectile.damage * 2, Projectile.knockBack, Projectile.owner);

            for (int i = 0; i < 100; i++)
            {
                //calculate for main spikes
                launchVelocity = launchVelocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(0, 361)));
                launchVelocity *= Main.rand.Next(1, 6);

                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ModContent.ProjectileType<flamingSlimeSpikeClone>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                launchVelocity = new Vector2(0, -2);

                //calculate for sparks


            }
        }

        // Now, using CloneDefaults() and aiType doesn't copy EVERY aspect of the projectile. In Vanilla, several other methods
        // are used to generate different effects that aren't included in AI. For the case of the Meowmere projectile, since the
        // ricochet sound is not included in the AI, we must add it ourselves:
    }


    public class flamingSlimeSpikeClone : ModProjectile
    {

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SpikedSlimeSpike}";
        int timer;
        public override void SetDefaults()
        {
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.aiStyle = ProjAIStyleID.Arrow;

            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.glowMask = 5;
            Projectile.timeLeft = 600;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Color.Orange;
            return lightColor;
        }

        public override void AI()
        {
            timer++;

            if (timer % 2 == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);   
            }

            if (timer >= 300)
            {
                Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
