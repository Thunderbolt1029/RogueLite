using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RogueLight___The_attack_of_the_upholstery
{
    static class Globals
    {
        public const bool DebugModeEnabled = false; // Shows things like hitboxes and centre lines

        public static ControlScheme Controls = new ControlScheme()
        {
            Up = Keys.W,
            Right = Keys.D,
            Down = Keys.S,
            Left = Keys.A,

            Dash = Keys.Space,
            Interact = Keys.E
        };

        public static Random random = new Random(); // For use in all random calculations
        public static Texture2D ColorTexture; // 1x1 white texture for use in procedural textures

        public static Rectangle ScreenRectangle
        {
            get => new Rectangle(0, 0, GameWindowWidth, GameWindowHeight);
        }
        public static Rectangle GameWindowRectangle // "Hitbox" for screen rectangle
        {
            get => new Rectangle((int)((GameWindowWidth - WindowScale * 1080) / 2), 0, (int)(WindowScale * 1080), GameWindowHeight);
        }
        public static int GameWindowWidth, GameWindowHeight; // Size of the window
        public static float WindowScale // Most constants need to be multiplied by this scale so that when the window is a different size constants stay in proportion
        {
            get => GameWindowHeight / 720f;
        }
        public static Rectangle FloorRectangle
        {
            get => new Rectangle((GameWindowRectangle.Location.ToVector2() + new Vector2(50) * WindowScale).ToPoint(), (GameWindowRectangle.Size.ToVector2() - new Vector2(100) * WindowScale).ToPoint());
        }

        public static float DifficultyMultiplier { get => 1 + (Stage - 1) * 0.2f;  }

        public static int Stage = 0; // Stage 0 is the lobby area
        public const int TotalNoStages = 1;
        public static Room[,] Map = new Room[10, 10]; // Room Map 10x10 (first digit is y, second x)
        public static Room CurrentRoom => Map[CurrentRoomPos.X, CurrentRoomPos.Y];
        public static Point CurrentRoomPos = new Point(5, 5); // The room pos that the player is currently in

        public static Rectangle[] RoomCollisionBoxes; // Where to put collisons for spikes / hole in ground
        public static Texture2D[] RoomTextures = new Texture2D[TotalNoStages + 1]; // Textures for a room
        public static List<Door> ActiveDoors = new List<Door>(); // Which doors to update / draw

        public static Player player; // The player
        public static List<Entity> Entities = new List<Entity>(); // List of all entities currently being drawn and updated - includes player

        // Current GameState e.g. InMenu or InGame
        public static GameState CurrentGameState;
        public static Dictionary<GameState, Button[]> GameStateButtons = new Dictionary<GameState, Button[]>(); // Buttons to be displayed during a specific GameState
        public static Button[] CurrentGameStateButtons
        {
            get 
            {
                return GameStateButtons[CurrentGameState];
            }
        }

        public static List<Component> Components = new List<Component>();

        // Current and previous state for the three input devices
        public static KeyboardState PreviousKeyboardState, CurrentKeyboardState;
        public static MouseState PreviousMouseState, CurrentMouseState;
        public static GamePadState PreviousGamePadState, CurrentGamePadState;

        public static int CurrentSaveGameID;
        public static int RunNo;
        public static int NoBossCoins;

        public static DateTime RunStartTime;
        public static TimeSpan RunTime;
        public static int NoBossCoinsCollected;
        public static int Score
        {
            get => (int)((NoBossCoinsCollected - 1 / RunTime.TotalSeconds) * 1000);
        }

        public static Texture2D[] BossHealthBar = new Texture2D[2];

        public static int CountEnemiesInCurrentRoom // No. of enimies that are currently in the Entities list
        {
            get
            {
                int i = 0;
                foreach (Entity entity in Entities)
                    if (entity is Enemy)
                        i++;
                return i;
            }
        }
        public static bool YouDied = false; // Whether the player is dead and the you died screen should be displayed
        public static bool GameWon = false; // Whether the game is won
        public static bool FirstFrameWonDeath = false;
        
        public static bool SecretRoom = false;

        // Resets the player after they press space on the you died screen
        public static void Reset()
        {  
            Stage = 0;

            YouDied = false;
            GameWon = false;

            player.HeartsRemaining = player.TotalHearts;

            Entities.Clear();
            Components.Clear();
        }

        public static double RandomNormal() => Math.Sqrt(-2.0 * Math.Log(random.NextDouble())) * Math.Sin(2.0 * Math.PI * random.NextDouble());

        public static Vector2 RandomPointWithinRectangle(Rectangle rectangle)
        {
            Vector2 vector = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            vector = new Vector2(vector.X * rectangle.Size.X, vector.Y * rectangle.Size.Y);
            vector += rectangle.Location.ToVector2();
            return vector;
        }

        public static Vector2 RandomPointWithinCircle(Vector2 Centre, float Radius)
        {
            Vector2 vector;

            do
            {
                vector = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);
            } while (vector.LengthSquared() > 1);

            vector *= Radius;
            vector += Centre;

            return vector;
        }
    }
}
