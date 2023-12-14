using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    class TilePath
    {
        public Point Pos;
        public Vector2 Centre => Pos.ToVector2() + new Vector2(AStar.TileSize) / 2; 

        public TilePath(int intX, int intY)
        {
            Pos = new Point(intX, intY);
        }
    }
}
