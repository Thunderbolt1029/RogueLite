using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Bullet : Projectile
    {
        public Bullet(Entity Shooter)
        {
            projectileType = ProjectileType.Bullet;

            this.Shooter = Shooter;
            Rotation = Shooter.Rotation;

            Scale = 1.4f;
            Centre = Shooter.Centre;

            Speed = 7f;

            ShouldCollideWithPlayer = true;
        }

        public Bullet(Entity Shooter, float Rotation)
        {
            projectileType = ProjectileType.Bullet;

            this.Shooter = Shooter;
            this.Rotation = Rotation;

            Scale = 1.4f;
            Centre = Shooter.Centre;

            Speed = 7f;

            ShouldCollideWithPlayer = true;
        }

        public Bullet(Entity Shooter, float Rotation, int Damage)
        {
            projectileType = ProjectileType.Bullet;

            this.Shooter = Shooter;
            this.Rotation = Rotation;

            Scale = 1.6f;
            Centre = Shooter.Centre;

            this.Damage = Damage;

            Speed = 20f;

            ShouldCollideWithPlayer = false;
        }
    }
}
