using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    struct Run
    {
        public int RunID;
        public int SaveGameID;
        public int RunNo;
        public TimeSpan RunTime;
        public int NoBossCoinsCollected;
        public int Score;
        public bool RunWon;

        public Run(int RunID, int SaveGameID, int RunNo, TimeSpan RunTime, int NoBossCoinsCollected, int Score, bool RunWon)
        {
            this.RunID = RunID;
            this.SaveGameID = SaveGameID;
            this.RunNo = RunNo;
            this.RunTime = RunTime;
            this.NoBossCoinsCollected = NoBossCoinsCollected;
            this.Score = Score;
            this.RunWon = RunWon;
        }
    }
}
