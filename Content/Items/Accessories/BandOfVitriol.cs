using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Accessories
{
    internal class BandOfVitriol : ModItem
    {

        public override string Texture => $"fourClassesMod/Sprites/Accessories/BandOfVitriol";
        public static readonly int MultiplicativeDelayDecrease = 25;
        public override void SetDefaults()
        {
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<HateRing>() 
                .AddIngredient<BandOfVitality>() 
                .AddIngredient(ItemID.PhilosophersStone) 
                .AddIngredient(ItemID.AvengerEmblem) 
                .AddIngredient(ItemID.HallowedBar, 6) 
                .AddTile(TileID.MythrilAnvil) 
                .Register(); 

        }
    }
}
