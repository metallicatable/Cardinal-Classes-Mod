using fourClassesMod.Common.Classes.Heretic;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Heretic
{
    public class HereticWeapon : ModItem
    {
        public virtual float lifeCost => lifeCostBase;
        internal float lifeCostBase = 1f;
        public float lifeCostReforge;
        public override string Texture => $"Terraria/Images/Item_{ItemID.IronBroadsword}";

        public override bool WeaponPrefix() => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            HereticResourceHandler.HereticBleeds(player, lifeCost);
            return true;
        }

        public override void PreReforge()
        {
            lifeCostBase = 1f;
        }

        public override void PostReforge()
        {
            base.PostReforge();
        }

    }
}
