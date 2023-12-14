using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLight___The_attack_of_the_upholstery
{
    // Will store the SpriteFont for every font needed in the game
    struct Fonts
    {
        public static Font Debug;
        public static Font Health;
        public static Font TitleButtons;
        public static Font Leaderboard;
        public static Font PreviousRun;

        public static Font IWasCrazyOnce;
        public static Font YoureCrazy;
    }

    class Font
    {
        public SpriteFont SpriteFont;
        public AlignmentType Alignment = AlignmentType.Centre;
        public Vector2 Padding = new Vector2(0);
        public Color Color;

        public Font(SpriteFont SpriteFont, Color Color)
        {
            this.SpriteFont = SpriteFont;
            this.Color = Color;
        }

        public Font(SpriteFont SpriteFont, Color Color, AlignmentType Alignment, Vector2 Padding)
        {
            this.SpriteFont = SpriteFont;
            this.Color = Color;
            this.Alignment = Alignment;
            this.Padding = Padding;
        }
        
        public Vector2 PositionOffset(string String, Rectangle TextBox)
        {
            Vector2 Pos = new Vector2();
            Vector2 StringSize = SpriteFont.MeasureString(String);

            switch ((int)Alignment / 3)
            {
                case 0:
                    Pos.Y = TextBox.Top + Padding.Y;
                    break;
                case 1:
                    Pos.Y = TextBox.Center.Y - StringSize.Y / 2;
                    break;
                case 2:
                    Pos.Y = TextBox.Bottom - StringSize.Y - Padding.Y;
                    break;
            }
            switch ((int)Alignment % 3)
            {
                case 0:
                    Pos.X = TextBox.Left + Padding.X;
                    break;
                case 1:
                    Pos.X = TextBox.Center.X - StringSize.X / 2;
                    break;
                case 2:
                    Pos.X = TextBox.Right - StringSize.X - Padding.X;
                    break;
            }

            return Pos;
        }

        public static void DrawString(SpriteBatch spriteBatch, Font font, string text, Vector2 centre, float Scale = 1)
        {
            spriteBatch.DrawString(font.SpriteFont, text, centre - font.SpriteFont.MeasureString(text) * Scale / 2, font.Color, 0, new Vector2(), Scale, SpriteEffects.None, 0f);
        }
    }

    public enum AlignmentType
    {
        TopLeft,     Top,     TopRight,
        Left,        Centre,  Right,
        BottomLeft,  Bottom,  BottomRight
    }
}
