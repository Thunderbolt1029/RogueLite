using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class HealthContainer : Pickup
    {
        public HealthContainer()
        {
            Level = 2;
            OutName = "Health Container";
        }

        public override void OnPickup()
        {
            Globals.player.TotalHearts++;
            Globals.player.HeartsRemaining++;
        }
    }
}
