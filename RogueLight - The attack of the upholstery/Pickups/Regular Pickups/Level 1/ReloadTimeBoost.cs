using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class ReloadTimeBoost : Pickup
    {
        public ReloadTimeBoost()
        {
            Level = 1;
            OutName = "Reload Time Boost";
        }

        public override void OnPickup()
        {
            Globals.player.ReloadTimeScalar *= 0.8f;
        }
    }
}
