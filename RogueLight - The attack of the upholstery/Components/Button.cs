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
    class Button : Component
    {
        #region Properties

        public Rectangle BoundingBox;
        Texture2D Texture;

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

        Font TextFont;
        string Text = "";
        Action OnClickFunction;
        bool Dim = false;

        #endregion

        #region Constructors

        public Button(Action OnClickFunction, Rectangle BoundingBox)
        {
            this.OnClickFunction = OnClickFunction;
            this.BoundingBox = BoundingBox;
        }

        public Button(Action OnClickFunction, Rectangle BoundingBox, Texture2D Texture)
        {
            this.OnClickFunction = OnClickFunction;
            this.BoundingBox = BoundingBox;
            this.Texture = Texture;
        }

        public Button(Action OnClickFunction, Rectangle BoundingBox, Color BackgroundColor)
        {
            this.OnClickFunction = OnClickFunction;
            this.BoundingBox = BoundingBox;
            this.BackgroundColor = BackgroundColor;
        }

        public Button(Action OnClickFunction, Rectangle BoundingBox, string Text, Font TextFont)
        {
            this.OnClickFunction = OnClickFunction;
            this.BoundingBox = BoundingBox;
            this.Text = Text;
            this.TextFont = TextFont;
        }

        public Button(Action OnClickFunction, Rectangle BoundingBox, Texture2D Texture, string Text, Font TextFont)
        {
            this.OnClickFunction = OnClickFunction;
            this.BoundingBox = BoundingBox;
            this.Texture = Texture;
            this.Text = Text;
            this.TextFont = TextFont;
        }

        public Button(Action OnClickFunction, Rectangle BoundingBox, Color BackgroundColor, string Text, Font TextFont)
        {
            this.OnClickFunction = OnClickFunction;
            this.BoundingBox = BoundingBox;
            this.BackgroundColor = BackgroundColor;
            this.Text = Text;
            this.TextFont = TextFont;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            if (Globals.CurrentMouseState.LeftButton == ButtonState.Pressed && BoundingBox.Contains(Globals.CurrentMouseState.Position))
                Dim = true;
            else
                Dim = false;
            if (Globals.CurrentMouseState.LeftButton == ButtonState.Released && Globals.PreviousMouseState.LeftButton == ButtonState.Pressed && BoundingBox.Contains(Globals.CurrentMouseState.Position))
                OnClickFunction.Invoke();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, BoundingBox, Color.White);
            if (BackgroundColorTexture != null)
                spriteBatch.Draw(BackgroundColorTexture, BoundingBox, Color.White);
            if (Dim)
                spriteBatch.Draw(Globals.ColorTexture, BoundingBox, new Color(0, 0, 0, 50));
            if (TextFont != null)
                spriteBatch.DrawString(TextFont.SpriteFont, Text, TextFont.PositionOffset(Text, BoundingBox), TextFont.Color);
        }
    }
}
