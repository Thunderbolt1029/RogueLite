using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Portal : Entity
    {
        public static Texture2D Texture;
        public override Texture2D ActiveTexture => Texture;

        public static Action LoadNewLevel;

        int CurrentStage;

        bool Selected = false;

        public Portal(Vector2 Centre, int CurrentStage)
        {
            this.Centre = Centre;
            this.CurrentStage = CurrentStage;
        }

        public override void Update(GameTime gameTime)
        {
            if ((Globals.player.Centre - Centre).Length() <= 100 * Globals.WindowScale && !Globals.player.Selecting)
            {
                Selected = true;
                Globals.player.Selecting = true;
            }

            if ((Globals.player.Centre - Centre).Length() > 100 * Globals.WindowScale && Globals.player.Selecting && Selected == true)
            {
                Selected = false;
                Globals.player.Selecting = false;
            }

            if (Globals.CurrentKeyboardState.IsKeyDown(Globals.Controls.Interact) && Globals.PreviousKeyboardState.IsKeyUp(Globals.Controls.Interact) && Selected)
            {
                switch (CurrentStage)
                {
                    case Globals.TotalNoStages:
                        // Send player to win screen because final boss has been killed
                        Globals.GameWon = true;
                        Globals.FirstFrameWonDeath = true;
                        Globals.RunTime = DateTime.Now - Globals.RunStartTime;
                        break;

                    case 0:
                        // Send player into the game
                        Globals.RunNo++;
                        Globals.Stage++;
                        LoadNewLevel();
                        Globals.RunStartTime = DateTime.Now;
                        break;

                    default:
                        // Send player to next stage
                        Globals.Stage++;
                        LoadNewLevel();
                        break;
                }

                isRemoved = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Selected)
                Font.DrawString(spriteBatch, Fonts.Debug, Globals.Controls.Interact.ToString(), Centre, 3);
        }
    }
}
