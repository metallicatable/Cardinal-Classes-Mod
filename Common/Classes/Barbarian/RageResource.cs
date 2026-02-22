using fourClassesMod.Common.Classes.Cultist;
using fourClassesMod.Content.Buffs;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static System.Net.Mime.MediaTypeNames;

namespace fourClassesMod.Common.Classes.Barbarian
{
    public class RagePlayer : ModPlayer
    {
        // Here we create a custom resource, similar to mana or health.
        // Creating some variables to define the current value of our Cultist resource as well as the current maximum value. We also include a temporary max value, as well as some variables to handle the natural regeneration of this resource.
        public int RageCurrent; // Current value of our Cultist resource
        public const int DefaultRageMax = 100; // Default maximum value of Cultist resource
        public int RageMax; // Buffer variable that is used to reset maximum resource to default value in ResetDefaults().
        public int RageMax2; // Maximum amount of our Cultist resource. We will change that variable to increase maximum amount of our resource
        public float RageRegenRate; // By changing that variable we can increase/decrease regeneration rate of our resource
        internal int RageRegenTimer = 0; // A variable that is required for our timer
        public bool RageMagnet = false;
        public static readonly int RageMagnetGrabRange = 300;
        public static readonly Color HealRageColor = new(255, 215, 0); // The color to use with CombatText when replenishing RageCurrent
        int rageGainOnHit = 5;

        // In order to make the Cultist Resource Cultist straightforward, several things have been left out that would be needed for a fully functional resource similar to mana and health. 
        // Here are additional things you might need to implement if you intend to make a custom resource:
        // - Multiplayer Syncing: The current Cultist doesn't require MP code, but pretty much any additional functionality will require this. ModPlayer.SendClientChanges and CopyClientState will be necessary, as well as SyncPlayer if you allow the user to increase RageMax.
        // - Save/Load permanent changes to max resource: You'll need to implement Save/Load to remember increases to your RageMax cap.

        public override void Initialize()
        {
            RageMax = DefaultRageMax;
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
            RageRegenRate = 1f;
            RageMax2 = RageMax;
            RageMagnet = false;
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
           if (RageCurrent >= RageMax2)
           {
                RageCurrent = 0;
                Player.AddBuff(ModContent.BuffType<rageBuff>(), 300);
           }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.CountsAsACritter && Main.LocalPlayer.HeldItem.DamageType != ModContent.GetInstance<CultistDamageClass>() & !Player.HasBuff(ModContent.BuffType<rageBuff>()))
            {
                RageCurrent += rageGainOnHit;
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            int damage = hurtInfo.Damage;
            if (Main.LocalPlayer.HeldItem.DamageType != ModContent.GetInstance<CultistDamageClass>() & !Player.HasBuff(ModContent.BuffType<rageBuff>()))
            {
                MathHelper.Clamp(damage, 0, 40);
                
                RageCurrent += 10 + damage;
            }
        }



        private void CapResourceGodMode()
        {
            if (Main.myPlayer == Player.whoAmI && Player.creativeGodMode)
            {
                RageCurrent = RageMax2;
            }
        }

        // HealRage will increase the actual Rage stat, then HealRageEffect spawns a CombatText visual and SendRageEffectMessage/HandleRageEffectMessage handle syncing that visual to other players.
        public void HealRage(int healAmount)
        {
            RageCurrent = Math.Clamp(RageCurrent + healAmount, 0, RageMax2);
            if (Main.myPlayer == Player.whoAmI)
            {
                HealRageEffect(healAmount);
            }
        }

        // Responsible for spawning and syncing just the CombatText
        public void HealRageEffect(int healAmount)
        {
            CombatText.NewText(Player.getRect(), HealRageColor, healAmount);
            if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI == Main.myPlayer)
            {
                SendRageEffectMessage(Player.whoAmI, healAmount);
            }
        }

        // These methods handle syncing the CombatText that indicates that the player has healed some amount of Rage
        public static void HandleRageEffectMessage(BinaryReader reader, int whoAmI)
        {
            int player = reader.ReadByte();
            if (Main.netMode == NetmodeID.Server)
            {
                player = whoAmI;
            }

            int healAmount = reader.ReadInt32();
            if (player != Main.myPlayer)
            {
                Main.player[player].GetModPlayer<RagePlayer>().HealRageEffect(healAmount);
            }

            if (Main.netMode == NetmodeID.Server)
            {
                // If the server receives this message, it sends it to all other clients to sync the effects.
                SendRageEffectMessage(player, healAmount);
            }
        }

        public static void SendRageEffectMessage(int whoAmI, int healAmount)
        {
            // This code is called by both the initial message from a client running healing code and the server relaying the changes to other clients.
            ModPacket packet = ModContent.GetInstance<fourClassesMod>().GetPacket();
            packet.Write((byte)fourClassesMod.MessageType.RageEffect);
            packet.Write((byte)whoAmI);
            packet.Write(healAmount);
            packet.Send(ignoreClient: whoAmI);
        }
    }
}