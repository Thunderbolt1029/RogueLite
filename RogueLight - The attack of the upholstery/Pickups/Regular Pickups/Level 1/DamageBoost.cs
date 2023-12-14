using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class DamageBoost : Pickup
    {
        public DamageBoost()
        {
            Level = 1;
            OutName = "Damage Boost";
        }

        public override void OnPickup()
        {
            Globals.player.Damage += 0.2f;
        }
    }
}
