using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Tile
    {
        public int gCost { get; set; }
        public int hCost { get; set; }
        public int fCost => gCost + hCost;
        public Point Pos;
        public Tile Parent;

        public void SetDistance(int targetX, int targetY)
        {
            int dstX = Math.Abs(Pos.X - targetX);
            int dstY = Math.Abs(Pos.Y - targetY);

            if (dstX > dstY)
                hCost = 14 * dstY + 10 * (dstX - dstY);
            else
                hCost = 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
