using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Arrow : Projectile
    {
        public Arrow(Weapon Weapon, float Rotation, int Damage)
        {
            projectileType = ProjectileType.Arrow;

            Shooter = Weapon.player;
            this.Rotation = Rotation;

            Scale = 1.5f;

            Centre = Weapon.player.Centre + Weapon.DirectionToMouse * 40f;

            Speed = 13f;

            ShouldCollideWithPlayer = false;

            this.Damage = Damage;

            PiercingCount = 2;
        }
    }
}
