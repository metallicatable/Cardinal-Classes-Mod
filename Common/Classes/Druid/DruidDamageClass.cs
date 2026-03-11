using Terraria;
using Terraria.ModLoader;

namespace fourClassesMod.Common.Classes.Druid
{
    public class DruidDamageClass : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Generic)
            {
                return StatInheritanceData.Full;
            }

            return StatInheritanceData.None;
        }


        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return false;
        }

        public override bool UseStandardCritCalcs => true;
    }
}