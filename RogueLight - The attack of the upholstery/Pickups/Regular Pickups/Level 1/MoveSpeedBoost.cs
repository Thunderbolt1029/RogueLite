using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class MoveSpeedBoost : Pickup
    {
        public MoveSpeedBoost()
        {
            Level = 1;
            OutName = "Move Speed Boost";
        }

        public override void OnPickup()
        {
            Globals.player.MoveSpeed += 0.2f;
        }
    }
}
