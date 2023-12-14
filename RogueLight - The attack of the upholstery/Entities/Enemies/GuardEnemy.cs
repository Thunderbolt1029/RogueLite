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
    class GuardEnemy : Enemy
    {
        public const int StatesCount = 4;
        public enum State
        {
            Idle,
            Charging,
            Chasing,
            Cooldown
        }

        Dictionary<State, Texture2D> StateTextures;
        public override Texture2D ActiveTexture => StateTextures[CurrentState];

        State CurrentState = State.Idle;

        float ChargingTime = 0;
        float ChargingTimeLength;

        List<TilePath> ChaseRoute = new List<TilePath>();
        int ChaseIndex = 0;
        Vector2 PlayerPosition;

        float CooldownTime = 0;
        float CooldownTimeLength;

        const int InflateAmount = 0;
        Rectangle InflatedHitbox => new Rectangle(HitBox.X - InflateAmount, HitBox.Y - InflateAmount, HitBox.Width + InflateAmount * 2, HitBox.Height + InflateAmount * 2);

        public GuardEnemy(Dictionary<State, Texture2D> Textures, float Scale, Vector2 Centre)
        {
            StateTextures = Textures;
            this.Scale = Scale;

            this.Centre = Centre;

            Speed = 15f;

            ShouldCollideWithPlayer = true;

            Health = 30;

            ChargingTimeLength = 1f;
            CooldownTimeLength = 1.1f;

            MovingDirection = new Vector2(0);
        }

        public override void Update(GameTime gameTime)
        {
            switch (CurrentState)
            {
                case State.Idle:
                    if ((Globals.player.Centre - Centre).Length() < 400 * Globals.WindowScale)
                    {
                        ChargingTime = ChargingTimeLength;
                        CurrentState = State.Charging;
                        PlayerPosition = Globals.player.Centre;
                    }
                    break;

                case State.Charging:
                    ChargingTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (ChargingTime <= 0)
                    {
                        ChaseRoute = AStar.Pathfinding(Centre.ToPoint(), PlayerPosition.ToPoint(),
                            ((Func<List<Rectangle>>)(() =>
                            {
                                List<Rectangle> OutList = new List<Rectangle>();
                                OutList.AddRange(Globals.RoomCollisionBoxes);
                                OutList.AddRange(Globals.ActiveDoors.ConvertAll(x => x.CollisionBox));
                                return OutList;
                            }
                            ))(), InflatedHitbox);
                        ChaseIndex = 0;

                        CurrentState = State.Chasing;
                    }
                    break;

                case State.Chasing:
                    if (ChaseIndex < ChaseRoute.Count)
                    {
                        MovingDirection = ChaseRoute[ChaseIndex].Centre - Centre;
                        Rotation = VectorToAngle(MovingDirection);
                        if (MovingDirection.Length() < 20)
                            ChaseIndex++;

                    }
                    else
                    {
                        CooldownTime = CooldownTimeLength;

                        CurrentState = State.Cooldown;
                    }
                    break;

                case State.Cooldown:
                    MovingDirection = new Vector2(0);
                    CooldownTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (CooldownTime <= 0)
                        CurrentState = State.Idle;
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Globals.DebugModeEnabled)
            {
                for (int i = 0; i < 100; i++)
                    spriteBatch.Draw(Globals.ColorTexture, new Rectangle(Centre.ToPoint(), new Point((int)(250 * Globals.WindowScale), 1)), null, Color.Red, i * MathHelper.TwoPi / 100, new Vector2(0, 0.5f), SpriteEffects.None, 0f);


                foreach (TilePath tilePath in ChaseRoute)
                    spriteBatch.Draw(Globals.ColorTexture, new Rectangle(tilePath.Pos, new Point(AStar.TileSize)), Color.LightBlue);
            }
            base.Draw(spriteBatch);
        }
    }
}
