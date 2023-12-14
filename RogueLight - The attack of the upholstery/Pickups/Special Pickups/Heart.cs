using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Heart : Pickup
    {
        public Heart()
        {
            OutName = "Heart";

            Scale = 0.8f;

            Centre = Globals.GameWindowRectangle.Center.ToVector2();

            Special = true;
        }

        public Heart(Vector2 Centre)
        {
            OutName = "Heart";

            Scale = 0.8f;

            this.Centre = Centre;

            Special = true;
        }

        public override void OnPickup()
        {
            if (Globals.player.HeartsRemaining < Globals.player.TotalHearts)
                Globals.player.HeartsRemaining++;
        }
    }
}
