using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Common.Classes.Cultist
{
    public class FaithPlayer : ModPlayer
    {
        // Here we create a custom resource, similar to mana or health.
        // Creating some variables to define the current value of our Cultist resource as well as the current maximum value. We also include a temporary max value, as well as some variables to handle the natural regeneration of this resource.
        public int FaithCurrent; // Current value of our Cultist resource
        public const int DefaultFaithMax = 75; // Default maximum value of Cultist resource
        public int FaithMax; // Buffer variable that is used to reset maximum resource to default value in ResetDefaults().
        public int FaithMax2; // Maximum amount of our Cultist resource. We will change that variable to increase maximum amount of our resource
        public float FaithRegenRate; // By changing that variable we can increase/decrease regeneration rate of our resource
        internal int FaithRegenTimer = 0; // A variable that is required for our timer
        public bool FaithMagnet = false;
        public static readonly int FaithMagnetGrabRange = 300;
        public static readonly Color HealFaithColor = new(255, 215, 0); // The color to use with CombatText when replenishing FaithCurrent

        // In order to make the Cultist Resource Cultist straightforward, several things have been left out that would be needed for a fully functional resource similar to mana and health. 
        // Here are additional things you might need to implement if you intend to make a custom resource:
        // - Multiplayer Syncing: The current Cultist doesn't require MP code, but pretty much any additional functionality will require this. ModPlayer.SendClientChanges and CopyClientState will be necessary, as well as SyncPlayer if you allow the user to increase FaithMax.
        // - Save/Load permanent changes to max resource: You'll need to implement Save/Load to remember increases to your FaithMax cap.

        public override void Initialize()
        {
            FaithMax = DefaultFaithMax;
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
            FaithRegenRate = 1f;
            FaithMax2 = FaithMax;
            FaithMagnet = false;
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
            // For our resource lets make it regen slowly over time to keep it simple, let's use FaithRegenTimer to count up to whatever value we want, then increase currentResource.
            FaithRegenTimer++; // Increase it by 60 per second, or 1 per tick.

            // A simple timer that goes up to 1 second, increases the FaithCurrent by 1 and then resets back to 0.
            if (FaithRegenTimer > 6 / FaithRegenRate && Main.LocalPlayer.HeldItem.DamageType == ModContent.GetInstance<CultistDamageClass>())
            {
                FaithCurrent += 1;
                FaithRegenTimer = 0;
            }

            // Limit FaithCurrent from going over the limit imposed by FaithMax.
            FaithCurrent = Utils.Clamp(FaithCurrent, 0, FaithMax2);
        }

        private void CapResourceGodMode()
        {
            if (Main.myPlayer == Player.whoAmI && Player.creativeGodMode)
            {
                FaithCurrent = FaithMax2;
            }
        }

        // HealFaith will increase the actual Faith stat, then HealFaithEffect spawns a CombatText visual and SendFaithEffectMessage/HandleFaithEffectMessage handle syncing that visual to other players.
        public void HealFaith(int healAmount)
        {
            FaithCurrent = Math.Clamp(FaithCurrent + healAmount, 0, FaithMax2);
            if (Main.myPlayer == Player.whoAmI)
            {
                HealFaithEffect(healAmount);
            }
        }

        // Responsible for spawning and syncing just the CombatText
        public void HealFaithEffect(int healAmount)
        {
            CombatText.NewText(Player.getRect(), HealFaithColor, healAmount);
            if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI == Main.myPlayer)
            {
                SendFaithEffectMessage(Player.whoAmI, healAmount);
            }
        }

        // These methods handle syncing the CombatText that indicates that the player has healed some amount of Faith
        public static void HandleFaithEffectMessage(BinaryReader reader, int whoAmI)
        {
            int player = reader.ReadByte();
            if (Main.netMode == NetmodeID.Server)
            {
                player = whoAmI;
            }

            int healAmount = reader.ReadInt32();
            if (player != Main.myPlayer)
            {
                Main.player[player].GetModPlayer<FaithPlayer>().HealFaithEffect(healAmount);
            }

            if (Main.netMode == NetmodeID.Server)
            {
                // If the server receives this message, it sends it to all other clients to sync the effects.
                SendFaithEffectMessage(player, healAmount);
            }
        }

        public static void SendFaithEffectMessage(int whoAmI, int healAmount)
        {
            // This code is called by both the initial message from a client running healing code and the server relaying the changes to other clients.
            ModPacket packet = ModContent.GetInstance<fourClassesMod>().GetPacket();
            packet.Write((byte)fourClassesMod.MessageType.FaithEffect);
            packet.Write((byte)whoAmI);
            packet.Write(healAmount);
            packet.Send(ignoreClient: whoAmI);
        }
    }
}