using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Room
    {
        public RoomType Type;
        public bool Cleared;
        public Point Pos;

        // ShopRoom
        public BuyableItem[] BuyableItems;

        public List<Pickup> Pickups = new List<Pickup>();

        public static RoomType[,] ToRoomTypeArr(Room[,] RoomArr)
        {
            RoomType[,] OutArr = new RoomType[RoomArr.GetLength(0), RoomArr.GetLength(1)];
            for (int x = 0; x < RoomArr.GetLength(0); x++)
                for (int y = 0; y < RoomArr.GetLength(1); y++)
                    OutArr[x, y] = RoomArr[x, y].Type;
            return OutArr;
        }
    }
}
