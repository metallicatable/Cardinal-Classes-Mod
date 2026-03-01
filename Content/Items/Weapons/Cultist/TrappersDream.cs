using fourClassesMod.Common.Classes.Cultist;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Cultist
{
    internal class TrappersDream : ModItem
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
            Item.noMelee = true; 
            faithCost = 75;
            Item.rare = ItemRarityID.White;
            Item.shoot = ModContent.ProjectileType<dartTrapProjectileDart>();

            Item.shootSpeed = 16f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var faithPlayer = player.GetModPlayer<FaithPlayer>();
            int shotsFired = 0;
            int shotsFired2 = 0;
            int shotsFired3 = 0;
            int shotsFired4 = 0;

            if (faithPlayer.FaithCurrent >= faithCost)
            {
                Vector2 perturbedVelocity;

                for (int i = 0; i < 6; i++) // dart traps
                {

                    perturbedVelocity = new Vector2(6f, 0);
                    type = ModContent.ProjectileType<superDartTrapProjectile>();
                    Projectile.NewProjectile(source, position, perturbedVelocity.RotatedBy(MathHelper.ToRadians(60 * shotsFired)), type, damage, knockback, player.whoAmI);
                    shotsFired++;
                }

                for (int i = 0; i < 2; i++) // spike ball traps
                {
                    perturbedVelocity = new Vector2(0, -16f);
                    type = ModContent.ProjectileType<superSpikyBallTrapProjectile>();
                    Projectile.NewProjectile(source, position, perturbedVelocity.RotatedBy(MathHelper.ToRadians(-45 + (90 * shotsFired2))), type, damage, knockback, player.whoAmI);
                    shotsFired2++;
                }
                /*
                for (int i = 0; i < 18; i++) // flamethrower traps
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

    public class superFlameTrapProjectile : ModProjectile
    {

        public override string Texture => $"Terraria/Images/Item_{ItemID.FlameTrap}";
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
    }

    public class superSpikyBallTrapProjectile : ModProjectile
    {

        int startTimer = Main.rand.Next(30, 60); // Randomize the time before the spiky balls are released to add some variability to the trap  
        int timer = 0;

        public override string Texture => $"Terraria/Images/Item_{ItemID.SpikyBallTrap}";
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
            Projectile.velocity *= 0.98f; // Slow down the projectile over time to simulate it losing momentum after being fired
            Vector2 perturbedVelocity = new Vector2(0, 1f);
            perturbedVelocity.RotatedBy(MathHelper.ToRadians(-45));
            
            if (timer == startTimer)
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedVelocity, ModContent.ProjectileType<superSpikyBallTrapProjectileSpikyBall>(), 60, Projectile.knockBack, Projectile.owner);
                    perturbedVelocity.X += 1;
                    //perturbedVelocity.RotatedBy(MathHelper.ToRadians(22));
                }
            }

            if (timer >= startTimer * 2)
            {
                Projectile.Kill();
            }

            timer++;
        }

    }

    public class superSpikyBallTrapProjectileSpikyBall : ModProjectile
    {

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SpikyBallTrap}";
        public override void SetDefaults()
        {
            AIType = ProjectileID.SpikyBallTrap;  
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<CultistDamageClass>();
            Projectile.penetrate = 8;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
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
                Projectile.velocity *= 0.85f;
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
            return false;
        }
    }
    public class superDartTrapProjectile : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.SuperDartTrap}";
        int timer = 0;
        int timer2;
        int startTime = Main.rand.Next(60, 120); // Randomize the time before the first dart is fired to add some variability to the trap
        int dir;

        private NPC HomingTarget
        {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];

            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
            }
        }

        public ref float DelayTimer => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
        }
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
            Projectile.velocity = new Vector2(0, 0);
            float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target

            // A short delay to homing behavior after being fired
            if (DelayTimer < 10)
            {
                DelayTimer += 1;
                return;
            }

            // First, we find a homing target if we don't have one
            if (HomingTarget == null)
            {
                HomingTarget = FindStrongestNPC(maxDetectRadius);
            }

            // If we have a homing target, make sure it is still valid. If the NPC dies or moves away, we'll want to find a new target
            if (HomingTarget != null && !IsValidTarget(HomingTarget))
            {
                HomingTarget = null;
            }

            // If we don't have a target, don't adjust trajectory
            if (HomingTarget == null)
                return;

            // If found, we rotate the projectile velocity in the direction of the target.
            // We only rotate by 3 degrees an update to give it a smooth trajectory. Increase the rotation speed here to make tighter turns
            float length = Projectile.velocity.Length();
            float targetAngle = Projectile.AngleTo(HomingTarget.Center);
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(3)).ToRotationVector2() * length;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        // Finding the closest NPC to attack within maxDetectDistance range
        // If not found then returns null
        public NPC FindStrongestNPC(float maxDetectDistance)
        {
            NPC strongestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
            float hpSetter = 0;
            // Loop through all NPCs
            foreach (var target in Main.ActiveNPCs)
            {
                // Check if NPC able to be targeted.
                if (IsValidTarget(target))
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
                    float targetHP = target.lifeMax;

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance && targetHP > hpSetter)
                    {

                        hpSetter = targetHP;
                        strongestNPC = target;
                        
                    }
                }
            }

            return strongestNPC;
        }

        public bool IsValidTarget(NPC target)
        {
            // This method checks that the NPC is:
            // 1. active (alive)
            // 2. chaseable (e.g. not a cultist archer)
            // 3. max life bigger than 5 (e.g. not a critter)
            // 4. can take damage (e.g. moonlord core after all it's parts are downed)
            // 5. hostile (!friendly)
            // 6. not immortal (e.g. not a target dummy)
            // 7. doesn't have solid tiles blocking a line of sight between the projectile and NPC
            return target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, target.position, target.width, target.height);
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

    public class superDartTrapProjectileDart : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.PoisonDartTrap}";

        public override void SetDefaults()
        {
            Projectile.damage = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.scale = 0f;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 3;
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
}
