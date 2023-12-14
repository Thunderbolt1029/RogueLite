using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    abstract class SingleHandedWeapon : Weapon
    {
        float attackReloadTime;
        public float AttackReloadTime => attackReloadTime * player.ReloadTimeScalar;
        public float AttackReloadTimeRemaining;
        internal void AttackReloadTimeSet(float value)
        {
            attackReloadTime = value;
        }

        public bool Primary;

        public override void Update(GameTime gameTime)
        {
            UpdateReloadTimes(gameTime);
        }

        public void UpdateReloadTimes(GameTime gameTime)
        {
            if (AttackReloadTimeRemaining > 0)
                AttackReloadTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (AttackReloadTimeRemaining < 0)
                AttackReloadTimeRemaining = 0;
        }

        public virtual void Attack()
        {
            AttackReloadTimeRemaining = AttackReloadTime;
            if (Primary)
                player.Pickups.ForEach(pickup => pickup.OnPrimaryAttack());
            else
                player.Pickups.ForEach(pickup => pickup.OnSecondaryAttack());
        }
    }
}
