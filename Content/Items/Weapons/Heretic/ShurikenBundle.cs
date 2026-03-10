using fourClassesMod.Common.Classes.Heretic; 
using Microsoft.Xna.Framework; 
using Steamworks; 
using Terraria; 
using Terraria.DataStructures; 
using Terraria.ID; 
using Terraria.Localization; 
using Terraria.ModLoader; 
using fourClassesMod.Common;
using System;

namespace fourClassesMod.Content.Items.Weapons.Heretic 
{
    internal class ShurikenBundle : ModItem
    {


        public override string Texture => $"fourClassesMod/Sprites/Weapons/Shuriken_Bundle"; 

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Swing; // Makes the player do the proper arm motion 
            Item.useAnimation = 12; 
            Item.useTime = 60;
            Item.width = 16;
            Item.height = 16; 
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.noMelee = true; // The projectile will do the damage and not the item 
            Item.rare = ItemRarityID.White;
            Item.shoot = ProjectileID.PurificationPowder;

            Item.shootSpeed = 20f; 
        }

        // Make sure you can't use the item if you don't have enough resource 


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            float numberProjectiles = 5;
            float rotation = MathHelper.ToRadians(0); 
            type = ProjectileID.Shuriken;
            HereticResourceHandler.hereticBleeds(player, 5f);


            position += Vector2.Normalize(velocity) * 45f; 

            for (int i = 0; i < numberProjectiles; i++) 
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(35)); // This adds a random spread to the projectiles, 10 degrees in either direction.
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI); 
            }



            return false; 
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Shuriken, 100) 
                .Register();
        }
    }
}
