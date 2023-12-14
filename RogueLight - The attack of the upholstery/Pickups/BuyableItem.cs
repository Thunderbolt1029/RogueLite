using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class BuyableItem
    {
        // If this is changed then so will the ChangePriceStat function
        string[] ShopPrices = new string[]
        {
            "-1 Total Hearts",
            "-1 Hearts Remaining",
            "*0.8 Damage",
            "*1.2 Reload Time",
            "-0.1 Dodge Chance",
            "*0.8 Move Speed",
            "*0.8 Dash Distance"
        };

        public Pickup Item;
        public bool Bought;
        public int ShopID;
        bool Selected;
        public string BuyPrice;
        int BuyPriceIndex;

        Vector2 Centre => Globals.GameWindowRectangle.Center.ToVector2() + new Vector2((ShopID - 1) * 300, 0);
        Vector2 Position => Centre - new Vector2(64);

        public BuyableItem(int ShopID)
        {
            this.ShopID = ShopID;

            Item = Pickup.GetRandomPickup(null);
            Item.Scale = 1;
            Item.Position = Position;
            Bought = false;
            BuyPriceIndex = Globals.random.Next(ShopPrices.Length);
            BuyPrice = ShopPrices[BuyPriceIndex].Replace(" ", "");
        }

        public BuyableItem(Pickup Item, bool Bought, int ShopID, string BuyPrice)
        {
            this.Item = Item;
            this.Bought = Bought;
            this.ShopID = ShopID;
        }

        public void Update(GameTime gameTime)
        {
            if (!Bought)
            {
                if ((Globals.player.Centre - Centre).Length() <= 100 * Globals.WindowScale && !Globals.player.Selecting && !Selected)
                {
                    Selected = true;
                    Globals.player.Selecting = true;
                }
                else if (Selected)
                {
                    if ((Globals.player.Centre - Centre).Length() > 100 * Globals.WindowScale && Globals.player.Selecting)
                    {
                        Selected = false;
                        Globals.player.Selecting = false;
                    }

                    if (Globals.CurrentKeyboardState.IsKeyDown(Globals.Controls.Interact) && Globals.PreviousKeyboardState.IsKeyUp(Globals.Controls.Interact))
                    {
                        Selected = false;
                        Globals.player.Selecting = false;
                        Bought = true;
                        ChangePriceStat();
                        Globals.player.Pickups.Add(Item);
                        Globals.Components.Add(new NotificationPopup(Fonts.Debug, $"You picked up a {Item.OutName}", new Vector2(Globals.GameWindowRectangle.Center.X, Globals.GameWindowRectangle.Bottom - 100), 3, 0.7f, new Point(10)));
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Bought)
            {
                Item.Draw(spriteBatch);
                Font.DrawString(spriteBatch, Fonts.Debug, ShopPrices[BuyPriceIndex], Centre + new Vector2(0, 64 + Fonts.Debug.SpriteFont.MeasureString(ShopPrices[BuyPriceIndex]).Y * 2.5f), 2);
                if (Selected)
                {
                    Font.DrawString(spriteBatch, Fonts.Debug, Globals.Controls.Interact.ToString(), Centre, 3);
                }
            }
        }

        void ChangePriceStat()
        {
            switch (BuyPriceIndex)
            {
                case 0:
                    Globals.player.TotalHearts--;
                    break;
                case 1:
                    Globals.player.HeartsRemaining--;
                    break;
                case 2:
                    Globals.player.Damage *= 0.8f;
                    break;
                case 3:
                    Globals.player.ReloadTimeScalar *= 1.2f;
                    break;
                case 4:
                    Globals.player.DodgeChance = Math.Max(0, Globals.player.DodgeChance - 0.1f);
                    break;
                case 5:
                    Globals.player.MoveSpeed *= 0.8f;
                    break;
                case 6:
                    Globals.player.DashDistance *= 0.8f;
                    break;
            }
        }
    }
}
