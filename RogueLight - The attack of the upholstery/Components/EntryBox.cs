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
    class EntryBox : Component
    {
        Keys[] CurrentKeys, PreviousKeys;
        GameTime gameTime;

        bool TextBoxSelected = false;

        public string Text = "";
        Font TextFont;
        Rectangle BoundingBox;
        string EmptyBoxText = "";

        Color backgroundColor;
        Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                backgroundColor = value;
                BackgroundColorTexture = new Texture2D(Globals.ColorTexture.GraphicsDevice, 1, 1);
                BackgroundColorTexture.SetData(new[] { backgroundColor });
            }
        }
        Texture2D BackgroundColorTexture;

        #region Constructors

        public EntryBox(Rectangle BoundingBox, Font TextFont)
        {
            this.BoundingBox = BoundingBox;
            this.TextFont = TextFont;
        }

        public EntryBox(Rectangle BoundingBox, Font TextFont, Color BackgroundColor)
        {
            this.BoundingBox = BoundingBox;
            this.TextFont = TextFont;
            this.BackgroundColor = BackgroundColor;
        }

        public EntryBox(Rectangle BoundingBox, Font TextFont, string EmptyBoxText)
        {
            this.BoundingBox = BoundingBox;
            this.TextFont = TextFont;
            this.EmptyBoxText = EmptyBoxText;
        }

        public EntryBox(Rectangle BoundingBox, Font TextFont, string EmptyBoxText, Color BackgroundColor)
        {
            this.BoundingBox = BoundingBox;
            this.TextFont = TextFont;
            this.EmptyBoxText = EmptyBoxText;
            this.BackgroundColor = BackgroundColor;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            if (Globals.CurrentMouseState.LeftButton == ButtonState.Pressed && Globals.PreviousMouseState.LeftButton == ButtonState.Released)
                if (BoundingBox.Contains(Globals.CurrentMouseState.Position))
                    TextBoxSelected = true;
                else
                    TextBoxSelected = false;

            if (TextBoxSelected)
            {
                PreviousKeys = CurrentKeys;
                CurrentKeys = Globals.CurrentKeyboardState.GetPressedKeys();
                Keys[] PressedKeys = PreviousKeys != null ? PressedKeys = CurrentKeys.Except(PreviousKeys).ToArray() : CurrentKeys;

                if (PressedKeys.Length == 0) return;
                else if (PressedKeys.Contains(Keys.Back) && Text.Length != 0)
                    Text = Text.Substring(0, Text.Length - 1);
                else
                {
                    bool shift = !Globals.CurrentKeyboardState.CapsLock && (Globals.CurrentKeyboardState.IsKeyDown(Keys.LeftShift) || Globals.CurrentKeyboardState.IsKeyDown(Keys.RightShift)) || Globals.CurrentKeyboardState.CapsLock && !(Globals.CurrentKeyboardState.IsKeyDown(Keys.LeftShift) || Globals.CurrentKeyboardState.IsKeyDown(Keys.RightShift));
                    char[] chars = PressedKeys.Select(key => KeyToChar(key, shift)).Where(x => x != null).Select(x => (char)x).ToArray();

                    foreach (char character in chars)
                        if (TextFont.SpriteFont.Characters.Contains(character) && TextFont.SpriteFont.MeasureString(Text + character + "|").X < BoundingBox.Width - TextFont.Padding.X * 2 && (character != '\n' || character == '\n' && TextFont.SpriteFont.MeasureString(Text + character + "|").Y < BoundingBox.Height - TextFont.Padding.Y * 2))
                        {
                            Text += character;
                            break;
                        }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (BackgroundColorTexture != null)
                spriteBatch.Draw(BackgroundColorTexture, BoundingBox, Color.White);
            spriteBatch.DrawString(TextFont.SpriteFont, ((Text.Length > 0) ? Text : "") + (TextBoxSelected && (int)Math.Floor(gameTime.TotalGameTime.TotalSeconds*1.3) % 2 == 0 ? "|" : ""), TextFont.PositionOffset((Text.Length > 0) ? Text :  "|", BoundingBox), TextFont.Color);
            if (Text.Length == 0 && !TextBoxSelected)
                spriteBatch.DrawString(TextFont.SpriteFont, EmptyBoxText, TextFont.PositionOffset(EmptyBoxText, BoundingBox), new Color((int)(TextFont.Color.R * 0.7), (int)(TextFont.Color.G * 0.7), (int)(TextFont.Color.B * 0.7)));
        }

        char? KeyToChar(Keys key, bool shift)
        {
            switch (key)
            {
                // Alphabet keys
                case Keys.A: if (shift) { return 'A'; } else { return 'a'; }
                case Keys.B: if (shift) { return 'B'; } else { return 'b'; }
                case Keys.C: if (shift) { return 'C'; } else { return 'c'; }
                case Keys.D: if (shift) { return 'D'; } else { return 'd'; }
                case Keys.E: if (shift) { return 'E'; } else { return 'e'; }
                case Keys.F: if (shift) { return 'F'; } else { return 'f'; }
                case Keys.G: if (shift) { return 'G'; } else { return 'g'; }
                case Keys.H: if (shift) { return 'H'; } else { return 'h'; }
                case Keys.I: if (shift) { return 'I'; } else { return 'i'; }
                case Keys.J: if (shift) { return 'J'; } else { return 'j'; }
                case Keys.K: if (shift) { return 'K'; } else { return 'k'; }
                case Keys.L: if (shift) { return 'L'; } else { return 'l'; }
                case Keys.M: if (shift) { return 'M'; } else { return 'm'; }
                case Keys.N: if (shift) { return 'N'; } else { return 'n'; }
                case Keys.O: if (shift) { return 'O'; } else { return 'o'; }
                case Keys.P: if (shift) { return 'P'; } else { return 'p'; }
                case Keys.Q: if (shift) { return 'Q'; } else { return 'q'; }
                case Keys.R: if (shift) { return 'R'; } else { return 'r'; }
                case Keys.S: if (shift) { return 'S'; } else { return 's'; }
                case Keys.T: if (shift) { return 'T'; } else { return 't'; }
                case Keys.U: if (shift) { return 'U'; } else { return 'u'; }
                case Keys.V: if (shift) { return 'V'; } else { return 'v'; }
                case Keys.W: if (shift) { return 'W'; } else { return 'w'; }
                case Keys.X: if (shift) { return 'X'; } else { return 'x'; }
                case Keys.Y: if (shift) { return 'Y'; } else { return 'y'; }
                case Keys.Z: if (shift) { return 'Z'; } else { return 'z'; }

                // Number keys
                case Keys.D0: if (shift) { return ')'; } else { return '0'; }
                case Keys.D1: if (shift) { return '!'; } else { return '1'; }
                case Keys.D2: if (shift) { return '"'; } else { return '2'; }
                case Keys.D3: if (shift) { return '£'; } else { return '3'; }
                case Keys.D4: if (shift) { return '$'; } else { return '4'; }
                case Keys.D5: if (shift) { return '%'; } else { return '5'; }
                case Keys.D6: if (shift) { return '^'; } else { return '6'; }
                case Keys.D7: if (shift) { return '&'; } else { return '7'; }
                case Keys.D8: if (shift) { return '*'; } else { return '8'; }
                case Keys.D9: if (shift) { return '('; } else { return '9'; }

                // Numpad keys
                case Keys.NumPad0: return '0';
                case Keys.NumPad1: return '1';
                case Keys.NumPad2: return '2';
                case Keys.NumPad3: return '3';
                case Keys.NumPad4: return '4';
                case Keys.NumPad5: return '5';
                case Keys.NumPad6: return '6';
                case Keys.NumPad7: return '7';
                case Keys.NumPad8: return '8';
                case Keys.NumPad9: return '9';

                // Symbol keys
                case Keys.OemTilde: if (shift) { return '@'; } else { return '`'; }
                case Keys.OemSemicolon: if (shift) { return ':'; } else { return ';'; }
                case Keys.OemQuotes: if (shift) { return '~'; } else { return '#'; }
                case Keys.OemQuestion: if (shift) { return '?'; } else { return '/'; }
                case Keys.OemPlus: if (shift) { return '+'; } else { return '='; }
                case Keys.OemPipe: if (shift) { return '|'; } else { return '\\'; }
                case Keys.OemPeriod: if (shift) { return '>'; } else { return '.'; }
                case Keys.OemOpenBrackets: if (shift) { return '{'; } else { return '['; }
                case Keys.OemCloseBrackets: if (shift) { return '}'; } else { return ']'; }
                case Keys.OemMinus: if (shift) { return '_'; } else { return '-'; }
                case Keys.OemComma: if (shift) { return '<'; } else { return ','; }

                case Keys.Space: return ' ';
            }
            return '\u25A1';
        }
    }
}
