using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLight___The_attack_of_the_upholstery
{
    static class Upgrades
    {
        public const int DefaultUpgrades = 17825856;

        public static int[] WeaponUpgrades = new int[(int)WeaponType.Count];
        public static int[] CharacterUpgrades = new int[(int)CharacterType.Count];

        public static void ImportUpgrades(int EncodedUpgrades)
        {
            string StringUpgrades = IntToQuaternaryString(EncodedUpgrades);

            for (int i = 0; i < (int)WeaponType.Count; i++)
                WeaponUpgrades[i] = int.Parse(StringUpgrades[i].ToString());

            for (int i = 0; i < (int)CharacterType.Count; i++)
                CharacterUpgrades[i] = int.Parse(StringUpgrades[i + (int)WeaponType.Count].ToString());
        }

        public static int ExportUpgrades()
        {
            string StringUpgrades = "";

            foreach (int Upgrade in WeaponUpgrades)
                StringUpgrades += Upgrade.ToString();

            foreach (int Upgrade in CharacterUpgrades)
                StringUpgrades += Upgrade.ToString();

            return QuaternaryStringToInt(StringUpgrades);
        }

        static string IntToQuaternaryString(int NumToConvert)
        {
            string Quatern = "";
            string Bin = Convert.ToString(NumToConvert, 2);

            if (Bin.Length % 2 != 0)
                Bin = "0" + Bin;

            for (int i = 0; i < Bin.Length; i += 2)
                Quatern += Convert.ToInt32(Bin.Substring(i, 2), 2);

            return Quatern;
        }

        static int QuaternaryStringToInt(string NumToConvert)
        {
            string ConvertedNum = "";

            for (int i = 0; i < NumToConvert.Length; i++)
                ConvertedNum += QuaternaryDigitToBinString(NumToConvert[i]);

            return Convert.ToInt32(ConvertedNum, 2);
        }

        static string QuaternaryDigitToBinString(char Digit)
        {
            switch (Digit)
            {
                case '0':
                    return "00";
                case '1':
                    return "01";
                case '2':
                    return "10";
                case '3':
                    return "11";
                default:
                    throw new FormatException("Not a valid Quaternary Digit.");
            }
        }
    }
}
