using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class DashDistanceBoost : Pickup
    {
        public DashDistanceBoost()
        {
            Level = 1;
            OutName = "Dash Distance Boost";
        }

        public override void OnPickup()
        {
            Globals.player.DashDistance += 0.2f;
        }
    }
}
