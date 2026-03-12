using fourClassesMod.Common.Classes.Druid;
using fourClassesMod.Common.Classes.Heretic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Accessories.Druid 
{
    internal class DruidEmblem : ModItem 
    {

        public override string Texture => $"fourClassesMod/Sprites/Accessories/DruidEmblem"; 
        

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed; 
        }




        public override void UpdateAccessory(Player player, bool hideVisual) 
        {
            
            
            player.GetDamage<DruidDamageClass>() += 0.15f; 

           

        }


    }
}
