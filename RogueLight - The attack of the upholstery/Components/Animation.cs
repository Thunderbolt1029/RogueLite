using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Animation : Component
    {
        Texture2D Texture;
        int Frames, CurrentFrame;
        float elapsed = 0, delay = 200f;
        bool loop = true;
        Vector2 Position;
        float Scale = 1f;

        public bool ToRemove { get; private set; } = false;

        #region Constructors

        public Animation(Texture2D Texture, int Frames, float MilliSecondsPerFrame, Vector2 Position)
        {
            this.Texture = Texture;
            this.Frames = Frames;
            this.Position = Position;
            delay = MilliSecondsPerFrame;
        }

        public Animation(Texture2D Texture, int Frames, float MilliSecondsPerFrame, Vector2 Position, bool loop)
        {
            this.Texture = Texture;
            this.Frames = Frames;
            this.Position = Position;
            delay = MilliSecondsPerFrame;
            this.loop = loop;
        }

        public Animation(Texture2D Texture, float Scale, int Frames, float MilliSecondsPerFrame, Vector2 Position)
        {
            this.Texture = Texture;
            this.Frames = Frames;
            this.Position = Position;
            this.Scale = Scale;
            delay = MilliSecondsPerFrame;
        }

        public Animation(Texture2D Texture, float Scale, int Frames, float MilliSecondsPerFrame, Vector2 Position, bool loop)
        {
            this.Texture = Texture;
            this.Frames = Frames;
            this.Position = Position;
            delay = MilliSecondsPerFrame;
            this.loop = loop;
            this.Scale = Scale;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay)
            {
                elapsed = 0;
                CurrentFrame++;

                if (CurrentFrame > Frames)
                    if (loop)
                        CurrentFrame = 0;
                    else
                        ToRemove = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2((int)Position.X - Texture.Width * Scale / Frames / 2, (int)Position.Y - Texture.Height * Scale / 2), new Rectangle(Texture.Width * CurrentFrame / Frames, 0, Texture.Width / Frames, Texture.Height), Color.White, 0, new Vector2(), Scale, SpriteEffects.None, 0f);
        }
    }
}
