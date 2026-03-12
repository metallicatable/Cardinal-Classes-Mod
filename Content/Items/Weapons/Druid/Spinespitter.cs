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
    internal class Spinespitter : DruidWeapon // tells the game that this is an item
    {

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
            Item.shoot = ProjectileID.PurificationPowder; // keep as purification powder always, to stay consistent

            Item.shootSpeed = 12f; // affects Projectile.velocity

        }
        
        public override void AddRecipes()     // creates the recipe, which is commented out as this weapon should not be obtainable and therefore has no recipe
        {
            CreateRecipe()
                .AddIngredient(ItemID.AntlionMandible, 4)
                .AddIngredient(ItemID.Cactus, 50)
                .AddIngredient(ItemID.FossilOre, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        { // overrides the shoot command to have different effects for left and right click, as well as allowing energy to work as intended.

            var energyPlayer = player.GetModPlayer<EnergyPlayer>(); // creates a variable based on the EnergyPlayer in common/druid so that energy can be influenced 
            Vector2 perturbedVelocity; // new variable we can use to edit velocity without breaking the game

            if (player.altFunctionUse == 2) // if player is right-clicking
            {
                if (energyPlayer.EnergyCurrent >= energyCost) // if player has enough energy
                {
                    type = ModContent.ProjectileType<SpineBall>(); // change projectile type from purification powder to the modded burstFireball

                    EnergyAttack(player, source, position, velocity / 2, type, 0, knockback, player.whoAmI);

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

                FastAttack(player, source, position, perturbedVelocity, type, damage, knockback, player.whoAmI); // create the projectile
                return false; // tell the game not to create the projectile itself, as we just did
            }
        }


    }
    

    public class Spine : ModProjectile // add a projectile to this file for the weapon to fire
    {
        int energyGained = 1;
        public override string Texture => "fourClassesMod/Sprites/Projectiles/Spine"; // why not
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var energyPlayer = player.GetModPlayer<EnergyPlayer>();

            energyPlayer.EnergyCurrent += energyGained;
        }

        public override void AI() // called every frame
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // make the projectile face the direction it's moving

            Projectile.velocity.Y += 0.6f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }
    }

    public class SpineBall : ModProjectile // add a projectile to this file for the weapon to fire
    {
        public override string Texture => "fourClassesMod/Sprites/Projectiles/SpineBall"; // why not
        int timer = 1; // timer used in the projectile AI
        int rotationHelper = 1;
        


        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<DruidDamageClass>();
            Projectile.penetrate = -1; // This makes the projectile disappear after hitting an enemy once
            Projectile.timeLeft = 300; // ticks until despawn
            Projectile.usesLocalNPCImmunity = true; // uses local iframes
        }



        public override void AI() // called every frame
        {

            if (timer % 1 == 0)
            {
                rotationHelper++;
            }


            if (timer > 30)
            {
                Vector2 perturbedSpeed = new Vector2(0, 11);
                int rotationCoeff = Main.rand.Next(0, 91);

                perturbedSpeed = perturbedSpeed.RotatedBy(rotationCoeff);
                for (int i = 0; i < 5; i++)
                {
                    perturbedSpeed = perturbedSpeed.RotatedBy(MathHelper.ToRadians(72));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, perturbedSpeed, ModContent.ProjectileType<Spine>(), 20, 1f);
                }

                Projectile.Kill();
            }

            
            Projectile.rotation += MathHelper.ToRadians(rotationHelper);

            timer++;
        }
    }
}