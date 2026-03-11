using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using fourClassesMod.Common.Classes.Druid;
using Terraria.ID;
using System.Configuration;

namespace fourClassesMod.Content.Items.Weapons.Druid
{
    public class DruidWeapon : ModItem
    {
        public float energyAttackBoost;
        public int energyAttackFlat;

        public float fastAttackBoost;
        public int fastAttackFlat;

        public int energyCost;


        public override string Texture => $"Terraria/Images/Item_{ItemID.IronBroadsword}";

        public override bool WeaponPrefix() => true;

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            var energyPlayer = player.GetModPlayer<EnergyPlayer>();

            if (player.altFunctionUse == 2 && energyPlayer.EnergyCurrent < energyCost)
            {
                return false;
            }

            return true;
        }

        public void EnergyAttack(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai = 0f)
        {
            var energyPlayer = player.GetModPlayer<EnergyPlayer>();

            energyAttackBoost = energyPlayer.energyAttackBoost;
            energyAttackFlat = energyPlayer.energyAttackFlat;

            damage = (int)((float)damage * energyAttackBoost);
            damage += energyAttackFlat;

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
        }

        public void FastAttack(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai = 0f)
        {
            var energyPlayer = player.GetModPlayer<EnergyPlayer>();

            fastAttackBoost = energyPlayer.fastAttackBoost;
            fastAttackFlat = energyPlayer.fastAttackFlat;

            damage = (int)((float)damage * fastAttackBoost);
            damage += fastAttackFlat;

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
        }


    }

    public class DruidProjectile : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.IronBroadsword}";

        public int energyGain;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (energyGain > 0)
            {
                Player player = Main.player[Projectile.owner];
                var energyPlayer = player.GetModPlayer<EnergyPlayer>();

                energyPlayer.EnergyCurrent += energyGain;
            }
        }
    }

}
