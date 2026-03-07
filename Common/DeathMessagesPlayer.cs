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
    public class DeathMessagesPlayer : ModPlayer 
    {
        public string hereticDeathMessages;

        public override void OnHurt(Player.HurtInfo info)
        {

            int messageHelper = Main.rand.Next(0, 4);
            if (messageHelper == 0)
            {
                hereticDeathMessages = $"{Player.name}'s blood became dry of mana, unable to sustain its body.";
            }
            if (messageHelper == 1)
            {
                hereticDeathMessages = $"{Player.name} overdrew their own life essence.";
            }
            if (messageHelper == 2)
            {
                hereticDeathMessages = $"{Player.name} got greedy.";
            }
            if (messageHelper == 3)
            {
                hereticDeathMessages = $"{Player.name} was consumed by their own magiks.";
            }
            else
            {
                hereticDeathMessages = $"bro died.";
            }
        }

    }
}
