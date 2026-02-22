using fourClassesMod.Common.Classes.Cultist;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Cultist
{
    internal class DandelionSwarm : ModItem
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
                velocity.X = 0;
                velocity.Y = 0;
                type = ModContent.ProjectileType<ChargedDandelionSwarmSeeds>();
                faithPlayer.FaithCurrent -= faithCost;

            }
            else
            {
                position = player.position;
                type = ModContent.ProjectileType<DandelionSwarmSeeds>();
                damage -= 6; // calculate weaker form

            }
        }
    }

    public class DandelionSwarmSeeds : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // Total count animation frames
            Main.projFrames[Type] = 4;
        }

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/DandelionSwarmSeeds";

        public override void SetDefaults()
        {
            // This method right here is the backbone of what we're doing here; by using this method, we copy all of
            // the Meowmere Projectile's SetDefault stats (such as projectile.friendly and projectile.penetrate) on to our projectile,
            // so we don't have to go into the source and copy the stats ourselves. It saves a lot of time and looks much cleaner;
            // if you're going to copy the stats of a projectile, use CloneDefaults().
            AIType = ProjectileID.DandelionSeed;
            Projectile.friendly = true;
            // To further the Cloning process, we can also copy the ai of any given projectile using AIType, since we want
            // the projectile to essentially behave the same way as the vanilla projectile.
            // After CloneDefaults has been called, we can now modify the stats to our wishes, or keep them as they are.
            // For the sake of Cultist, lets make our projectile penetrate enemies a few more times than the vanilla projectile.
            // This can be done by modifying projectile.penetrate
            Projectile.penetrate += 3;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;

        }

        public override void AI()
        {
            Vector2 spawnPosition = Projectile.Center;

            // All projectiles have timers that help to delay certain events
            // Projectile.ai[0], Projectile.ai[1] — timers that are automatically synchronized on the client and server
            // Projectile.localAI[0], Projectile.localAI[0] — only on the client
            // In this example, a timer is used to control the fade in / out and despawn of the projectile
            Projectile.ai[0] += 1f;

            // Slow down
            Projectile.velocity *= 0.96f;

            // Loop through the 4 animation frames, spending 5 ticks on each
            // Projectile.frame — index of current frame
            if (++Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Type];
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }

            // Despawn this projectile after 1 second (60 ticks)
            // You can use Projectile.timeLeft = 60f in SetDefaults() for same goal
            if (Projectile.ai[0] >= 240f)
                Projectile.Kill();

            // Set both direction and spriteDirection to 1 or -1 (right and left respectively)
            // Projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
        }
    }

    public class ChargedDandelionSwarmSeeds : ModProjectile
    {

        int timer;

        public override string Texture => $"fourClassesMod/Sprites/Projectiles/ChargedDandelionSwarmSeeds";
        public override void SetDefaults()
        {
            // This method right here is the backbone of what we're doing here; by using this method, we copy all of
            // the Meowmere Projectile's SetDefault stats (such as projectile.friendly and projectile.penetrate) on to our projectile,
            // so we don't have to go into the source and copy the stats ourselves. It saves a lot of time and looks much cleaner;
            // if you're going to copy the stats of a projectile, use CloneDefaults().
            Projectile.friendly = true;
            // To further the Cloning process, we can also copy the ai of any given projectile using AIType, since we want
            // the projectile to essentially behave the same way as the vanilla projectile.
            // After CloneDefaults has been called, we can now modify the stats to our wishes, or keep them as they are.
            // For the sake of Cultist, lets make our projectile penetrate enemies a few more times than the vanilla projectile.
            // This can be done by modifying projectile.penetrate
            Projectile.penetrate += 8;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.timeLeft = 180;
        }


        public override void AI()
        {
            if (timer++ > 10)
            {
                Vector2 helperVelocity;
                Vector2 spawnPosition = Projectile.Center;

                spawnPosition.X -= 1000 + Main.rand.Next(-240, 240); //spawn in the center of the main projectile
                spawnPosition.Y += Main.rand.Next(-500, 750);

                helperVelocity = new Vector2(20 + Main.rand.Next(-10, 10), 0);

                Projectile.NewProjectile(Projectile.InheritSource(Projectile), spawnPosition, helperVelocity, ModContent.ProjectileType<DandelionSwarmSeedsChargeClone>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            }
        }
    }

    public class DandelionSwarmSeedsChargeClone : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Total count animation frames
            Main.projFrames[Type] = 4;
        }
        public override string Texture => $"fourClassesMod/Sprites/Projectiles/DandelionSwarmSeedsChargeClone";

        public override void SetDefaults()
        {
            // This method right here is the backbone of what we're doing here; by using this method, we copy all of
            // the Meowmere Projectile's SetDefault stats (such as projectile.friendly and projectile.penetrate) on to our projectile,
            // so we don't have to go into the source and copy the stats ourselves. It saves a lot of time and looks much cleaner;
            // if you're going to copy the stats of a projectile, use CloneDefaults().
            AIType = ProjectileID.DandelionSeed;
            Projectile.friendly = true;
            // To further the Cloning process, we can also copy the ai of any given projectile using AIType, since we want
            // the projectile to essentially behave the same way as the vanilla projectile.
            // After CloneDefaults has been called, we can now modify the stats to our wishes, or keep them as they are.
            // For the sake of Cultist, lets make our projectile penetrate enemies a few more times than the vanilla projectile.
            // This can be done by modifying projectile.penetrate
            Projectile.penetrate += 3;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.knockBack = 2f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Color.White;
            return lightColor;
        }

        public override void AI()
        {
            // All projectiles have timers that help to delay certain events
            // Projectile.ai[0], Projectile.ai[1] — timers that are automatically synchronized on the client and server
            // Projectile.localAI[0], Projectile.localAI[0] — only on the client
            // In this example, a timer is used to control the fade in / out and despawn of the projectile
            Projectile.ai[0] += 1f;

            // Loop through the 4 animation frames, spending 5 ticks on each
            // Projectile.frame — index of current frame
            if (++Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Type];
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }

            // Despawn this projectile after 1 second (60 ticks)
            // You can use Projectile.timeLeft = 60f in SetDefaults() for same goal
            if (Projectile.ai[0] >= 360f)
            {
                Projectile.Kill();
            }
            // Set both direction and spriteDirection to 1 or -1 (right and left respectively)
            // Projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

            Projectile.rotation = Projectile.velocity.ToRotation();
            // Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }
        }



    }
}
