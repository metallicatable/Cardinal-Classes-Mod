using fourClassesMod.Common.Classes.Druid;
using fourClassesMod.Content.Extras.Rarities;
using Microsoft.Xna.Framework;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Druid // tells the game where to find this file
{
    internal class DruidItemName : ModItem // tells the game that this is an item
    {

        private int energyCost; // energy cost per right click
        public override string Texture => $"Terraria/Images/Item_{ItemID.IronBroadsword}"; //for using vanilla sprites
        // public override string Texture => $"fourClassesMod/Sprites/Weapons/Dandelion_Swarm"; this is for a modded sprite, use the correct file path
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Swing; // handles anim
            Item.useAnimation = 5; //keep useAnimation and useTime identical for most things, will cost clockwork assault rifle stuff if they arent the same
            Item.useTime = 5;
            Item.width = 32; //hitbox size for melee stuff
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<DruidDamageClass>(); // tells the game the damage type
            Item.noMelee = true; // The projectile will do the damage and not the item
            energyCost = 100;
            Item.rare = ItemRarityID.White; //rarity
            // Item.rare = ModContent.RarityType<lunarBlue>();    for modded rarities
            Item.shoot = ProjectileID.PurificationPowder; // keep as purification powder always, to stay consistent

            Item.shootSpeed = 8f; // affects Projectile.velocity
        }

        public override bool AltFunctionUse(Player player) 
        {
            return true; // allows right click to have functionality
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        { // overrides the shoot command to have different effects for left and right click, as well as allowing energy to work as intended.

            var energyPlayer = player.GetModPlayer<EnergyPlayer>(); // creates a variable based on the EnergyPlayer in common/druid so that energy can be influenced 
            Vector2 perturbedVelocity; // new variable we can use to edit velocity without breaking the game

            if (player.altFunctionUse == 2) // if player is right-clicking
            {
                if (energyPlayer.EnergyCurrent >= energyCost) // if player has enough energy
                {
                    for (int i = 0; i < 5; i++) // execute the following code 5 times
                    {
                        perturbedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(10)); // random rotation, 10 degrees total i think (5 up, 5 down)
                        perturbedVelocity *= Main.rand.NextFloat(0.25f, 5f);  // randomize velocity, at minimum 25% of base speed and maximum 5 times base speed
                        type = ModContent.ProjectileType<burstFireball>(); // change projectile type from purification powder to the modded burstFireball
                        damage *= 2; // double the damage listed in SetDefaults()

                        Projectile.NewProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI);
                    } // ^ create projectile with the parameters in the brackets

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
                type = ProjectileID.WoodenArrowFriendly; // fires wooden arrows when left clicks

                perturbedVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5)); // This adds a random spread to the projectiles, 5 degrees in either direction.
                perturbedVelocity *= 2f + Main.rand.NextFloat(-0.5f, 0.5f); // randomizes velocity by at minumum 1.5x and maximum 2.5x

                Projectile.NewProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI); // create the projectile
                return false; // tell the game not to create the projectile itself, as we just did
            }               
        }

        /*
        public override void AddRecipes()     // creates the recipe, which is commented out as this weapon should not be obtainable and therefore has no recipe
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddIngredient(ItemID.Bone, 80)
                .AddTile(TileID.Anvils)
                .Register();
        }

        */
    }
    

    public class druidExampleProjectile : ModProjectile // add a projectile to this file for the weapon to fire
    {
        int energyGained = 1;
        public override string Texture => $"Terraria/Images/Tiles_{TileID.AncientBlueBrick}"; // why not
        int timer = 0; // timer used in the projectile AI
        int dustRate = 10; // variable used in the projectile AI to spawn dust

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Projectile.penetrate = 2; // This makes the projectile disappear after hitting an enemy once
            Projectile.timeLeft = 300; // ticks until despawn
            Projectile.usesLocalNPCImmunity = true; // uses local iframes
            Projectile.localNPCHitCooldown = -1; // -1 means each projectile can hit an enemy once only
        }

        public override void OnSpawn(IEntitySource source) // called when the projectile spawns
        {
            dustRate = Main.rand.Next(1, 4); // randomize the rate at which dust is spawned
        }


        public override void AI() // called every frame
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; // make the projectile face the direction it's moving
            if (timer % dustRate == 0) // %, or modulus, returns the remainder when divided. In this case, if the timer divided by the dustRate with a remainder of zero, 
            { //                          dust is spawned.
                int dustSize = Main.rand.Next(1, 3); // randomzed dust scale
                Dust.NewDust(Projectile.position, 10, 10, DustID.Torch, 0, 0, 0, default , dustSize); // create the dust
            }

            timer++; // increases the timer every frame
        }

        public override Color? GetAlpha(Color lightColor)
        {
            lightColor = Color.Orange; // set the projectile to orange, making it always appear to glow. Only for projectiles that glow / should be lit
            return lightColor;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var energyPlayer = player.GetModPlayer<EnergyPlayer>();

            energyPlayer.EnergyCurrent += energyGained;
        }
    }
}