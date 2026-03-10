using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace fourClassesMod.Common.Classes.Heretic
{
    public class HereticResourceHandler : ModPlayer
    {

        private static float lifeMult = 1f; 
        private static float lifeFlat = 0f; 

        public static void hereticBleeds(Player player, float damage) 
        {            
            player.Hurt(player.DeathByLocalization("hereticDeathMessages." + Main.rand.Next(4)), (int)((lifeMult * damage) + lifeFlat), 0, false, false, -1, false, 1000, 1000, 0f); 
        } 
    } 
} 
