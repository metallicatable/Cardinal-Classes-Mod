using fourClassesMod.Common.Classes.Barbarian;
using fourClassesMod.Common.Classes.Cultist;
using fourClassesMod.Common.Classes.Druid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace fourClassesMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class fourClassesMod : Mod
	{
        internal enum MessageType : byte
        {
            FaithEffect,
            RageEffect,
            EnergyEffect,
        }

        // Override this method to handle network packets sent for this mod.
        //TODO: Introduce OOP packets into tML, to avoid this god-class level hardcode.
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                // This message syncs ExampleStatIncreasePlayer.exampleLifeFruits and ExampleStatIncreasePlayer.exampleManaCrystals

                case MessageType.FaithEffect:
                    FaithPlayer.HandleFaithEffectMessage(reader, whoAmI);
                    break;
            }
            switch (msgType)
            {
                // This message syncs ExampleStatIncreasePlayer.exampleLifeFruits and ExampleStatIncreasePlayer.exampleManaCrystals

                case MessageType.RageEffect:
                    RagePlayer.HandleRageEffectMessage(reader, whoAmI);
                    break;
            }
            switch (msgType)
            {
                // This message syncs ExampleStatIncreasePlayer.exampleLifeFruits and ExampleStatIncreasePlayer.exampleManaCrystals

                case MessageType.EnergyEffect:
                    EnergyPlayer.HandleEnergyEffectMessage(reader, whoAmI);
                    break;
            }
        }
    }
}
