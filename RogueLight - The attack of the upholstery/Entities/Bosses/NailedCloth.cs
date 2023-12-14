using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    class NailedCloth : Boss
    {
        public enum NailedClothState
        {
            Idle,
            Slash,
            SmallWall,
            FollowAim,

            Count
        }

        public override Texture2D ActiveTexture => Textures[(int)AttackStates.Peek()];
        Stack<NailedClothState> AttackStates = new Stack<NailedClothState>();

        float IdleReload = 0.7f, IdleReloadRemaining = 1f;

        int NailSlashCount = 3, NailSlashCountRemaining = 0;
        float NailSlashReload = 2f, NailSlashReloadRemaining = 0f;

        int SmallWallCount = 10, SmallWallCountRemaining = 0;
        float SmallWallReload = 0.7f, SmallWallReloadRemaining = 0f;

        int FollowAimCount = 20, FollowAimCountRemaining = 0;
        float FollowAimReload = 0.4f, FollowAimReloadRemaining = 0f;

        public NailedCloth(List<Texture2D> Textures, Vector2 Centre)
        {
            AttackStates.Push(NailedClothState.Idle);

            this.Textures = Textures;
            this.Centre = Centre;

            MaxHealth = Health = 150;
            Boss = true;


            NailSlashCountRemaining = NailSlashCount;
            SmallWallCountRemaining = SmallWallCount;
            FollowAimCountRemaining = FollowAimCount;
        }

        public override void Update(GameTime gameTime)
        {
            // Attacking
            switch (AttackStates.Peek())
            {
                case NailedClothState.Idle:
                    if (IdleReloadRemaining <= 0)
                    {
                        IdleReloadRemaining = IdleReload;
                        if (Globals.random.NextDouble() < 0.5f)
                            AttackStates.Push((NailedClothState)Globals.random.Next(1, (int)NailedClothState.Count));
                    }
                    else
                        IdleReloadRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    break;

                case NailedClothState.Slash:
                    if (NailSlashReloadRemaining <= 0)
                    {
                        NailSlashCountRemaining--;
                        NailSlash((MoveDirection)Globals.random.Next(4));
                        NailSlashReloadRemaining = NailSlashReload;
                    }
                    else
                        NailSlashReloadRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (NailSlashCountRemaining <= 0)
                    {
                        AttackStates.Pop();
                        NailSlashCountRemaining = NailSlashCount;
                        NailSlashReloadRemaining = 0;
                    }
                    break;

                case NailedClothState.SmallWall:
                    if (SmallWallReloadRemaining <= 0)
                    {
                        SmallWallCountRemaining--;
                        NailSmallWall();
                        SmallWallReloadRemaining = SmallWallReload;
                    }
                    else
                        SmallWallReloadRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (SmallWallCountRemaining <= 0)
                    {
                        AttackStates.Pop();
                        SmallWallCountRemaining = SmallWallCount;
                        SmallWallReloadRemaining = 0;
                    }
                    break;

                case NailedClothState.FollowAim:
                    if (FollowAimReloadRemaining <= 0)
                    {
                        FollowAimCountRemaining--;
                        NailFollowAim();
                        FollowAimReloadRemaining = FollowAimReload;
                    }
                    else
                        FollowAimReloadRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (FollowAimCountRemaining <= 0)
                    {
                        AttackStates.Pop();
                        FollowAimCountRemaining = FollowAimCount;
                        FollowAimReloadRemaining = 0;
                    }
                    break;

                default:
                    AttackStates.Pop();
                    break;
            }

            base.Update(gameTime);
        }

        internal override void OnDeath() 
        {
            DropBossCoins((int)Math.Round(4 * Globals.DifficultyMultiplier));
            Globals.Entities.Add(new Portal(Globals.GameWindowRectangle.Center.ToVector2(), Globals.Stage));
        }

        const int VerticalNailCount = 18, HorizontalNailCount = 8;
        const float VerticalNailSpacing = 80, HorizontalNailSpacing = 120;
        void NailSlash(MoveDirection Direction)
        {
            switch (Direction)
            {
                case MoveDirection.Up:
                    for (int i = 0; i < VerticalNailCount; i++)
                    {
                        float Index = i - (float)(VerticalNailCount - 1) / 2;
                        Globals.Entities.Add(new Nail(this, -MathHelper.PiOver2, new Vector2(Globals.GameWindowRectangle.Center.X + Index * VerticalNailSpacing, Globals.GameWindowRectangle.Bottom - 25), new Vector2(Globals.GameWindowRectangle.Center.X + Index * VerticalNailSpacing, Globals.GameWindowRectangle.Top + 50), Globals.random.Next(4) == 0));
                    }
                    break;
                case MoveDirection.Right:
                    for (int i = 0; i < HorizontalNailCount; i++)
                    {
                        float Index = i - (float)(HorizontalNailCount - 1) / 2;
                        Globals.Entities.Add(new Nail(this, 0, new Vector2(Globals.GameWindowRectangle.Left + 25, Globals.GameWindowRectangle.Center.Y + Index * HorizontalNailSpacing), new Vector2(Globals.GameWindowRectangle.Right - 50, Globals.GameWindowRectangle.Center.Y + Index * HorizontalNailSpacing), Globals.random.Next(4) == 0));
                    }
                    break;
                case MoveDirection.Down:
                    for (int i = 0; i < VerticalNailCount; i++)
                    {
                        float Index = i - (float)(VerticalNailCount - 1) / 2;
                        Globals.Entities.Add(new Nail(this, MathHelper.PiOver2, new Vector2(Globals.GameWindowRectangle.Center.X + Index * VerticalNailSpacing, Globals.GameWindowRectangle.Top + 25), new Vector2(Globals.GameWindowRectangle.Center.X + Index * VerticalNailSpacing, Globals.GameWindowRectangle.Bottom - 50), Globals.random.Next(4) == 0));
                    }
                    break;
                case MoveDirection.Left:
                    for (int i = 0; i < HorizontalNailCount; i++)
                    {
                        float Index = i - (float)(HorizontalNailCount - 1) / 2;
                        Globals.Entities.Add(new Nail(this, MathHelper.Pi, new Vector2(Globals.GameWindowRectangle.Right - 25, Globals.GameWindowRectangle.Center.Y + Index * HorizontalNailSpacing), new Vector2(Globals.GameWindowRectangle.Left + 50, Globals.GameWindowRectangle.Center.Y + Index * HorizontalNailSpacing), Globals.random.Next(4) == 0));
                    }
                    break;
            }
        }

        const int SmallWallNailCount = 5, SmallWallNailSpacing = 15;
        void NailSmallWall()
        {
            float Rotation = MathHelper.WrapAngle((float)Globals.random.NextDouble() * MathHelper.TwoPi);

            for (int i = 0; i < SmallWallNailCount; i++)
            {
                float Index = i - (float)(SmallWallNailCount - 1) / 2;
                Globals.Entities.Add(new Nail(this, Rotation, Globals.ScreenRectangle.Center.ToVector2() - AngleToVector(Rotation) * 500 + AngleToVector(Rotation + MathHelper.PiOver2) * Index * SmallWallNailSpacing, Globals.ScreenRectangle.Center.ToVector2() + AngleToVector(Rotation) * 900 + AngleToVector(Rotation + MathHelper.PiOver2) * Index * SmallWallNailSpacing));
            }
        }

        void NailFollowAim()
        {
            float Rotation = (float)(MathHelper.Pi * (Globals.random.NextDouble() - 0.5));
            if (Vector2.Dot(AngleToVector(Rotation), Globals.player.Centre - Globals.GameWindowRectangle.Center.ToVector2()) < 0)
                Rotation += MathHelper.Pi;
            Rotation = MathHelper.WrapAngle(Rotation);

            Globals.Entities.Add(new Nail(this, Rotation, Globals.GameWindowRectangle.Center.ToVector2() - AngleToVector(Rotation) * 350, Globals.GameWindowRectangle.Center.ToVector2() + Vector2.Normalize(Globals.player.Centre - Globals.GameWindowRectangle.Center.ToVector2()) * 575 * Globals.WindowScale));
        }
    }
}
