using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLight___The_attack_of_the_upholstery
{
    abstract class Pickup
    {
        public const int MaxLevel = 3;

        /// <summary>
        /// First array is level
        /// Second array is the pickup's ID
        /// </summary>
        public static Type[][] Pickups = new Type[][]
        {
            // Level 1 Pickups
            new Type[]
            {
                typeof(DamageBoost),
                typeof(ReloadTimeBoost),
                typeof(DodgeChanceBoost),
                typeof(DodgeReloadTimeBoost),
                typeof(MoveSpeedBoost),
                typeof(DashDistanceBoost),
                typeof(DashReloadTimeBoost),
            },

            // Level 2 Pickups
            new Type[]
            {
                typeof(HeartContainer),
                typeof(HealthContainer)
            },

            // Level 3 Pickups
            new Type[]
            {
                typeof(LifeSteal)
            }
        };

        public static Type[] SpecialPickups = new Type[]
        {
            typeof(BossCoin),
            typeof(Heart),
        };

        public string OutName;
        public int Level;
        public bool Special = false;

        public Texture2D Texture => PickupTextures[GetType()];
        public static Dictionary<Type, Texture2D> PickupTextures = new Dictionary<Type, Texture2D>();

        public Vector2 Position;
        public float Scale;
        public Vector2 Centre 
        { 
            get => Position + Texture.Bounds.Size.ToVector2() * Scale / 2;
            set { Position = value - Texture.Bounds.Size.ToVector2() * Scale / 2; }
        }

        public bool PickedUp;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }

        public void Update(GameTime gameTime)
        {
            if (Globals.player.Intersects(new Rectangle(Position.ToPoint(), (Texture.Bounds.Size.ToVector2() * Scale).ToPoint())))
            {
                OnPickup();
                PickedUp = true;
                Globals.player.Pickups.Add(this);
                Globals.Components.Add(new NotificationPopup(Fonts.Debug, $"You picked up a {OutName}", new Vector2(Globals.GameWindowRectangle.Center.X, Globals.GameWindowRectangle.Bottom - 100), 3, 0.7f, new Point(10)));
            }
        }

        /// <param name="Chances">
        /// Null for default level chances
        /// </param>
        /// <param name="Level">
        /// Level of the generated pickup
        /// If level = 0 any level of pickup is picked
        /// </param>
        public static Pickup GetRandomPickup(double[] Chances, int Level = 0)
        {
            Chances = Chances ?? new double[] { 0.6, 0.3, 0.1 };

            if (Level == 0)
            {
                double rand = Globals.random.NextDouble();

                double Chance = 0;
                for (Level = 0; Level < Chances.Length; Level++)
                {
                    Chance += Chances[Level];
                    if (rand < Chance)
                        break;
                }
                Level++;
            }

            Type RandType = Pickups[Level - 1][Globals.random.Next(Pickups[Level - 1].Length)];
            return (Pickup)Activator.CreateInstance(RandType);
        }

        public static Pickup RandomSpecialPickup() => (Pickup)Activator.CreateInstance(SpecialPickups[Globals.random.Next(SpecialPickups.Length)]);

        public virtual void UpdateReloadTimes(GameTime gameTime) { }

        /// <summary>
        /// Runs once when a pickup is picked up
        /// </summary>
        public virtual void OnPickup() { }
        
        /// <summary>
        /// Runs when the player gets hit
        /// </summary>
        public virtual void OnPlayerHit() { }

        /// <summary>
        /// Runs when the player takes damage
        /// </summary>
        public virtual void OnDamage() { }

        /// <summary>
        /// Runs on player using primary attack
        /// </summary>
        public virtual void OnPrimaryAttack() { }

        /// <summary>
        /// Runs on player using secondary attack
        /// </summary>
        public virtual void OnSecondaryAttack() { }

        /// <summary>
        /// Runs when an enemy takes damage
        /// </summary>
        public virtual void OnEnemyHit() { }

        /// <summary>
        /// Runs when the player kills an enemy
        /// </summary>
        public virtual void OnKill() { }

        /// <summary>
        /// Runs when the player starts their dash
        /// </summary>
        public virtual void OnStartDash() { }

        /// <summary>
        /// Runs when the player ends their dash
        /// </summary>
        public virtual void OnEndDash() { }
    }
}
