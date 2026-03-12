using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Common.Classes.Druid
{
    public class EnergyPlayer : ModPlayer
    {
        
        public int EnergyCurrent; // Current value of our Cultist resource
        public const int DefaultEnergyMax = 3000; // Default maximum value of energy resource
        public int EnergyMax; // Buffer variable that is used to reset maximum resource to default value in ResetDefaults().
        public int EnergyMax2; // Maximum amount of our Cultist resource. We will change that variable to increase maximum amount of our resource
        public bool EnergyMagnet = false;
        public static readonly int EnergyMagnetGrabRange = 300;
        public static readonly Color HealEnergyColor = new(255, 215, 0); // The color to use with CombatText when replenishing EnergyCurrent
        public bool canRegenEnergy = true;

        public float energyAttackBoost = 1f;
        public int energyAttackFlat = 0;
        public float fastAttackBoost = 1f;
        public int fastAttackFlat = 0;

        // In order to make the Cultist Resource Cultist straightforward, several things have been left out that would be needed for a fully functional resource similar to mana and health. 
        // Here are additional things you might need to implement if you intend to make a custom resource:
        // - Multiplayer Syncing: The current Cultist doesn't require MP code, but pretty much any additional functionality will require this. ModPlayer.SendClientChanges and CopyClientState will be necessary, as well as SyncPlayer if you allow the user to increase EnergyMax.
        // - Save/Load permanent changes to max resource: You'll need to implement Save/Load to remember increases to your EnergyMax cap.

        public override void Initialize()
        {
            EnergyMax = DefaultEnergyMax;
        }

        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        // We need this to ensure that regeneration rate and maximum amount are reset to default values after increasing when conditions are no longer satisfied (e.g. we unequip an accessory that increases our resource)
        private void ResetVariables()
        {
            EnergyMax2 = EnergyMax;
            EnergyMagnet = false;
        }

        public override void PostUpdateMiscEffects()
        {
            UpdateResource();
        }

        public override void PostUpdate()
        {
            CapResourceGodMode();
        }

        // Lets do all our logic for the custom resource here, such as limiting it, increasing it and so on.
        private void UpdateResource()
        {
            
        }

        private void CapResourceGodMode()
        {
            if (Main.myPlayer == Player.whoAmI && Player.creativeGodMode)
            {
                EnergyCurrent = EnergyMax2;
            }
        }

        // HealEnergy will increase the actual Energy stat, then HealEnergyEffect spawns a CombatText visual and SendEnergyEffectMessage/HandleEnergyEffectMessage handle syncing that visual to other players.
        public void HealEnergy(int healAmount)
        {
            EnergyCurrent = Math.Clamp(EnergyCurrent + healAmount, 0, EnergyMax2);
            if (Main.myPlayer == Player.whoAmI)
            {
                HealEnergyEffect(healAmount);
            }
        }


        // Responsible for spawning and syncing just the CombatText
        public void HealEnergyEffect(int healAmount)
        {
            CombatText.NewText(Player.getRect(), HealEnergyColor, healAmount);
            if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI == Main.myPlayer)
            {
                SendEnergyEffectMessage(Player.whoAmI, healAmount);
            }
        }

        // These methods handle syncing the CombatText that indicates that the player has healed some amount of Energy
        public static void HandleEnergyEffectMessage(BinaryReader reader, int whoAmI)
        {
            int player = reader.ReadByte();
            if (Main.netMode == NetmodeID.Server)
            {
                player = whoAmI;
            }

            int healAmount = reader.ReadInt32();
            if (player != Main.myPlayer)
            {
                Main.player[player].GetModPlayer<EnergyPlayer>().HealEnergyEffect(healAmount);
            }

            if (Main.netMode == NetmodeID.Server)
            {
                // If the server receives this message, it sends it to all other clients to sync the effects.
                SendEnergyEffectMessage(player, healAmount);
            }
        }

        public static void SendEnergyEffectMessage(int whoAmI, int healAmount)
        {
            // This code is called by both the initial message from a client running healing code and the server relaying the changes to other clients.
            ModPacket packet = ModContent.GetInstance<fourClassesMod>().GetPacket();
            packet.Write((byte)fourClassesMod.MessageType.EnergyEffect);
            packet.Write((byte)whoAmI);
            packet.Write(healAmount);
            packet.Send(ignoreClient: whoAmI);
        }
    }
}