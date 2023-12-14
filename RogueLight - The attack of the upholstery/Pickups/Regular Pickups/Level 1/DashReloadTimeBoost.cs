using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class DashReloadTimeBoost : Pickup
    {
        public DashReloadTimeBoost()
        {
            Level = 1;
            OutName = "Dash Reload Time Boost";
        }

        public override void OnPickup()
        {
            Globals.player.DashReloadTimeScalar *= 0.8f;
        }
    }
}
