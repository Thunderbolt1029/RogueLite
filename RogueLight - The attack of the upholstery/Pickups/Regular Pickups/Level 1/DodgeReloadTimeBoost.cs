using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class DodgeReloadTimeBoost : Pickup
    {
        public DodgeReloadTimeBoost()
        {
            Level = 1;
            OutName = "Dodge Reload Time Boost";
        }

        public override void OnPickup()
        {
            Globals.player.DodgeReloadTime *= 0.8f;
        }
    }
}
