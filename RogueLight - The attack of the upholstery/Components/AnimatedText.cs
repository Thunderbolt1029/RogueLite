using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class AnimatedText : Component
    {
        Font TextFont;
        string Text;
        Vector2 Position;
        float Rotation = 0f, Scale = 1f;
        float StartRotation = 0f, StartScale = 1f;

        public float RotationScaleOffset = 0f;
        
        public float RotationStrength { get; private set; } = 0f;
        public float ScaleStrength { get; private set; } = 0f;
        public float RotationPeriod { get; private set; } = 1f;
        public float ScalePeriod { get; private set; } = 1f;

        #region Constructors

        public AnimatedText(Font TextFont, string Text, Vector2 Position)
        {
            this.TextFont = TextFont;
            this.Text = Text;
            this.Position = Position;
        }

        public AnimatedText(Font TextFont, string Text, Vector2 Position, float Rotation)
        {
            this.TextFont = TextFont;
            this.Text = Text;
            this.Position = Position;
            StartRotation = Rotation;
            this.Rotation = Rotation;
        }

        public AnimatedText(Font TextFont, string Text, float Scale, Vector2 Position)
        {
            this.TextFont = TextFont;
            this.Text = Text;
            this.Position = Position;
            StartScale = Scale;
            this.Scale = Scale;
        }

        public AnimatedText(Font TextFont, string Text, float Scale, Vector2 Position, float Rotation)
        {
            this.TextFont = TextFont;
            this.Text = Text;
            this.Position = Position;
            StartRotation = Rotation;
            this.Rotation = Rotation;
            StartScale = Scale;
            this.Scale = Scale;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            if (RotationStrength != 0)
                Rotation = StartRotation + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * MathHelper.TwoPi / RotationPeriod + RotationScaleOffset) * RotationStrength;
            if (ScaleStrength != 0)
                Scale = StartScale + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * MathHelper.TwoPi / ScalePeriod + RotationScaleOffset) * ScaleStrength;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextFont.SpriteFont, Text, Position - TextFont.SpriteFont.MeasureString(Text) / 2, TextFont.Color, Rotation, TextFont.SpriteFont.MeasureString(Text) / 2, Scale, SpriteEffects.None, 0f);
        }

        public void SetRotationAmount(float Strength, float Period)
        {
            RotationStrength = Strength;
            RotationPeriod = Period;
        }
        public void SetScaleAmount(float Strength, float Period)
        {
            ScaleStrength = Strength;
            ScalePeriod = Period;
        }
    }
}
