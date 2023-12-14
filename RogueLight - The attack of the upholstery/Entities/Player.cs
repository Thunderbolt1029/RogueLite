using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Player : Entity
    {
        public override Texture2D ActiveTexture { get => Textures[(int)(SpriteType.Looking)Math.Floor((MathHelper.WrapAngle(LookingRotation - 0.625f * MathHelper.Pi) / MathHelper.PiOver4) + 4) + (int)characterType * (int)SpriteType.Looking.Count]; }

        static Rectangle[] MoveRoomChecks = new Rectangle[]
        {
            new Rectangle(Globals.GameWindowRectangle.Left, Globals.GameWindowRectangle.Top, Globals.GameWindowRectangle.Width, 5),         // Up
            new Rectangle(Globals.GameWindowRectangle.Right - 5, Globals.GameWindowRectangle.Top, 5, Globals.GameWindowRectangle.Height),   // Right
            new Rectangle(Globals.GameWindowRectangle.Left, Globals.GameWindowRectangle.Bottom - 5, Globals.GameWindowRectangle.Width, 5),  // Down
            new Rectangle(Globals.GameWindowRectangle.Left, Globals.GameWindowRectangle.Top, 5, Globals.GameWindowRectangle.Height)         // Left
        };

        public float LookingRotation = 0f;

        public float GracePeriodLength = 1.5f, GracePeriodRemaining = 0;

        public bool Moving;

        public override float LinearVelocity
        {
            get => Speed * MoveSpeed * Globals.WindowScale;
        }

        public bool Dashing;
        public float DashTime => 0.13f;
        public float DashTimeRemaining = 0;
        public float DashReloadTime => 0.7f * DashReloadTimeScalar;
        public float DashReloadTimeRemaining = 0;
        public float DashingSpeed => 25f * DashDistance;
        public KeyboardState KeyboardStateBeforeDashing;

        float DodgeReloadTimeRemaining = 0f;

        public bool Invunerable
        {
            get => Dashing || GracePeriodRemaining > 0;
        }

        public float TotalHearts;
        public float HeartsRemaining;

        public float Damage;
        public float ReloadTimeScalar;

        public float DodgeChance;
        public float DodgeReloadTime;
        public float MoveSpeed;
        public float DashDistance;
        public float DashReloadTimeScalar;

        public Weapon[] EquippedWeapons = new Weapon[2];
        public List<Pickup> Pickups = new List<Pickup>();

        public CharacterType characterType;
        public Character character
        {
            get
            {
                return Character.Characters[characterType];
            }
        }

        bool FirstFrameDeath = true;

        public float TimeSinceDamageTake = 10f;

        public bool Selecting = false;

        public Player(List<Texture2D> Textures, float Scale, CharacterType characterType, Vector2 Centre)
        {
            this.Textures = Textures;
            this.Scale = Scale;

            this.characterType = characterType;
            Pickups.AddRange(character.StartingItems);

            this.Centre = Centre;

            Speed = 3.3f;

            TotalHearts = character.TotalHearts;
            HeartsRemaining = character.StartingHearts;

            ShouldCollideWithPlayer = false;

            Damage = character.BaseDamage;
            ReloadTimeScalar = character.BaseReloadTime;
            DodgeChance = character.DodgeChance;
            DodgeReloadTime = character.DodgeReloadTime;
            MoveSpeed = character.MoveSpeed;
            DashDistance = character.DashDistance;
            DashReloadTimeScalar = character.DashReloadTime;
    }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ActiveTexture, Position, null, Color.White, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0);
            for (int i = 0; i < 2; i++)
                if (EquippedWeapons[i] != null)
                    EquippedWeapons[i].Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            // Keyboard Movement
            Vector2 MovementVector = Vector2.Zero;

            if (Globals.CurrentKeyboardState.IsKeyDown(Globals.Controls.Dash) && DashReloadTimeRemaining <= 0 && !Dashing)
                Dash();
            else if (DashTimeRemaining <= 0)
            {
                Dashing = false;
                Pickups.ForEach(pickup => pickup.OnEndDash());
            }
            if (Dashing)
            {
                if (KeyboardStateBeforeDashing.IsKeyDown(Globals.Controls.Up))
                    MovementVector += new Vector2(0, -1);
                if (KeyboardStateBeforeDashing.IsKeyDown(Globals.Controls.Right))
                    MovementVector += new Vector2(1, 0);
                if (KeyboardStateBeforeDashing.IsKeyDown(Globals.Controls.Down))
                    MovementVector += new Vector2(0, 1);
                if (KeyboardStateBeforeDashing.IsKeyDown(Globals.Controls.Left))
                    MovementVector += new Vector2(-1, 0);

                if (DashTimeRemaining > 0)
                    DashTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                else if (DashTimeRemaining < 0)
                    DashTimeRemaining = 0;
            }
            else
            {
                if (Globals.CurrentKeyboardState.IsKeyDown(Globals.Controls.Up))
                    MovementVector += new Vector2(0, -1);
                if (Globals.CurrentKeyboardState.IsKeyDown(Globals.Controls.Right))
                    MovementVector += new Vector2(1, 0);
                if (Globals.CurrentKeyboardState.IsKeyDown(Globals.Controls.Down))
                    MovementVector += new Vector2(0, 1);
                if (Globals.CurrentKeyboardState.IsKeyDown(Globals.Controls.Left))
                    MovementVector += new Vector2(-1, 0);
            }
            Moving = MovementVector != Vector2.Zero;
            if (Moving) MovementVector = Vector2.Normalize(MovementVector) * (Dashing ? DashingSpeed : LinearVelocity);
            Move(MovementVector, out Position);

            // Look towards mouse
            Vector2 MousePositionFromPlayer = Globals.CurrentMouseState.Position.ToVector2() - Centre;
            LookingRotation = VectorToAngle(MousePositionFromPlayer);

            for (int i = 0; i < 2; i++)
                if (EquippedWeapons[i] != null)
                    EquippedWeapons[i].Update(gameTime);

            // Attack
            if (EquippedWeapons[0] is TwoHandedWeapon)
            {
                if (Globals.CurrentMouseState.LeftButton == ButtonState.Pressed && Globals.PreviousMouseState.LeftButton == ButtonState.Released)
                    ((TwoHandedWeapon)EquippedWeapons[0]).MainAttack(); // Left Attack
                else if (Globals.CurrentMouseState.RightButton == ButtonState.Pressed && Globals.PreviousMouseState.RightButton == ButtonState.Released)
                    ((TwoHandedWeapon)EquippedWeapons[0]).SecondaryAttack(); // Right Attack
            }
            else
            {
                if (Globals.CurrentMouseState.LeftButton == ButtonState.Pressed && Globals.PreviousMouseState.LeftButton == ButtonState.Released && EquippedWeapons[0] != null)
                    ((SingleHandedWeapon)EquippedWeapons[0]).Attack(); // Left Attack
                else if (Globals.CurrentMouseState.RightButton == ButtonState.Pressed && Globals.PreviousMouseState.RightButton == ButtonState.Released && EquippedWeapons[1] != null)
                    ((SingleHandedWeapon)EquippedWeapons[1]).Attack(); // Right Attack
            }
            

            // Decrease grace period after being hit if it is more than 0
            if (GracePeriodRemaining > 0)
                GracePeriodRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (GracePeriodRemaining < 0)
                GracePeriodRemaining = 0;

            if (DashReloadTimeRemaining > 0)
                DashReloadTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (DashReloadTimeRemaining < 0)
                DashReloadTimeRemaining = 0;

            if (DodgeReloadTimeRemaining > 0)
                DodgeReloadTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (DodgeReloadTimeRemaining < 0)
                DodgeReloadTimeRemaining = 0;

            Pickups.ForEach(pickup => pickup.UpdateReloadTimes(gameTime));

            TimeSinceDamageTake += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GracePeriodRemaining <= 0 && ShouldTakeDamage())
            {
                Pickups.ForEach(pickup => pickup.OnPlayerHit());
                if (RollForDodge())
                {
                    HeartsRemaining--;
                    GracePeriodRemaining = GracePeriodLength;
                    TimeSinceDamageTake = 0;

                    Pickups.ForEach(pickup => pickup.OnDamage());
                }
            }          

            if (HeartsRemaining == 0 && FirstFrameDeath)
            {
                Globals.YouDied = true;
                Globals.RunTime = DateTime.Now - Globals.RunStartTime;
                Globals.FirstFrameWonDeath = true;
                FirstFrameDeath = false;
                MediaPlayer.Stop();
                MediaPlayer.Play(Sounds.DeathTheme);
            }
            else
            {
                FirstFrameDeath = true;
            }
        }

        bool RollForDodge()
        {
            if (DodgeReloadTimeRemaining <= 0 && Globals.random.NextDouble() > DodgeChance)
            {
                DodgeReloadTimeRemaining = DodgeReloadTime;
                return true;
            }

            return false;
        }

        void Dash()
        {
            if (Globals.CurrentKeyboardState.GetPressedKeys().Any(x => x == Globals.Controls.Up || x == Globals.Controls.Down || x == Globals.Controls.Left || x == Globals.Controls.Right))
            {
                Dashing = true;
                DashTimeRemaining = DashTime;
                DashReloadTimeRemaining = DashReloadTime + DashTime;
                KeyboardStateBeforeDashing = Globals.CurrentKeyboardState;
                Pickups.ForEach(pickup => pickup.OnStartDash());
            }
        }

        bool ShouldTakeDamage()
        {
            foreach (Entity entity in Globals.Entities)
                if (entity.ShouldCollideWithPlayer && (!(entity is Nail) && !Dashing || (entity is Nail && (((Nail)entity).Blue || !Dashing) && (!((Nail)entity).Blue || Moving))) && entity.Intersects(this))
                    return true;

            return false;
        }

        public bool CheckMoveRoom(out MoveDirection? moveDirection)
        {
            bool ShouldMoveRoom = false;
            moveDirection = null;

            for (int i = 0; i < MoveRoomChecks.Length; i++)
                if (Intersects(MoveRoomChecks[i]))
                    moveDirection = (MoveDirection)i;

            if (moveDirection != null)
                ShouldMoveRoom = true;

            return ShouldMoveRoom;
        }
    }
}
