using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueLight___The_attack_of_the_upholstery
{
    abstract class Weapon
    {
        public static Dictionary<WeaponType, Texture2D> WeaponTextures = new Dictionary<WeaponType, Texture2D>();
        public Texture2D texture => WeaponTextures[WeaponType];

        public WeaponType WeaponType;

        public string Lore;

        public Player player;
        public Vector2 Position;
        public float Rotation;

        public float Scale;

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public static float VectorToAngle(Vector2 vector) => (float)Math.Atan2(vector.Y, vector.X);
        public static Vector2 AngleToVector(float angle) => Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
        public Vector2 DirectionToMouse
        {
            get => Vector2.Normalize(Globals.CurrentMouseState.Position.ToVector2() - player.Centre);
        }
    }
}
