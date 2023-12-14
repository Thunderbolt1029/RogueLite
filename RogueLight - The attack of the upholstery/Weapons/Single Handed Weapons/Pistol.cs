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
    class Pistol : SingleHandedWeapon
    {
        bool LookingLeft => player.LookingRotation < -MathHelper.PiOver2 || player.LookingRotation > MathHelper.PiOver2;

        public Pistol(Player player, bool Primary)
        {
            WeaponType = WeaponType.Pistol;

            this.player = player;
            this.Primary = Primary;

            Scale = 1f;

            AttackReloadTimeSet(0.4f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Position = player.Centre + AngleToVector(Globals.player.LookingRotation) * 10f;
            Rotation = player.LookingRotation + (AttackReloadTimeRemaining > 0 ? AttackReloadTimeRemaining * 0.5f / AttackReloadTime : 0) * (LookingLeft ? 1 : -1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, Rotation, new Vector2(0, texture.Height / 2), Scale, LookingLeft ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
        }

        public override void Attack()
        {
            if (AttackReloadTimeRemaining <= 0)
            {
                Globals.Entities.Add(new Bullet(player, VectorToAngle(DirectionToMouse), (int)Math.Round(3 * player.Damage)));

                base.Attack();
            }
        }
    }
}
