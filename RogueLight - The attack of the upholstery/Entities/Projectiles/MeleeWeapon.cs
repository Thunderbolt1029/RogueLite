using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLight___The_attack_of_the_upholstery
{
    class MeleeWeapon : Projectile
    {
        public override Texture2D ActiveTexture => Weapon.WeaponTextures[weapon.WeaponType];

        Vector2[] Corners
        { 
            get
            {
                Vector2[] Corners = new Vector2[4];

                Corners[0] = Vector2.Zero;
                Corners[2] = ActiveTexture.Bounds.Size.ToVector2() * Scale;

                Corners[1] = new Vector2(Corners[0].X, Corners[2].Y);
                Corners[3] = new Vector2(Corners[2].X, Corners[0].Y);

                Matrix FullTransformation = Matrix.CreateTranslation(new Vector3(-new Vector2(0, ActiveTexture.Height / 2) * Scale, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(Position, 0));

                List<Vector2> TransformedCorners = new List<Vector2>();

                for (int i = 0; i < 4; i++)
                {
                    TransformedCorners.Add(Vector2.Transform(Corners[i], FullTransformation));
                }

                // Add extra points along each side
                int AmountOfExtraPoints = 10;
                for (int Side = 0; Side < 4; Side++)
                    for (int i = 1; i <= AmountOfExtraPoints; i++)
                    {
                        Vector2 Left = TransformedCorners[Side], Right = TransformedCorners[(Side + 1) % 4];
                        Vector2 PointSpacing = (Right - Left) / (AmountOfExtraPoints + 1);
                        TransformedCorners.Add(Left + PointSpacing * i);
                    }

                return TransformedCorners.ToArray();
            }
        }
        public override bool Intersects(Entity entity) => Corners.Any(x => entity.HitBox.Contains(x));
        public override bool Intersects(Rectangle rectangle) => Corners.Any(x => rectangle.Contains(x));

        Weapon weapon;
        float AttackTime;

        public MeleeWeapon(Entity Shooter, Weapon weapon, int Damage, float AttackTime)
        {
            this.Shooter = Shooter;
            this.weapon = weapon;
            this.AttackTime = AttackTime;

            this.Damage = Damage;
            ShouldCollideWithPlayer = false;
        }

        public override void Update(GameTime gameTime)
        {
            AttackTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position = weapon.Position;
            Rotation = weapon.Rotation;
            Scale = weapon.Scale;

            // if (AttackTime <= 0) isRemoved = true;

            thisPreviousFrame = this;
        }

        public override void Draw(SpriteBatch spriteBatch) { }
    }
}
