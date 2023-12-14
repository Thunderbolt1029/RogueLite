using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLight___The_attack_of_the_upholstery
{
    class Axe : TwoHandedWeapon
    {
        public Axe(Player player)
        {
            WeaponType = WeaponType.Axe;

            this.player = player;

            Scale = 1f;

            MainAttackReloadTimeSet(0.3f);
            SecondaryAttackReloadTimeSet(1f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bool LookingLeft = player.LookingRotation < -MathHelper.PiOver2 || player.LookingRotation > MathHelper.PiOver2;
            Position = player.Centre + AngleToVector(Globals.player.LookingRotation) * (Math.Abs(MainAttackReloadTimeRemaining - MainAttackReloadTime / 2) * -70f / MainAttackReloadTime + 70f / 2 - 10f);
            Rotation = player.LookingRotation + (SecondaryAttackReloadTimeRemaining > 0 ? (SecondaryAttackReloadTimeRemaining - SecondaryAttackReloadTime / 2) * 3f / SecondaryAttackReloadTime : 0) * (LookingLeft ? 1 : -1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, Rotation, new Vector2(0, texture.Height / 2), (SecondaryAttackReloadTimeRemaining <= 0 ? 1f : 1.5f) * Scale, SpriteEffects.None, 0);
        }

        public override void MainAttack()
        {
            if (MainAttackReloadTimeRemaining <= 0 && SecondaryAttackReloadTimeRemaining <= 0)
            {
                Globals.Entities.Add(new MeleeWeapon(player, this, (int)Math.Round(8 * player.Damage), MainAttackReloadTime));

                base.MainAttack();
            }
        }

        public override void SecondaryAttack()
        {
            if (MainAttackReloadTimeRemaining <= 0 && SecondaryAttackReloadTimeRemaining <= 0)
            {
                Globals.Entities.Add(new MeleeWeapon(player, this, (int)Math.Round(30 * player.Damage), SecondaryAttackReloadTime));

                base.SecondaryAttack();
            }
        }
    }
}
