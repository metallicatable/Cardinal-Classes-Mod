using fourClassesMod.Common;
using fourClassesMod.Common.Classes.Druid;
using fourClassesMod.Content.Extras.Rarities;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
/*
    NOTES
Create aura projectile when sprite and design is done.
Change useStyle when main sprite is done.
Stats, no balancing has been done.


*/
namespace fourClassesMod.Content.Items.Weapons.Druid // tells the game where to find this file
{
    internal class WintersWarmth : DruidWeapon // tells the game that this is an item
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/WintersWarmth"; //for using vanilla sprites
        // public override string Texture => $"fourClassesMod/Sprites/Weapons/Dandelion_Swarm"; this is for a modded sprite, use the correct file path


        public override void SetDefaults()
        {
            Item.damage = 2;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Shoot; // handles anim
            Item.useAnimation = 2; //keep useAnimation and useTime identical for most things, will cost clockwork assault rifle stuff if they arent the same
            Item.useTime = 2;
            Item.width = 32; //hitbox size for melee stuff
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<DruidDamageClass>(); // tells the game the damage type
            Item.noMelee = true; // The projectile will do the damage and not the item
            energyCost = 100;
            Item.rare = ItemRarityID.White; //rarity
            // Item.rare = ModContent.RarityType<lunarBlue>();    for modded rarities
            Item.shoot = ProjectileID.PurificationPowder; // keep as purification powder always, to stay consistent

            Item.shootSpeed = 12f; // affects Projectile.velocity
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        { // overrides the shoot command to have different effects for left and right click, as well as allowing energy to work as intended.

            var energyPlayer = player.GetModPlayer<EnergyPlayer>(); // creates a variable based on the EnergyPlayer in common/druid so that energy can be influenced 
            Vector2 perturbedVelocity; // new variable we can use to edit velocity without breaking the game

            if (player.altFunctionUse == 2) // if player is right-clicking
            {
                if (energyPlayer.EnergyCurrent >= energyCost) // if player has enough energy
                {
                    type = ModContent.ProjectileType<campfireProjectile>(); // change projectile type from purification powder to the modded burstFireball

                    EnergyAttack(player, source, position, velocity / 2, type, 0, knockback);

                    energyPlayer.EnergyCurrent -= energyCost; // lower energy after using big attacl
                    return false; // tell the game to not use the vanilla shoot behaviour so ours can run instead
                }
                else
                {
                    return false; // functionality for when the weapon is right clicked but not enough energy is present
                }
            }
            else // what to do when left clicked
            {
                type = ModContent.ProjectileType<snowBallClone>(); // fires wooden arrows when left clicks

                perturbedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5)); // This adds a random spread to the projectiles, 5 degrees in either direction.
                perturbedVelocity *= 2f + Main.rand.NextFloat(-0.5f, 0.5f); // randomizes velocity by at minumum 1.5x and maximum 2.5x

                FastAttack(player, source, position, perturbedVelocity, type, damage, knockback); // create the projectile
                return false; // tell the game not to create the projectile itself, as we just did
            }               
        }

        
        public override void AddRecipes()     // creates the recipe, which is commented out as this weapon should not be obtainable and therefore has no recipe
        {
            CreateRecipe()
                .AddIngredient(ItemID.FlinxFur, 5)
                .AddRecipeGroup(RecipeGroupSystem.campfireGroup, 1)
                .AddIngredient(ItemID.Silk, 8)
                .AddTile(TileID.Loom)
                .Register();
        }

        
    }

    public class snowBallClone : DruidProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.SnowBallFriendly}";



        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            energyGain = 4;
            
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.15f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }
    }

    public class campfireProjectile  : DruidProjectile
    {
        int timer = 0;
        public override string Texture => $"fourClassesMod/Sprites/Projectiles/campfireClone";
        bool tileCollided = false;
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 40;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.scale = 0.5f;
        }

        public override void SetStaticDefaults()
        {
            // Total count animation frames
            Main.projFrames[Type] = 8;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = new Vector2(0, 0);
            tileCollided = true;
            return false;
        }


        public override void AI()
        {
            
            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0.05f);
            if (!tileCollided)
            {
                Projectile.velocity.Y += 0.25f; // gravity
            }
           
            int effectRadius = 300;
            Player player = Main.player[Projectile.owner];
            if (timer % 3 == 0)
            {
                Vector2 perturbedPosition = new Vector2(Projectile.position.X + 24, Projectile.position.Y);
                Dust.NewDust(perturbedPosition, 48, 16, DustID.Smoke, 0, -0.25f, 100, default, 1f);
            }

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Type];
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;

                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }

                foreach (var target in Main.ActiveNPCs)
                {
                    // Check if NPC able to be targeted.
                    if (IsValidTarget(target))
                    {
                        // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                        float distanceToTarget = Vector2.Distance(target.Center, Projectile.Center);

                        // Check if it is within the radius
                        if (distanceToTarget < effectRadius)
                        {
                            target.AddBuff(BuffID.OnFire, 300);

                        }
                    }
                }

                if (timer % 30 == 0)
                {
                    if (player.position.X - Projectile.position.X <= effectRadius && player.position.Y - Projectile.position.Y <= effectRadius)
                    {
                        player.Heal(1);
                    }
                }

                if (timer > 300)
                {
                    Projectile.Kill();
                }

                timer++;
            }
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
    }

    
    
}