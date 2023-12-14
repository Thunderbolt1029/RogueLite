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
    abstract class Projectile : Entity
    {
        public static Dictionary<ProjectileType, Texture2D> ProjectileTextures = new Dictionary<ProjectileType, Texture2D>();
        public override Texture2D ActiveTexture => ProjectileTextures[projectileType];

        public override Rectangle HitBox
        {
            get
            {
                Vector2[] Corners = new Vector2[4];

                Corners[0] = -ActiveTexture.Bounds.Size.ToVector2() / 2;
                Corners[2] = ActiveTexture.Bounds.Size.ToVector2() / 2;

                Corners[1] = new Vector2(Corners[0].X, Corners[2].Y);
                Corners[3] = new Vector2(Corners[2].X, Corners[0].Y);


                Matrix FullTransformation =  Matrix.CreateScale(Scale) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(Position, 0));

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

        public ProjectileType projectileType;

        public Entity Shooter;

        public int Damage;
        public int PiercingCount = 1;
        internal Projectile thisPreviousFrame;

        public override void Update(GameTime gameTime)
        {
            if (PiercingCount <= 0) isRemoved = true;

            Position += AngleToVector(Rotation) * LinearVelocity;
            if (ShouldCollideWithPlayer)
                if (!Globals.player.Invunerable && Intersects(Globals.player.HitBox) && !thisPreviousFrame.Intersects(Globals.player.HitBox))
                    PiercingCount--;
            
            thisPreviousFrame = this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ActiveTexture, Centre, null, Color.White, Rotation, ActiveTexture.Bounds.Center.ToVector2(), Scale, SpriteEffects.None, 0);
        }
    }
}
