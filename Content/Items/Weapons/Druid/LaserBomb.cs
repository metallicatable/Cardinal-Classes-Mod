using fourClassesMod.Common.Classes.Druid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Druid
{
    internal class laserBomb : ModItem
    {

        private int energyCost; // Add our custom resource cost
        int laserColourTimer = 0;
        public override string Texture => $"Terraria/Images/Item_{ItemID.IronBroadsword}";

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Swing; // Makes the player do the proper arm motion
            Item.useAnimation = 5;
            Item.useTime = 5;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Item.autoReuse = false;
            Item.noMelee = true; // The projectile will do the damage and not the item
            energyCost = 100;
            Item.rare = ItemRarityID.White;
            Item.shoot = ProjectileID.PurificationPowder;

            Item.shootSpeed = 8f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        }

        // Make sure you can't use the item if you don't have enough resource

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var energyPlayer = player.GetModPlayer<EnergyPlayer>();
            Vector2 perturbedVelocity;

            if (player.altFunctionUse == 2)
            {
                if (energyPlayer.EnergyCurrent >= energyCost)
                {
                    type = ProjectileID.Bananarang;
                    energyPlayer.EnergyCurrent -= energyCost;
                    damage *= 2; ; // Optional: Increase damage for the alternate function
                    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                    return false;

                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (laserColourTimer == 0)
                {
                    type = ModContent.ProjectileType<laserCloneProjectile>();
                    laserColourTimer++;
                    perturbedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5)); // This adds a random spread to the projectiles, 10 degrees in either direction.
                    perturbedVelocity *= 2f + Main.rand.NextFloat(0.5f, 2f);

                    Projectile.NewProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI);
                    return false;
                }

                if (laserColourTimer == 1)
                {
                    type = ModContent.ProjectileType<laserCloneProjectile2>();
                    laserColourTimer = 0;
                    perturbedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                    perturbedVelocity *= 2f + Main.rand.NextFloat(0.5f, 2f);

                    Projectile.NewProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI);
                    return false;
                }
            }

            return false;
        }
    }

    public class laserCloneProjectile : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.PurpleLaser}";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Projectile.penetrate = 1; // This makes the projectile disappear after hitting an enemy once
            Projectile.timeLeft = 600; // The projectile will last for 10 seconds if it doesn't hit anything

        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Purple.ToVector3()); // This makes the projectile emit purple light
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
       
        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Color.Purple;
            lightColor.A = 50;
            return lightColor;
        }
       

    }

    public class laserCloneProjectile2 : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.PurpleLaser}";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Projectile.penetrate = 1; // This makes the projectile disappear after hitting an enemy once
            Projectile.timeLeft = 600; // The projectile will last for 10 seconds if it doesn't hit anything

        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Red.ToVector3()); // This makes the projectile emit purple light
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        
        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Color.Red;
            lightColor.A = 50;
            return lightColor;
        }
        

    }
}