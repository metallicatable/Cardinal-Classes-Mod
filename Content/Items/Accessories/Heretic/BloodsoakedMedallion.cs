using fourClassesMod.Common;
using fourClassesMod.Common.Classes.Heretic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Accessories.Heretic
{
    internal class BloodsoakedMedallion : ModItem
    {

        public override string Texture => $"fourClassesMod/Sprites/Accessories/HateRing";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 5)
                .AddTile(TileID.WorkBenches)
                .Register(); 

        }


        public override void UpdateAccessory(Player player, bool hideVisual) 
        {
            player.GetModPlayer<CardinalPlayer>().lifeCostFlat -= 1;
        }


    }
}
