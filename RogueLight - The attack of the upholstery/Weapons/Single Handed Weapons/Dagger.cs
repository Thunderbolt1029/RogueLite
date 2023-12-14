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
    class Dagger : SingleHandedWeapon
    {
        public Dagger(Player player, bool Primary)
        {
            WeaponType = WeaponType.Dagger;

            this.player = player;
            this.Primary = Primary;

            Scale = 1.2f;

            AttackReloadTimeSet(0.2f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bool LookingLeft = player.LookingRotation < -MathHelper.PiOver2 || player.LookingRotation > MathHelper.PiOver2;
            Position = player.Centre + AngleToVector(Globals.player.LookingRotation) * 10f;
            Rotation = player.LookingRotation + (AttackReloadTimeRemaining > 0 ? (AttackReloadTimeRemaining - AttackReloadTime / 2) / AttackReloadTime : 0) * (LookingLeft ? 1 : -1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, Rotation, new Vector2(0, texture.Height / 2), Scale, SpriteEffects.None, 0);
        }

        public override void Attack()
        {
            if (AttackReloadTimeRemaining <= 0)
            {
                Globals.Entities.Add(new MeleeWeapon(player, this, (int)Math.Round(15 * player.Damage), AttackReloadTime));

                base.Attack();
            }
        }
    }
}
