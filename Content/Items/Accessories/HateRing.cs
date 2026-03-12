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
    internal class HateRing : ModItem
    {

        public override string Texture => $"fourClassesMod/Sprites/Accessories/HateRing";
        public static readonly int MultiplicativeDelayDecrease = 25;

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
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
            if (player.statLife < 0.5 * player.statLifeMax2)
            {
                player.GetDamage<HereticDamageClass>() += 0.15f; 

            }

        }


    }
}
