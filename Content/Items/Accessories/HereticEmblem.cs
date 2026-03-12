using fourClassesMod.Common.Classes.Heretic; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Terraria; 
using Terraria.ID; 
using Terraria.ModLoader; 

namespace fourClassesMod.Content.Items.Accessories 
{
    internal class HereticEmblem : ModItem 
    {

        public override string Texture => $"fourClassesMod/Sprites/Accessories/HereticEmblem"; 
        

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed; 
        }

        public override void AddRecipes() 
        {
            CreateRecipe() 
                .AddIngredient(ItemID.Bone, 40) 
                .AddIngredient(ItemID.Spike, 40) 
                .AddTile(TileID.Anvils)
                .Register(); 

        }


        public override void UpdateAccessory(Player player, bool hideVisual) 
        {
            
            
            player.GetDamage<HereticDamageClass>() += 0.15f; 

           

        }


    }
}
