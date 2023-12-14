using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Mimic : Boss
    {
        enum MimicStates
        {
            Idle,
            FastShoot,
            BulletWall,
            CircularShoot,

            Count
        }

        public override Texture2D ActiveTexture => Textures[0];
        Stack<MimicStates> AttackState = new Stack<MimicStates>();

        float ShootRotation = 0f;

        float FastShootReloadTime, FastShootReloadTimeRemaining = 0;
        float BulletWallReloadTime, BulletWallReloadTimeRemaining = 0;
        float CircularShootReloadTime, CircularShootReloadTimeRemaining = 0;

        int FastShootNoOfBullets = 10;
        int NoOfBulletWalls = 3, NoOfBulletWallsRemaining = 0;
        int CircularShootCount = 30, CircularShootCountRemaining = 0;

        float TimeSinceChange = 0f;

        public Mimic(List<Texture2D> Textures, Vector2 Centre)
        {
            this.Textures = Textures;
            this.Centre = Centre;

            Scale = 0.6f;

            MaxHealth = Health = 80;
            Boss = true;

            AttackState.Push(MimicStates.Idle);

            FastShootReloadTime = 0.5f;
            BulletWallReloadTime = 0.03f;
            CircularShootReloadTime = 0.07f;
        }

        public override void Update(GameTime gameTime)
        {
            TimeSinceChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (AttackState.Peek())
            {
                case MimicStates.Idle:
                    if (TimeSinceChange >= 0.9)
                    {
                        int rand = Globals.random.Next((int)MimicStates.Count);
                        if (rand != 0)
                            AttackState.Push((MimicStates)rand);

                        TimeSinceChange = 0;
                    }
                    break;

                case MimicStates.FastShoot:
                    if (FastShootReloadTimeRemaining > 0)
                        FastShootReloadTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else if (FastShootReloadTimeRemaining <= 0)
                    {
                        FastShootReloadTimeRemaining = FastShootReloadTime;

                        for (int i = 0; i < FastShootNoOfBullets; i++)
                            Globals.Entities.Add(new Bullet(this, ShootRotation + i * MathHelper.TwoPi / FastShootNoOfBullets));

                        ShootRotation += MathHelper.TwoPi / (FastShootNoOfBullets * 3);
                    }

                    if (TimeSinceChange >= 3)
                    {
                        if (Globals.random.NextDouble() < 0.6f)
                            AttackState.Pop();
                        else
                        {
                            int rand = Globals.random.Next((int)MimicStates.Count);
                            if (rand == 0)
                                AttackState.Pop();
                            else
                                AttackState.Push((MimicStates)rand);
                        }

                        TimeSinceChange = 0;
                    }

                    break;

                case MimicStates.BulletWall:
                    if (NoOfBulletWallsRemaining == 0)
                        NoOfBulletWallsRemaining = NoOfBulletWalls;

                    if (BulletWallReloadTimeRemaining > 0)
                        BulletWallReloadTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else if (BulletWallReloadTimeRemaining <= 0)
                    {
                        BulletWallReloadTimeRemaining = BulletWallReloadTime;

                        for (int i = 0; i < 50; i++)
                            Globals.Entities.Add(new Bullet(this, ShootRotation + i * MathHelper.TwoPi / 50));
                        ShootRotation += 0.1f;

                        NoOfBulletWallsRemaining--;
                    }

                    if (NoOfBulletWallsRemaining == 0)
                    {
                        AttackState.Pop();
                        TimeSinceChange = 0;
                    }
                    break;

                case MimicStates.CircularShoot:
                    if (CircularShootCountRemaining == 0)
                        CircularShootCountRemaining = CircularShootCount;

                    if (CircularShootReloadTimeRemaining > 0)
                        CircularShootReloadTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else if (CircularShootReloadTimeRemaining <= 0)
                    {
                        CircularShootReloadTimeRemaining = CircularShootReloadTime;

                        Globals.Entities.Add(new Bullet(this, ShootRotation));
                        Globals.Entities.Add(new Bullet(this, ShootRotation - MathHelper.Pi));

                        ShootRotation += MathHelper.TwoPi / CircularShootCount;

                        CircularShootCountRemaining--;
                    }

                    if (CircularShootCountRemaining == 0)
                    {
                        AttackState.Pop();
                        TimeSinceChange = 0;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        internal override void OnDeath() 
        {
            DropBossCoins(2);

            Pickup Item = Globals.random.NextDouble() < 0.4 ? Pickup.GetRandomPickup(null, 1) : Pickup.GetRandomPickup(null, 2);

            Globals.player.Pickups.Add(Item);
            Globals.Components.Add(new NotificationPopup(Fonts.Debug, $"You picked up a {Item.OutName}", new Vector2(Globals.GameWindowRectangle.Center.X, Globals.GameWindowRectangle.Bottom - 100), 3, 0.7f, new Point(10)));
        }
    }
}
