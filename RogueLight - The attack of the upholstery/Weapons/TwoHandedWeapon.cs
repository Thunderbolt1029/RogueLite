using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RogueLight___The_attack_of_the_upholstery
{
    abstract class TwoHandedWeapon : Weapon
    {
        float mainAttackReloadTime;
        public float MainAttackReloadTime => mainAttackReloadTime * player.ReloadTimeScalar;
        public float MainAttackReloadTimeRemaining;
        internal void MainAttackReloadTimeSet(float value)
        {
            mainAttackReloadTime = value;
        }

        float secondaryAttackReloadTime;
        public float SecondaryAttackReloadTime => secondaryAttackReloadTime * player.ReloadTimeScalar;
        public float SecondaryAttackReloadTimeRemaining;
        internal void SecondaryAttackReloadTimeSet(float value)
        {
            secondaryAttackReloadTime = value;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateReloadTimes(gameTime);
        }

        public void UpdateReloadTimes(GameTime gameTime)
        {
            if (MainAttackReloadTimeRemaining > 0)
                MainAttackReloadTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (MainAttackReloadTimeRemaining < 0)
                MainAttackReloadTimeRemaining = 0;

            if (SecondaryAttackReloadTimeRemaining > 0)
                SecondaryAttackReloadTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (SecondaryAttackReloadTimeRemaining < 0)
                SecondaryAttackReloadTimeRemaining = 0;
        }

        public virtual void MainAttack()
        {
            MainAttackReloadTimeRemaining = MainAttackReloadTime;
            player.Pickups.ForEach(pickup => pickup.OnPrimaryAttack());
        }
        public virtual void SecondaryAttack()
        {
            SecondaryAttackReloadTimeRemaining = SecondaryAttackReloadTime;
            player.Pickups.ForEach(pickup => pickup.OnSecondaryAttack());
        }
    }
}
