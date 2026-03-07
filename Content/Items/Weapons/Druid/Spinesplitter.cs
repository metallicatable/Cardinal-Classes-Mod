using fourClassesMod.Common.Classes.Druid;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Druid
{
    internal class Spinesplitter : ModItem
    {

        private int energyCost; // Add our custom resource cost
        public override string Texture => $"fourClassesMod/Sprites/Weapons/Spinesplitter";
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true; // This makes the useStyle animate as a staff instead of as a gun.
        }
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player do the proper arm motion
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Item.autoReuse = false;
            Item.noMelee = true; // The projectile will do the damage and not the item
            Item.rare = ItemRarityID.White;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 8f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
            Item.scale = 0.85f;

            energyCost = 100;
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
            int shotsFired = 0;

            if (player.altFunctionUse == 2)
            {
                if (energyPlayer.EnergyCurrent >= energyCost)
                {
                    type = ModContent.ProjectileType<burstFireball>();
                    damage *= 2;

                    for (int i = 0; i < 5; i++)
                    {
                        perturbedVelocity = new Vector2(0, 5);
                        perturbedVelocity = perturbedVelocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(55, 85) * shotsFired));
                        perturbedVelocity *= Main.rand.NextFloat(0.33f, 0.5f);

                        Projectile.NewProjectile(source, Main.MouseWorld, perturbedVelocity, type, damage, knockback, player.whoAmI);
                        shotsFired++;
                    }

                    energyPlayer.EnergyCurrent -= energyCost;
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                type = ModContent.ProjectileType<piercingFireball>();

                Projectile.NewProjectile(source, position, velocity * 3, type, damage, knockback, player.whoAmI);
                return false;
            }               
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddIngredient(ItemID.Bone, 80)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    

    public class piercingFireball : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.ImpFireball}";
        int timer = 0;
        int dustRate = 10;
        int energyGained = 5;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Projectile.penetrate = 5; // This makes the projectile disappear after hitting an enemy once
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            dustRate = Main.rand.Next(3, 6);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            
                Color colour = Color.Orange;
                float dustSize = Main.rand.NextFloat(0.75f, 1.25f);
                int colourHandler = Main.rand.Next(0, 6);
                if (colourHandler == 0)
                {
                    colour = Color.Orange;
                }
                else if (colourHandler == 1)
                {
                    colour = Color.OrangeRed;
                }
                else if (colourHandler == 2)
                {
                    colour = Color.DarkOrange;
                }
                else if (colourHandler == 3)
                {
                    colour = Color.Red;
                }
                else if (colourHandler == 4)
                {
                    colour = Color.Gold;
                }
                else if (colourHandler == 5)
                {
                    colour = Color.Yellow;
                }
                Dust.NewDust(Projectile.position, 10, 10, DustID.Torch, -Projectile.velocity.X, 0 - Projectile.velocity.Y, 0, colour, dustSize);
            

            timer++;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Color.Yellow;
            return lightColor;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) // add energy to on hits
        {
            var energyPlayer = ModContent.GetInstance<EnergyPlayer>();
            energyPlayer.energyGain += energyGained;

        }
    }

    public class burstFireball : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.DD2FlameBurstTowerT1Shot}";
        int timer = 0;
        int dustRate = 0;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Projectile.timeLeft = 300;
            Projectile.penetrate = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            dustRate = Main.rand.Next(1, 4);
        }

        public override void AI()
        {
            if (timer % dustRate == 0)
            {
                int dustSize = Main.rand.Next(1, 3);
                Dust.NewDust(Projectile.position, 10, 10, DustID.Torch, 0, 0, 0, default, dustSize);
            }

            Projectile.velocity.Y += 0.2f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            timer++;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Color.Orange;
            return lightColor;
        }
    }
}