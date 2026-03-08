using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace fourClassesMod.Common
{
    public class RecipeGroupSystem : ModSystem
    {

        public static RecipeGroup campfireGroup;

        public override void Unload()
        {
            campfireGroup = null;
        }

        public override void AddRecipeGroups()
        {
            campfireGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.Campfire)}",
                ItemID.Campfire,
                ItemID.BoneCampfire,
                ItemID.CoralCampfire,
                ItemID.CorruptCampfire,
                ItemID.CrimsonCampfire,
                ItemID.CursedCampfire,
                ItemID.DemonCampfire,
                ItemID.DesertCampfire,
                ItemID.FrozenCampfire,
                ItemID.HallowedCampfire,
                ItemID.IchorCampfire,
                ItemID.JungleCampfire,
                ItemID.MushroomCampfire,
                ItemID.RainbowCampfire,
                ItemID.ShimmerCampfire,
                ItemID.UltraBrightCampfire);

            RecipeGroup.RegisterGroup(nameof(ItemID.Campfire), campfireGroup);

        }
    }
}
