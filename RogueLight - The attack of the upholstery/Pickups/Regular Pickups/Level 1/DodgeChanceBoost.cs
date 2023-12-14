using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class DodgeChanceBoost : Pickup
    {
        public DodgeChanceBoost()
        {
            Level = 1;
            OutName = "Dodge Chance Boost";
        }

        public override void OnPickup()
        {
            Globals.player.DodgeChance = 1 - (1 - Globals.player.DodgeChance) * 0.8f;
        }
    }
}
