using fourClassesMod.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Accessories.Heretic
{
    internal class BandOfVitality : ModItem
    {
        public override string Texture => $"fourClassesMod/Sprites/Accessories/BandOfVitality";
        public static readonly int HereticCritBonus = 10;

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.lifeRegen += 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CardinalPlayer>().lifeCostMult -= 0.1f;
        }
    }
}
