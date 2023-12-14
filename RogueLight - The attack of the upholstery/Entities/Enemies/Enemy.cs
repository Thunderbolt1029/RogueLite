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
    abstract class Enemy : Entity
    {
        public int Health, MaxHealth;
        public float GracePeriodLength = 0.2f, GracePeriodRemaining = 0;
        public Vector2 MovingDirection;
        public float LookingRotation = 0f;

        public bool Boss = false;

        protected Vector2 DirectionToPlayer
        {
            get => (Globals.player.Centre - Centre).Length() < 0.01f * Globals.WindowScale ? Vector2.Zero : Vector2.Normalize(Globals.player.Centre - Centre);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ActiveTexture, Position, null, Color.White, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 0);

            if (Boss)
            {
                Rectangle BossHealthBarRectangle = new Rectangle(Globals.GameWindowRectangle.Center.X - (int)(Globals.BossHealthBar[0].Width * Globals.WindowScale / 2), Globals.GameWindowRectangle.Bottom - (int)(Globals.BossHealthBar[0].Height * Globals.WindowScale * 1.5), (int)(Globals.BossHealthBar[0].Width * Globals.WindowScale), (int)(Globals.BossHealthBar[0].Height * Globals.WindowScale));
                Rectangle FullBarRectangle = new Rectangle(BossHealthBarRectangle.Location, new Point((int)(BossHealthBarRectangle.Width * ((float)Health / (float)MaxHealth)), BossHealthBarRectangle.Height));

                spriteBatch.Draw(Globals.BossHealthBar[0], BossHealthBarRectangle, Color.White);
                spriteBatch.Draw(Globals.BossHealthBar[1], FullBarRectangle, new Rectangle(new Point(0), new Point((int)(Globals.BossHealthBar[1].Width * ((float)Health / (float)MaxHealth)), Globals.BossHealthBar[1].Height)), Color.White);
            }
            else
            {
                string OutHealth = $"-- {Health} --";
                spriteBatch.DrawString(Fonts.Health.SpriteFont, OutHealth, Fonts.Health.PositionOffset(OutHealth, HitBox) - new Vector2(0, Fonts.Health.SpriteFont.MeasureString(OutHealth).Y) * 0.7f - new Vector2(0, HitBox.Height) / 2f, Fonts.Health.Color);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Move(MovingDirection == new Vector2(0) ? MovingDirection * LinearVelocity : Vector2.Normalize(MovingDirection) * LinearVelocity, out Position);

            // Decrease grace period after being hit if it is more than 0
            if (GracePeriodRemaining > 0)
                GracePeriodRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (GracePeriodRemaining < 0)
                GracePeriodRemaining = 0;

            int Damage = DamageToTake();
            if (Damage > 0 && GracePeriodRemaining <= 0)
            {
                Health -= Damage;
                GracePeriodRemaining = GracePeriodLength;
                Globals.player.Pickups.ForEach(pickup => pickup.OnEnemyHit());
            }

            if (Health <= 0)
            {
                isRemoved = true;
                Globals.player.Pickups.ForEach(pickup => pickup.OnKill());
            }
        }

        int MaxProjectileLength = 10;
        Queue<Projectile> projectiles = new Queue<Projectile>();
        int DamageToTake()
        {
            int Damage = 0;

            foreach (Projectile projectile in Globals.Entities.OfType<Projectile>().Where(x => !projectiles.ToList().Contains(x)))
                if (projectile.Shooter == Globals.player && projectile.Intersects(this))
                {
                    Damage += projectile.Damage;
                    projectile.PiercingCount--;
                    projectiles.Enqueue(projectile);
                    if (projectiles.Count > MaxProjectileLength)
                        projectiles.Dequeue();
                }

            return Damage;
        }
    }
}
