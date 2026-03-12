using fourClassesMod.Content.Items.Lore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace fourClassesMod.Common
{

    internal class CardinalPlayer : ModPlayer
    {

        #region Heretic Variables
        public float lifeCostMult = 1f;
        public int lifeCostFlat = 0;
        #endregion

        #region Inventory Management
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (!mediumCoreDeath)
            {
                return [
                    new Item(ModContent.ItemType<theCultist>(), 1),
                    new Item(ModContent.ItemType<theDruid>(), 1),
                    new Item(ModContent.ItemType<theHeretic>(), 1),
                    new Item(ModContent.ItemType<theBarbarian>(), 1),

                ];
            }

            else return null;

        }
        #endregion

        
    }
}

