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
    class RandomMoverShooterEnemy : Enemy
    {
        public override Texture2D ActiveTexture { get => Textures[(int)(SpriteType.Looking)Math.Floor((MathHelper.WrapAngle(LookingRotation - 0.625f * MathHelper.Pi) / MathHelper.PiOver4) + 4)]; }

        public float MovingDirectionAngle = 0f;
        public float WanderStrength;
        public (float Min, float Max) ReloadTimeBounds;
        public float ReloadingTime = 0f;

        public RandomMoverShooterEnemy(List<Texture2D> Textures, float Scale, Vector2 Centre, float Speed, float WanderStrength)
        {
            this.Textures = Textures;
            this.Scale = Scale;

            this.Centre = Centre;

            this.Speed = Speed;

            this.WanderStrength = MathHelper.Clamp(WanderStrength, 0, 1);

            ShouldCollideWithPlayer = false;

            ReloadTimeBounds = (1f, 3f);

            Health = 40;
        }

        public override void Update(GameTime gameTime)
        {
            LookingRotation = VectorToAngle(DirectionToPlayer);

            MovingDirectionAngle += MathHelper.WrapAngle(((float)Globals.random.NextDouble()-0.5f) * WanderStrength * MathHelper.TwoPi);
            MovingDirection = AngleToVector(MovingDirectionAngle);

            if (ReloadingTime > 0)
                ReloadingTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (ReloadingTime < 0)
                ReloadingTime = 0;

            if (ReloadingTime == 0)
                Shoot();

            base.Update(gameTime);
        }

        public void Shoot()
        {
            ReloadingTime = (float)Globals.random.NextDouble() * (ReloadTimeBounds.Max - ReloadTimeBounds.Min) + ReloadTimeBounds.Min;

            Globals.Entities.Add(new Bullet(this, LookingRotation));
        }
    }
}
