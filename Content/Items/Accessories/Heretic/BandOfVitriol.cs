using fourClassesMod.Common;
using fourClassesMod.Common.Classes.Heretic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Accessories.Heretic
{
    internal class BandOfVitriol : ModItem
    {

        public override string Texture => $"fourClassesMod/Sprites/Accessories/BandOfVitriol";
        public static readonly int MultiplicativeDelayDecrease = 25;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.lifeRegen += 2;
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

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<HereticDamageClass>() += 0.15f;

            if (player.statLife  < player.statLifeMax2 / 2)
            {
                player.GetDamage<HereticDamageClass>() += 0.15f;
            }

            player.GetModPlayer<CardinalPlayer>().lifeCostMult -= 0.1f;

            player.PotionDelayModifier *= 0.75f;

        }
    }
}
