using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class EnergyBall : Projectile
    {
        public override Rectangle HitBox
        {
            get
            {
                Vector2[] Corners = new Vector2[4];

                Corners[0] = new Vector2(-25);
                Corners[2] = new Vector2(24, 25);

                Corners[1] = new Vector2(Corners[0].X, Corners[2].Y);
                Corners[3] = new Vector2(Corners[2].X, Corners[0].Y);


                Matrix FullTransformation = Matrix.CreateScale(Scale) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(Position, 0));

                Vector2[] TransformedCorners = new Vector2[4];

                for (int i = 0; i < 4; i++)
                {
                    TransformedCorners[i] = Vector2.Transform(Corners[i], FullTransformation);
                }

                Rectangle hitBox = Rectangle.Empty;

                hitBox.X = (int)TransformedCorners.Min(x => x.X);
                hitBox.Y = (int)TransformedCorners.Min(x => x.Y);

                hitBox.Width = (int)TransformedCorners.Max(x => x.X) - hitBox.X;
                hitBox.Height = (int)TransformedCorners.Max(x => x.Y) - hitBox.Y;

                return hitBox;
            }
        }

        public EnergyBall(Weapon Weapon, int Damage)
        {
            projectileType = ProjectileType.EnergyBall;

            Shooter = Weapon.player;
            Rotation = (Shooter as Player).LookingRotation;

            Scale = 1f;

            Centre = Weapon.player.Centre;

            Speed = 3f;

            ShouldCollideWithPlayer = false;

            this.Damage = Damage;

            PiercingCount = 3;
        }
    }
}
