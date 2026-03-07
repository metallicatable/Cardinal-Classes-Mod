using fourClassesMod.Common.Classes.Cultist;
using fourClassesMod.Content.Items.Weapons.Heretic;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Cultist
{
    internal class GeliticTransmutation : ModItem
    {

        private int faithCost; // Add our custom resource cost

        public override string Texture => $"fourClassesMod/Sprites/Weapons/Dandelion_Swarm";

        public override void SetDefaults()
        {
            Item.damage = 16;
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
            Item.shoot = ProjectileID.PurificationPowder;

            Item.shootSpeed = 8f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
        }


        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            var faithPlayer = player.GetModPlayer<FaithPlayer>();

            if (faithPlayer.FaithCurrent >= faithCost)
            {
                velocity = Vector2.Zero;
                type = ModContent.ProjectileType<transmutationHandler>();
                faithPlayer.FaithCurrent -= faithCost;

            }
            else
            {
                position = player.position;
                type = ModContent.ProjectileType<gelStream>();
                damage -= 6; // calculate weaker form

            }
        }
    }

    public class gelStream : ModProjectile
    {

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SlimeGun}";

        public override void SetDefaults()
        {
            AIType = ProjectileID.SlimeGun;

            Projectile.penetrate += 50;
            Projectile.timeLeft = 120;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slimed, 180);
        }

    }

    public class babySlimeClone : ModProjectile
    {

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SlimeGun}";

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabySlime);

            AIType = ProjectileID.BabySlime;

            Projectile.penetrate += 50;
            Projectile.timeLeft = 240;
            Projectile.minionSlots = 0f;
            Projectile.alpha = 0;
            Projectile.minion = false;
        }
    }

    public class transmutationHandler : ModProjectile
    {

        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.WoodenArrowFriendly}";

        public override void SetDefaults()
        {
            Projectile.penetrate += 50;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.hostile = false;
        }



        public override void AI()
        {
            Projectile.velocity = new Vector2(0, 0);
            float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectRadius * maxDetectRadius;

            int counter = 0;
            int projectileMax = 5;

            // Loop through all NPCs
            foreach (var target in Main.ActiveNPCs)
            {
                // Check if NPC able to be targeted.
                if (IsValidTarget(target))
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);


                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance && counter <= projectileMax)
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            target.scale *= 0.93f;
                        }
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), target.position, Vector2.Zero, ProjectileID.BabySlime, 60, 2f);
                        target.StrikeInstantKill();

                        counter++;
                    }

                    if (counter >= projectileMax)
                    {
                        Projectile.Kill();
                    }
                }
            }

        }

        // Finding the closest NPC to attack within maxDetectDistance range
        // If not found then returns null
        

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
            if (!target.boss)
            {
                return target.CanBeChasedBy();
            }
            return false;
        }

    }
}
