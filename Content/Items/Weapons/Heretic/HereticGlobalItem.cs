using fourClassesMod.Common.Classes.Heretic;
using Terraria;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Heretic
{
    public sealed class HereticGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        ///  Tracks the stealth strike damage modifier for this item, derived from its prefix. 
        internal float lifeCost = 0.0f;

        public override GlobalItem Clone(Item from, Item to)
        {
            HereticGlobalItem myClone = (HereticGlobalItem)base.Clone(from, to);

            // Heretic
            myClone.lifeCost = lifeCost;


            return myClone;
        }

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.CountsAsClass<HereticDamageClass>();
        }

        public override void PreReforge(Item item)
        {
            lifeCost = 0f;
        }
    }
}
