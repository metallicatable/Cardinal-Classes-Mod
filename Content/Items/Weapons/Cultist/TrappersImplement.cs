using fourClassesMod.Common.Classes.Cultist;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Cultist
{
    internal class TrappersImplement : ModItem
    {

        private int faithCost; // Add our custom resource cost

        public override string Texture => $"fourClassesMod/Sprites/Weapons/Slime_Rain";

        public override void SetDefaults()
        {
            Item.damage = 40;
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
            Item.shoot = ModContent.ProjectileType<dartTrapProjectileDart>();

            Item.shootSpeed = 16f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        }

        // Make sure you can't use the item if you don't have enough resource


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var faithPlayer = player.GetModPlayer<FaithPlayer>();
            int shotsFired = 0;

            if (faithPlayer.FaithCurrent >= faithCost)
            {
                Vector2 perturbedVelocity = new Vector2(4f, 0f);
                type = ModContent.ProjectileType<dartTrapProjectile>();
                damage *= 2;

                for (int i = 0; i < 6; i++) // 6 spread shots
                {

                    Projectile.NewProjectile(source, position, perturbedVelocity.RotatedBy(MathHelper.ToRadians(60 * shotsFired)), type, damage, knockback, player.whoAmI);
                    shotsFired++;
                }

                for (int i = 0; i < 6; i++) // 6 spread shots numero uno
                {
                    Projectile.NewProjectile(source, position, (perturbedVelocity.RotatedBy(MathHelper.ToRadians(15 + 60 * shotsFired)) * 2f), type, damage, knockback, player.whoAmI);
                    shotsFired++;
                }
                /*
                for (int i = 0; i < 18; i++) // shotgun shots
                {
                    Projectile.NewProjectile(source, position, (velocity.RotatedByRandom(MathHelper.ToRadians(15)) * Main.rand.NextFloat(0.2f, 2f)), ModContent.ProjectileType<dartTrapProjectileDart>(), damage, knockback, player.whoAmI);

                }
                */
                faithPlayer.FaithCurrent -= faithCost; // Consume the resource
                return false;

            }
            else
            {
                return true;
            }

        }
    }   
    
    public class dartTrapProjectile : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.DartTrap}";
        int timer = 0;
        int timer2;
        int dartsFired = 0;
        int startTime = Main.rand.Next(0, 60); // Randomize the time before the first dart is fired to add some variability to the trap
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.damage = 0;
            Projectile.DamageType = ModContent.GetInstance<CultistDamageClass>();
            Projectile.penetrate = 100;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.98f; // Simulate gravity
            Vector2 dartVelocity = new Vector2(8, 0);
            if (timer2 >= startTime)
            {
                if (timer % 30 == 0)
                {
                    dartVelocity = dartVelocity.RotatedBy(MathHelper.ToRadians(60 * dartsFired));
                    dartVelocity *= Main.rand.NextFloat(0.8f, 1.2f); // Add some randomness to the speed of the darts
                    dartVelocity.RotatedByRandom(MathHelper.ToRadians(10)); // Add some randomness to the direction of the darts
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dartVelocity, ModContent.ProjectileType<dartTrapProjectileDart>(), 80, Projectile.knockBack, Projectile.owner);
                    dartsFired++;
                }

                if (timer % 15 == 0 && timer % 30 != 0)
                {
                    Projectile.rotation += MathHelper.ToRadians(60);
                }

                if (timer >= 240)
                {
                    Projectile.Kill();
                }


                timer++;
            }
            timer2++;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.ai[0] += 0.1f;
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                Projectile.velocity *= 0.75f;
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
            return false;
        }
    }

    public class dartTrapProjectileDart : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.PoisonDart}";

        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.scale = 0f;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.scale < 1f)
            {
                Projectile.scale += 0.25f; // Scale up the projectile over time to simulate it coming out of the trap
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }
    }
}
