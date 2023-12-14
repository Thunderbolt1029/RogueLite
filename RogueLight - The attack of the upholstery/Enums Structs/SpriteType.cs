using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueLight___The_attack_of_the_upholstery
{
    struct SpriteType
    {
        public enum Looking
        {
            Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft, Count, // Looking Directions
        }
        public enum Chest
        {
            Opened, Closed, Count // Chest Textures
        }
        public enum Spawner
        {
            Idle, Spawning, Count
        }
    }
}