using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    struct Character
    {
        public static Dictionary<CharacterType, Character> Characters = new Dictionary<CharacterType, Character>()
        {
            { CharacterType.Guy,                new Character(5, 5, 1f, 1f, 0f, 1f, 1f, 1f, 1f, new List<Pickup>()) },
            { CharacterType.GuyWithHat, new Character(3, 3, 1.2f, 0.8f, 0.25f, 1f, 1.5f, 1.2f, 0.8f, new List<Pickup>()) },
            { CharacterType.GuyWithHelmet,      new Character(6, 6, 1.5f, 1.5f, 0f, 1f, 0.8f, 0.8f, 1f, new List<Pickup>()) },
            { CharacterType.GuyWithFangs,       new Character(4, 2, 1.2f, 1f, 0f, 1f, 1.1f, 1f, 0.9f, new List<Pickup>() { new LifeSteal() }) }
        };

        public int TotalHearts;
        public int StartingHearts;
        public float BaseDamage;
        public float BaseReloadTime;
        public float DodgeChance;
        public float DodgeReloadTime;
        public float MoveSpeed;
        public float DashDistance;
        public float DashReloadTime;
        public List<Pickup> StartingItems;

        Character(int TotalHearts, int StartingHearts, float BaseDamage, float BaseReloadTime, float DodgeChance, float DodgeReloadTime, float MoveSpeed, float DashDistance, float DashReloadTime, List<Pickup> StartingItems)
        {
            this.TotalHearts = TotalHearts;
            this.StartingHearts = StartingHearts;
            this.BaseDamage = BaseDamage;
            this.BaseReloadTime = BaseReloadTime;
            this.DodgeChance = DodgeChance;
            this.DodgeReloadTime = DodgeReloadTime;
            this.MoveSpeed = MoveSpeed;
            this.DashDistance = DashDistance;
            this.DashReloadTime = DashReloadTime;
            this.StartingItems = StartingItems;
        }
    }
}
