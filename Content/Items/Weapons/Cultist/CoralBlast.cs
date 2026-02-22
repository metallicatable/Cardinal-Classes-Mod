using fourClassesMod.Common.Classes.Cultist;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Cultist
{
    internal class CoralBlast : ModItem
    {

        private int faithCost; // Add our custom resource cost

        public override string Texture => $"Terraria/Images/Item_{ItemID.Coral}";

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.RaiseLamp; // Makes the player do the proper arm motion
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<CultistDamageClass>();
            Item.autoReuse = false;
            Item.noMelee = true; // The projectile will do the damage and not the item
            faithCost = 75;
            Item.rare = ItemRarityID.White;
            Item.shoot = ModContent.ProjectileType<starFishProjectile>();

            Item.shootSpeed = 6f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var faithPlayer = player.GetModPlayer<FaithPlayer>();
            int coralType;
            if (faithPlayer.FaithCurrent >= faithCost)
            {

                for (int i = 0; i < 160; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.ToRadians(180), MathHelper.ToRadians(180)));
                    perturbedSpeed *= Main.rand.NextFloat(1.0f, 3.0f); // randomize speed
                    perturbedSpeed *= 2;

                    coralType = Main.rand.Next(1, 9); // Randomly select one of the 8 coral projectile types
                    
                    if (coralType == 1)
                    {
                        Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<mixedCoralProjectile1>(), damage, knockback, player.whoAmI);
                    }
                    else if (coralType == 2)
                    {
                        Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<mixedCoralProjectile2>(), damage, knockback, player.whoAmI);
                    }
                    else if (coralType == 3)
                    {
                        Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<mixedCoralProjectile3>(), damage, knockback, player.whoAmI);
                    }
                    else if (coralType == 4)
                    {
                        Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<mixedCoralProjectile4>(), damage, knockback, player.whoAmI);
                    }
                    else if (coralType == 5)
                    {
                        Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<mixedCoralProjectile5>(), damage, knockback, player.whoAmI);
                    }
                    else if (coralType == 6)
                    {
                        Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<mixedCoralProjectile6>(), damage, knockback, player.whoAmI);
                    }
                    else if (coralType == 7)
                    {
                        Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<mixedCoralProjectile7>(), damage, knockback, player.whoAmI);
                    }
                    else if (coralType == 8)
                    {
                        Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<mixedCoralProjectile8>(), damage, knockback, player.whoAmI);
                    }


                }

                faithPlayer.FaithCurrent -= faithCost;
                return false;
            }

            else
            {
                return true;
            }       
        }
    }

    public class starFishProjectile : ModProjectile
    {


        public override string Texture => $"Terraria/Images/Item_{ItemID.Starfish}";

        public override void SetDefaults()
        {

            AIType = ProjectileID.WoodenBoomerang;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;

            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
        }
    }

    public class mixedCoralProjectile1 : ModProjectile
    {

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/mixedCoralProjectile1";

        public override void SetDefaults()
        {
            
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;

            Projectile.penetrate += 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

    }

    public class mixedCoralProjectile2 : ModProjectile
    {

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/mixedCoralProjectile2";

        public override void SetDefaults()
        {

            AIType = ProjectileID.WoodenBoomerang;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;

            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

    }

    public class mixedCoralProjectile3 : ModProjectile
    {

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/mixedCoralProjectile3";

        public override void SetDefaults()
        {

            AIType = ProjectileID.WoodenBoomerang;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;

            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

    }

    public class mixedCoralProjectile4 : ModProjectile
    {

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/mixedCoralProjectile4";

        public override void SetDefaults()
        {

            AIType = ProjectileID.WoodenBoomerang;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;

            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

    }

    public class mixedCoralProjectile5 : ModProjectile
    {

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/mixedCoralProjectile5";

        public override void SetDefaults()
        {

            AIType = ProjectileID.WoodenBoomerang;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;

            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

    }

    public class mixedCoralProjectile6 : ModProjectile
    {

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/mixedCoralProjectile6";

        public override void SetDefaults()
        {

            AIType = ProjectileID.WoodenBoomerang;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;

            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

    }

    public class mixedCoralProjectile7 : ModProjectile
    {

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/mixedCoralProjectile7";

        public override void SetDefaults()
        {

            AIType = ProjectileID.WoodenBoomerang;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;

            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

    }

    public class mixedCoralProjectile8 : ModProjectile
    {

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/mixedCoralProjectile8";

        public override void SetDefaults()
        {

            AIType = ProjectileID.WoodenBoomerang;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;

            Projectile.penetrate += 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }

    }


}