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
    class Staff : TwoHandedWeapon
    {
        float RecoveryTime = 0.2f, RecoveryTimeRemaining = 0f;

        public Staff(Player player)
        {
            WeaponType = WeaponType.Staff;

            this.player = player;

            Scale = 1.5f;

            MainAttackReloadTimeSet(0.3f);
            SecondaryAttackReloadTimeSet(1.8f);
        }

        public override void Update(GameTime gameTime)
        {
            if (RecoveryTimeRemaining > 0)
                RecoveryTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);

            Position = player.Centre + AngleToVector(Globals.player.LookingRotation) * (Math.Abs(MainAttackReloadTimeRemaining - MainAttackReloadTime / 2) * -70f / MainAttackReloadTime + 70f / 2 + 20f);
            Rotation = player.LookingRotation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, Rotation, new Vector2(0, texture.Height / 2), Scale, SpriteEffects.None, 0);
        }

        public override void MainAttack()
        {
            if (MainAttackReloadTimeRemaining <= 0 && RecoveryTimeRemaining <= 0)
            {
                Globals.Entities.Add(new MeleeWeapon(player, this, (int)Math.Round(8 * player.Damage), MainAttackReloadTime));

                base.MainAttack();
            }
        }

        public override void SecondaryAttack()
        {
            if (MainAttackReloadTimeRemaining <= 0 && SecondaryAttackReloadTimeRemaining <= 0)
            {
                Globals.Entities.Add(new EnergyBall(this, (int)Math.Round(15 * player.Damage)));
                RecoveryTimeRemaining = RecoveryTime;

                base.SecondaryAttack();
            }
        }
    }
}
