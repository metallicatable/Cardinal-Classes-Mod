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
    public class HereticResourceHandler : ModSystem
    {


        public static void HereticBleeds(Player player, float damage) 
        {            
            CardinalPlayer cardinal = player.GetModPlayer<CardinalPlayer>();
            player.Hurt(player.DeathByLocalization("hereticDeathMessages." + Main.rand.Next(4)), (int)((cardinal.lifeCostMult * damage) + cardinal.lifeCostFlat), 0, false, false, -1, false, 1000, 1000, 0f); 
        } 
    } 
} 
