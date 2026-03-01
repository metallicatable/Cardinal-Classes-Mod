using fourClassesMod.Common.Classes.Barbarian;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Cultist
{
    internal class twoByFour : ModItem
    {

        public override string Texture => $"fourClassesMod/Sprites/Weapons/twoByFour";

        public override void SetDefaults()
        {

            Item.damage = 24;
            Item.knockBack = 4f;
            Item.useStyle = ItemUseStyleID.Swing; // Makes the player do the proper arm motion
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<BarbarianDamageClass>();
            Item.autoReuse = true;
            Item.rare = ItemRarityID.White;
        }    
    }
}
