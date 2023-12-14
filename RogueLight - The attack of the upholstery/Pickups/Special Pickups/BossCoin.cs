using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    class BossCoin : Pickup
    {
        public BossCoin()
        {
            OutName = "Boss Coin";

            Scale = 0.4f;

            Centre = Globals.GameWindowRectangle.Center.ToVector2();

            Special = true;
        }

        public BossCoin(Vector2 Centre)
        {
            OutName = "Boss Coin";

            Scale = 0.4f;

            this.Centre = Centre;

            Special = true;
        }

        public override void OnPickup()
        {
            Globals.NoBossCoinsCollected++;
            Globals.NoBossCoins++;
        }
    }
}
