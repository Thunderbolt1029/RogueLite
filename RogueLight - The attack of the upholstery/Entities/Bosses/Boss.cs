using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    abstract class Boss : Enemy
    {
        public override void Update(GameTime gameTime) 
        {
            base.Update(gameTime);

            if (Health <= 0)
                OnDeath();
        }

        abstract internal void OnDeath();

        public void DropBossCoins(int Coins)
        {
            int NoBossCoins = (int)Math.Round(Coins + Globals.RandomNormal());
            NoBossCoins = NoBossCoins == 0 ? 1 : NoBossCoins;

            for (int i = 0; i < NoBossCoins; i++)
                Globals.CurrentRoom.Pickups.Add(new BossCoin(Globals.RandomPointWithinRectangle(Globals.FloorRectangle)));
        }
    }
}
