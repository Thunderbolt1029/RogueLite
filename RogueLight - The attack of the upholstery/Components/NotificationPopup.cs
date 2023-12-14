using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLight___The_attack_of_the_upholstery
{
    class NotificationPopup : Component
    {
        Font TextFont;
        string Text;
        Vector2 StartPosition;
        Vector2 Position => StartPosition - Vector2.UnitY * FadeAmount * TextFont.SpriteFont.MeasureString(Text) * 0.5f;
        Rectangle BackgroundRectangle => new Rectangle((Position - TextFont.SpriteFont.MeasureString(Text) / 2 - Pad.ToVector2()).ToPoint(), (TextFont.SpriteFont.MeasureString(Text) + Pad.ToVector2() * 2).ToPoint());
        float HoldTime, StartFadeTime, FadeTime;
        Point Pad;
        float FadeAmount => FadeTime / StartFadeTime;

        public bool ToRemove { get; private set; } = false;

        public NotificationPopup(Font TextFont, string Text, Vector2 Position, float HoldTime, float FadeTime, Point Pad)
        {
            this.TextFont = TextFont;
            this.Text = Text;
            StartPosition = Position;
            this.HoldTime = HoldTime;
            StartFadeTime = this.FadeTime = FadeTime;
            this.Pad = Pad;
        }

        public override void Update(GameTime gameTime)
        {
            if (HoldTime > 0)
                HoldTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (FadeTime > 0)
            {
                FadeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
            else
                ToRemove = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Globals.ColorTexture, BackgroundRectangle, new Color(Color.Black, FadeAmount));
            spriteBatch.DrawString(TextFont.SpriteFont, Text, Position, new Color(Color.White, FadeAmount), 0f, TextFont.SpriteFont.MeasureString(Text) / 2, 1f, SpriteEffects.None, 1f);
        }
    }
}
