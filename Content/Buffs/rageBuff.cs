using fourClassesMod.Common.Classes.Barbarian;
using fourClassesMod.Common.Classes.Cultist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Buffs
{
    public class rageBuff : ModBuff
    {
        int timer;

        public override string Texture => $"Terraria/Images/Item_{ItemID.Starfish}";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

            var ragePlayer = player.GetModPlayer<RagePlayer>();

            player.GetDamage(ModContent.GetInstance<BarbarianDamageClass>()) *= 1.5f;
            player.GetAttackSpeed(ModContent.GetInstance<BarbarianDamageClass>()) *= 3f;
            player.GetArmorPenetration(ModContent.GetInstance<BarbarianDamageClass>()) += 20;
            player.noKnockback = true;

        }
    }
}
