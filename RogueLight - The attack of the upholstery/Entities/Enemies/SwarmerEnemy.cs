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
    class SwarmerEnemy : Enemy
    {
        public override Texture2D ActiveTexture 
        {
            get 
            {
                return Textures[(int)(SpriteType.Looking)Math.Floor((MathHelper.WrapAngle(LookingRotation - 0.625f * MathHelper.Pi) / MathHelper.PiOver4) + 4)];
            }
            
        }

        public override float LinearVelocity
        {
            get
            {
                if (Intersects(Globals.player))
                    return Speed * Globals.WindowScale * 0.5f;
                else
                    return Speed * Globals.WindowScale;
            }
        }

        public SwarmerEnemy(List<Texture2D> Textures, float Scale, Vector2 Centre, float Speed)
        {
            this.Textures = Textures;
            this.Scale = Scale;

            this.Centre = Centre;

            this.Speed = Speed;

            ShouldCollideWithPlayer = true;

            Health = 15;
        }

        public override void Update(GameTime gameTime)
        {
            MovingDirection = DirectionToPlayer;
            if (DirectionToPlayer != Vector2.Zero) LookingRotation = VectorToAngle(DirectionToPlayer);

            base.Update(gameTime);
        }
    }
}
