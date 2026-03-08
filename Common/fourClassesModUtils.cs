using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace fourClassesMod.Common
{
    static class fourClassesModUtils
    {
        public static PlayerDeathReason DeathByLocalization(this Player p, string key)
        {
            NetworkText death = Language.GetText($"Mods.fourClassesMod.{key}").WithFormatArgs(p.name).ToNetworkText();
            return PlayerDeathReason.ByCustomReason(death);
        }
    }
}
