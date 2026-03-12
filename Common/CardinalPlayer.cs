using fourClassesMod.Common.Classes.Heretic;
using fourClassesMod.Content.Items.Lore;
using fourClassesMod.Content.Prefixes;
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
    public class CardinalPlayer : ModPlayer
    {
        #region Heretic Variables
        public float lifeCostMult = 1f;
        public int lifeCostFlat = 0;
        #endregion

        #region Heretic Functions
        public override void ResetEffects()
        {
            lifeCostFlat = 0;
            lifeCostMult = 1f;
        }

        public override bool CanUseItem(Item item)
        {
            if (item.CountsAsClass<HereticDamageClass>())
            {
                if (item.prefix == ModContent.PrefixType<DoubleEdged>())
                {
                    lifeCostMult += 0.2f;
                }

                if (item.prefix == ModContent.PrefixType<Safe>())
                {
                    lifeCostMult -= 0.25f;
                }

                if (item.prefix == ModContent.PrefixType<Draining>())
                {
                    lifeCostMult += 0.3f;
                }

                if (item.prefix == ModContent.PrefixType<Visceral>())
                {
                    lifeCostMult -= 0.15f;
                }

                if (item.prefix == ModContent.PrefixType<Painful>())
                {
                    lifeCostMult += 0.3f;
                }
            }
            return true;
        }
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

