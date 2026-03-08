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
    internal class Spinespitter : ModItem // tells the game that this is an item
    {

        private int energyCost; // energy cost per right click
        public override string Texture => $"fourClassesMod/Sprites/Weapons/Spinespitter"; //for using vanilla sprites
        // public override string Texture => $"fourClassesMod/Sprites/Weapons/Dandelion_Swarm"; this is for a modded sprite, use the correct file path
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Swing; // handles anim
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
                    type = ModContent.ProjectileType<SpinesBall>(); // change projectile type from purification powder to the modded burstFireball

                    Projectile.NewProjectile(source, position, velocity / 2, type, 0, knockback, player.whoAmI);

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
    

    public class Spine : ModProjectile // add a projectile to this file for the weapon to fire
    {
        int energyGained = 1;
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.RollingCactusSpike}"; // why not
        int timer = 0; // timer used in the projectile AI
        int dustRate = 10; // variable used in the projectile AI to spawn dust

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Projectile.penetrate = 1; // This makes the projectile disappear after hitting an enemy once
            Projectile.timeLeft = 300; // ticks until despawn
            Projectile.usesLocalNPCImmunity = true; // uses local iframes
            Projectile.localNPCHitCooldown = -1; // -1 means each projectile can hit an enemy once only
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) // add energy to on hits
        {
            var energyPlayer = ModContent.GetInstance<EnergyPlayer>();
            energyPlayer.energyGain += energyGained;

        }

        public override void AI() // called every frame
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; // make the projectile face the direction it's moving
        }
    }

    public class SpinesBall : ModProjectile // add a projectile to this file for the weapon to fire
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.RollingCactus}"; // why not
        int timer = 1; // timer used in the projectile AI
        int rotationHelper = 1;
        


        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Projectile.penetrate = -1; // This makes the projectile disappear after hitting an enemy once
            Projectile.timeLeft = 300; // ticks until despawn
            Projectile.usesLocalNPCImmunity = true; // uses local iframes
            Projectile.localNPCHitCooldown = -1; // -1 means each projectile can hit an enemy once only
            Vector2 projectileVector = new Vector2(0, Projectile.width);
        }



        public override void AI() // called every frame
        {
            Vector2 projectileVector = new Vector2(0, Projectile.width / 2);
            projectileVector = projectileVector.RotatedBy(Projectile.rotation);

            if (timer % 3 == 0)
            {
                rotationHelper++;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position + projectileVector, new Vector2(8, 0).RotatedBy(Projectile.rotation), ModContent.ProjectileType<Spine>(), 10, 1f);
            }

            
            Projectile.rotation += MathHelper.ToRadians(rotationHelper);
            Projectile.velocity *= 0.97f;

            timer++;
        }
    }
}