using fourClassesMod.Common.Classes.Heretic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace fourClassesMod.Content.Items.Weapons.Heretic
{
    public class WoodenDagger : ModItem
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/WoodenDagger";
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.rare = ItemRarityID.White;
            Item.knockBack = 1f;
            Item.height = 42;
            Item.width = 42;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<WoodenDaggerProjectile>();
        }
    }

    public class WoodenDaggerProjectile : ModProjectile
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/WoodenDagger";
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // make the projectile face the direction it's moving

            Projectile.velocity.Y += 0.2f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }
    }
    public class TinDagger : ModItem
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/TinDagger";
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.rare = ItemRarityID.White;
            Item.knockBack = 1f;
            Item.height = 42;
            Item.width = 42;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<TinDaggerProjectile>();
        }
    }

    public class TinDaggerProjectile : ModProjectile
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/TinDagger";
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // make the projectile face the direction it's moving

            Projectile.velocity.Y += 0.2f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }
    }

    public class CopperDagger : ModItem
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/CopperDagger";
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.rare = ItemRarityID.White;
            Item.knockBack = 1f;
            Item.height = 42;
            Item.width = 42;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<CopperDaggerProjectile>();
        }
    }

    public class CopperDaggerProjectile : ModProjectile
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/CopperDagger";
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // make the projectile face the direction it's moving

            Projectile.velocity.Y += 0.2f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }
    }

    public class SilverDagger : ModItem
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/SilverDagger";
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.rare = ItemRarityID.White;
            Item.knockBack = 1f;
            Item.height = 42;
            Item.width = 42;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<SilverDaggerProjectile>();
        }
    }

    public class SilverDaggerProjectile : ModProjectile
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/SilverDagger";

        public override void SetDefaults()
        {
            Projectile.penetrate = 2;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // make the projectile face the direction it's moving

            Projectile.velocity.Y += 0.1f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }


    }

    public class TungstenDagger : ModItem
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/TungstenDagger";
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.rare = ItemRarityID.White;
            Item.knockBack = 1f;
            Item.height = 42;
            Item.width = 42;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<TungstenDaggerProjectile>();
        }
    }

    public class TungstenDaggerProjectile : ModProjectile
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/TungstenDagger";
        public override void SetDefaults()
        {
            Projectile.penetrate = 2;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // make the projectile face the direction it's moving

            Projectile.velocity.Y += 0.1f;

            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }


    }

    public class GoldDagger : ModItem
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/GoldDagger";
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.rare = ItemRarityID.White;
            Item.knockBack = 1f;
            Item.height = 42;
            Item.width = 42;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<GoldDaggerProjectile>();
        }
    }

    public class GoldDaggerProjectile : ModProjectile
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/GoldDagger";
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // make the projectile face the direction it's moving
        }


    }

    public class PlatinumDagger : ModItem
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/PlatinumDagger";
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = ModContent.GetInstance<HereticDamageClass>();
            Item.rare = ItemRarityID.White;
            Item.knockBack = 1f;
            Item.height = 42;
            Item.width = 42;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<PlatinumDaggerProjectile>();
        }
    }

    public class PlatinumDaggerProjectile : ModProjectile
    {
        public override string Texture => $"fourClassesMod/Sprites/Weapons/PlatinumDagger";
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // make the projectile face the direction it's moving
        }


    }
}
