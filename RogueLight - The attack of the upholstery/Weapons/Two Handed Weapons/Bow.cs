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
    class Bow : TwoHandedWeapon
    {
        public Bow(Player player)
        {
            WeaponType = WeaponType.Bow;

            this.player = player;

            Scale = 1.5f;

            MainAttackReloadTimeSet(0.5f);
            SecondaryAttackReloadTimeSet(2f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, player.Centre, null, Color.White, player.LookingRotation, new Vector2(0, texture.Height / 2), Scale, SpriteEffects.None, 0);
        }

        public override void MainAttack()
        {
            if (MainAttackReloadTimeRemaining <= 0)
            {
                Globals.Entities.Add(new Arrow(this, VectorToAngle(DirectionToMouse), (int)Math.Round(5 * player.Damage)));

                base.MainAttack();
            }
        }

        public override void SecondaryAttack()
        {
            int AmountOfArrows = 4;
            float SpreadAngle = 30;

            if (SecondaryAttackReloadTimeRemaining <= 0)
            {
                for (int i = 0; i < AmountOfArrows; i++)
                {
                    Globals.Entities.Add(new Arrow(this, VectorToAngle(DirectionToMouse) + (i - (((float)AmountOfArrows - 1) / 2)) * MathHelper.TwoPi * SpreadAngle / 360 / (AmountOfArrows - 1), (int)Math.Round(3 * player.Damage)));
                }

                base.SecondaryAttack();
            }
        }
    }
}
