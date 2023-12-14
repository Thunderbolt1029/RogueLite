using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class LifeSteal : Pickup
    {
        public LifeSteal()
        {
            Level = 3;
            OutName = "Life Steal";
        }

        public override void OnKill()
        {
            if (Globals.player.HeartsRemaining < Globals.player.TotalHearts && Globals.random.NextDouble() < 0.05f)
                Globals.player.HeartsRemaining++;
        }
    }
}
