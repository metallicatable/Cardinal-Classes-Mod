using fourClassesMod.Common.Classes.Heretic;
using fourClassesMod.Content.Items.Weapons.Heretic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Prefixes
{

        public class HereticPrefix : ModPrefix
        {
        public virtual float damageMult => 1f;
        public virtual float useTimeMult => 1f;
        public virtual int critBonus => 0;
        public virtual float shootSpeedMult => 1f;
        public virtual float knockbackMult => 1f;
        public virtual float LifeMult => 2f;


        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

            public override float RollChance(Item item)
            {
                return 5f;
            }

            public override bool CanRoll(Item item)
            {
                return item.CountsAsClass(ModContent.GetInstance<HereticDamageClass>());
            }

            public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
            {
                damageMult *= this.damageMult;
                knockbackMult *= this.knockbackMult;
                useTimeMult *= this.useTimeMult;
                shootSpeedMult *= this.shootSpeedMult;
                critBonus *= this.critBonus;
            }

            public override void ModifyValue(ref float valueMult)
            {
                valueMult *= 1f + ((useTimeMult + critBonus + knockbackMult + damageMult + shootSpeedMult - LifeMult) / 3);
            }

            public override void Apply(Item item)
            {
            if (item.CountsAsClass<HereticDamageClass>() && item.TryGetGlobalItem<HereticGlobalItem>(out var hereticItem))
                hereticItem.lifeCost = LifeMult;
                item.
            }

           
            public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
            {
            // Due to inheritance, this code runs for ExamplePrefix and ExampleDerivedPrefix. We add 2 tooltip lines, the first is the typical prefix tooltip line showing the stats boost, while the other is just some additional flavor text.

            // The localization key for Mods.ExampleMod.Prefixes.PowerTooltip uses a special format that will automatically prefix + or - to the value.
            // This shared localization is formatted with the Power value, resulting in different text for ExamplePrefix and ExampleDerivedPrefix.
            // This results in "+1 Power" for ExamplePrefix and "+2 Power" for ExampleDerivedPrefix.
            // Power isn't an actual stat, the effects of Power are already shown in the "+X% damage" tooltip, so this example is purely educational.
                if (LifeMult == 1f)
                {
                    yield break;
                }

            string pos;
            if (LifeMult < 1f)
            {
                pos = "-";
            }
            else
            {
                pos = "+";
            }
                yield return new TooltipLine(Mod, "LifeCost", pos + (Math.Abs((LifeMult * 100f) - 100f)).ToString() + "% " + LifeCostDescription.Value)
                {
                    IsModifier = true,
                    IsModifierBad = LifeMult > 1f
                };
                // If possible and suitable, try to reuse the name identifier and translation value of Terraria prefixes. For example, this code uses the vanilla translation for the word defense, resulting in "-5 defense". Note that IsModifierBad is used for this bad modifier.
                /*yield return new TooltipLine(Mod, "PrefixAccDefense", "-5" + Lang.tip[25].Value) {
                    IsModifier = true,
                    IsModifierBad = true,
                };*/
            }

            // PowerTooltip is shared between ExamplePrefix and ExampleDerivedPrefix. 
            public static LocalizedText LifeCostDescription { get; private set; }

            // AdditionalTooltip shows off how to do the inheritable localized properties approach. This is necessary this this example uses inheritance and we want different translations for each inheriting class. https://github.com/tModLoader/tModLoader/wiki/Localization#inheritable-localized-properties
            public LocalizedText AdditionalTooltip => this.GetLocalization(nameof(AdditionalTooltip));

            public override void SetStaticDefaults()
            {
                // this.GetLocalization is not used here because we want to use a shared key
                LifeCostDescription = Mod.GetLocalization($"{LocalizationCategory}.{nameof(LifeCostDescription)}");
                // This seemingly useless code is required to properly register the key for AdditionalTooltip
                _ = AdditionalTooltip;
            }
        }
    
}
