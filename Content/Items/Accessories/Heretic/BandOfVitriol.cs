<<<<<<< HEAD:Content/Items/Accessories/BandOfVitriol.cs
﻿using fourClassesMod.Common.Classes.Heretic;
=======
﻿using fourClassesMod.Common;
using fourClassesMod.Common.Classes.Heretic;
>>>>>>> 637b7f898e9846cabc5d105b1a4b5f17766ed10d:Content/Items/Accessories/Heretic/BandOfVitriol.cs
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
        
        public override void SetDefaults()
        {
            Item.accessory = true;
//HEAD:Content/Items/Accessories/BandOfVitriol.cs//
            Item.rare = ItemRarityID.Lime;

            Item.lifeRegen += 2;
 //637b7f898e9846cabc5d105b1a4b5f17766ed10d:Content/Items/Accessories/Heretic/BandOfVitriol.cs//
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
            if (player.statLife < 0.5 * player.statLifeMax2)
            {
                player.GetDamage<HereticDamageClass>() += 0.15f;
            }


        }

// HEAD:Content/Items/Accessories/BandOfVitriol.cs
    
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
//637b7f898e9846cabc5d105b1a4b5f17766ed10d:Content/Items/Accessories/Heretic/BandOfVitriol.cs
    }
}
