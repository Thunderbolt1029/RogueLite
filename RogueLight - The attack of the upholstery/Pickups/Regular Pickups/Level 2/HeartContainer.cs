using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class HeartContainer : Pickup
    {
        public HeartContainer()
        {
            Level = 2;
            OutName = "Heart Container";
        }

        public override void OnPickup()
        {
            Globals.player.TotalHearts++;
        }
    }
}
