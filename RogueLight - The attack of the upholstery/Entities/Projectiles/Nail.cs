using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Nail : Projectile
    {
        Vector2 Target;
        float TotalDistance;
        float Smoothing;
        public bool Blue;

        public override float LinearVelocity
        {
            get
            {
                float eToMinusX = (float)Math.Exp(-Smoothing * (((Target - Centre).Length() / TotalDistance) - 0.5));
                return (eToMinusX * 4 * Speed) / ((1 + eToMinusX) * (1 + eToMinusX));
            }
        }

        public Nail(Entity Shooter, float StartRotation, Vector2 Start, Vector2 Target, bool Blue = false)
        {
            projectileType = ProjectileType.Nail;

            this.Shooter = Shooter;

            Scale = 0.7f;
            Rotation = StartRotation;

            Centre = Start;
            this.Target = Target;
            TotalDistance = (Target - Start).Length();

            Speed = 40f;
            Smoothing = 250f / (float)Math.Sqrt(TotalDistance);

            ShouldCollideWithPlayer = true;

            this.Blue = Blue;
        }

        public override void Update(GameTime gameTime)
        {
            if ((Blue && Globals.player.Moving || !Blue && !Globals.player.Dashing) && Intersects(Globals.player.HitBox) || (Target - Centre).Length() < 10)
                isRemoved = true;
            
            Rotation += Math.Abs(VectorToAngle(Target - Centre) - Rotation) > MathHelper.Pi ? (VectorToAngle(Target - Centre) - Rotation) > 0 ? (VectorToAngle(Target - Centre) - Rotation - MathHelper.TwoPi) / 10 : (VectorToAngle(Target - Centre) - Rotation + MathHelper.TwoPi) / 10 : (VectorToAngle(Target - Centre) - Rotation) / 10;

            Position += Vector2.Normalize(Target - Centre) * LinearVelocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ProjectileTextures[projectileType], Centre, null, Blue ? new Color(0, 0, 200) : Color.Black, Rotation, ActiveTexture.Bounds.Center.ToVector2(), Scale, SpriteEffects.None, 0);
        }
    }
}
