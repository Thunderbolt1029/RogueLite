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
    class Chest : Entity
    {
        Pickup ChestContents;
        bool Opened = false;
        bool Selected;

        public override Texture2D ActiveTexture
        {
            get
            {
                if (Opened)
                    return Textures[(int)SpriteType.Chest.Opened];
                else
                    return Textures[(int)SpriteType.Chest.Closed];
            }
        }

        public Chest(List<Texture2D> Textures, Vector2 Centre)
        {
            this.Textures = Textures;
            this.Rotation = 0f;
            this.Scale = 0.5f;
            this.Centre = Centre;

            Opened = true;
        }
        public Chest(List<Texture2D> Textures, Vector2 Centre, Pickup ChestContents)
        {
            this.Textures = Textures;
            this.Rotation = 0f;
            this.Scale = 0.5f;
            this.Centre = Centre;

            this.ChestContents = ChestContents;
            Opened = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Opened && (Globals.player.Centre - Centre).Length() <= 100 * Globals.WindowScale && !Globals.player.Selecting)
            {
                Selected = true;
                Globals.player.Selecting = true;
            }

            if (!Opened && (Globals.player.Centre - Centre).Length() > 100 * Globals.WindowScale && Globals.player.Selecting && Selected == true)
            {
                Selected = false;
                Globals.player.Selecting = false;
            }

            if (!Opened && Globals.CurrentKeyboardState.IsKeyDown(Globals.Controls.Interact) && Globals.PreviousKeyboardState.IsKeyUp(Globals.Controls.Interact) && Selected)
            {
                Opened = true;
                Globals.player.Pickups.Add(ChestContents);
                Globals.Components.Add(new NotificationPopup(Fonts.Debug, $"You picked up a {ChestContents.OutName}", new Vector2(Globals.GameWindowRectangle.Center.X, Globals.GameWindowRectangle.Bottom - 100), 3, 0.7f, new Point(10)));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (!Opened && Selected)
                Font.DrawString(spriteBatch, Fonts.Debug, Globals.Controls.Interact.ToString(), Centre, 3);
        }
    }
}
