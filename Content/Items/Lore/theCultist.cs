using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using fourClassesMod.Content.Extras.Rarities;

namespace fourClassesMod.Content.Items.Lore
{
    public class theCultist : ModItem
    {

        public override string Texture => $"Terraria/Images/Item_{ItemID.LunarBar}";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<lunarBlue>();
            Item.maxStack = 1;
        }
    }
}
