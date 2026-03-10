using fourClassesMod.Common.Classes.Heretic; 
using Microsoft.Xna.Framework; 
using Steamworks; 
using Terraria; 
using Terraria.DataStructures; 
using Terraria.ID; 
using Terraria.Localization; 
using Terraria.ModLoader; 
using fourClassesMod.Common; 
 
namespace fourClassesMod.Content.Items.Weapons.Heretic 
{
    internal class TaintedScroll : ModItem
    { 

        public override string Texture => $"fourClassesMod/Sprites/Weapons/Slime_Rain";

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player do the proper arm motion 
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.noMelee = true; // The projectile will do the damage and not the item 
            Item.rare = ItemRarityID.White;
            Item.shoot = ProjectileID.PurificationPowder;

            Item.shootSpeed = 5f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values 
        }

        // Make sure you can't use the item if you don't have enough resource 


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            float numberProjectiles = 3; 
            float rotation = MathHelper.ToRadians(12); 
            type = ProjectileID.EmeraldBolt;
           
            position += Vector2.Normalize(velocity) * 45f; 

            for (int i = 0; i < numberProjectiles; i++) 
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }

            HereticResourceHandler.hereticBleeds(player, 5f);
            return false; // return false to stop vanilla from calling Projectile.NewProjectile.
        }
        public override void AddRecipes() 
        {
            CreateRecipe() 
                .AddIngredient(ItemID.CrimstoneBlock, 40) 
                .AddIngredient(ItemID.Silk, 8) 
                .AddTile(TileID.Loom) 
                .Register(); 
        }
    }
}
