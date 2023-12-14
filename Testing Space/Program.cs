using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Text;

namespace Testing_Space
{
    
    // testing for a random number generator with choosable spread
    
    class Program
    {
        readonly static Random random = new Random();
        readonly static double[] Chances = new double[] { 0.2, 0.4, 0.4 };

        static void Main()
        {
            List<int> Occurences = new List<int>();
            for (int i = 0; i < Chances.Length; i++)
                Occurences.Add(0);

            const int Count = 1000000000;

            for (int i = 0; i < Count; i++)
            {
                Occurences[GetRandomLevel() - 1]++;

                if (i % 1000000 == 0)
                {
                    StringBuilder OutText = new StringBuilder();
                    double PercentageDifferenceTotal = 0;
                    for (int j = 0; j < Chances.Length; j++)
                    {
                        int CurrentCount = i + 1;
                        int Level = j + 1;
                        int AbsoluteOccurences = Occurences[j];
                        double PercentageOccurences = (double)AbsoluteOccurences / (double)CurrentCount;

                        double ExpectedOccurences = CurrentCount * Chances[j];
                        double AbsoluteDifference = ExpectedOccurences - AbsoluteOccurences;
                        double PercentageDifference = AbsoluteDifference / ExpectedOccurences;

                        PercentageDifferenceTotal += PercentageDifference;

                        OutText.AppendLine($"Level: {Level}");
                        OutText.AppendLine($"Absolute Occurences: {AbsoluteOccurences}");
                        OutText.AppendLine($"Percentage Occurences: {PercentageOccurences}");
                        OutText.AppendLine($"Expected Occurrences: {ExpectedOccurences}");
                        OutText.AppendLine($"Absolute Difference from expected: {AbsoluteDifference}");
                        OutText.AppendLine($"Percentage Difference: {PercentageDifference}");
                        OutText.AppendLine();
                    }

                    OutText.AppendLine($"Average Percentage Difference: {PercentageDifferenceTotal / 3}");
                    Console.Clear();
                    Console.WriteLine(OutText.ToString());
                }
            }
                
            Console.ReadLine();
        }

        static int GetRandomLevel()
        {
            int Level;

            double rand = random.NextDouble();

            double Chance = 0;
            for (Level = 0; Level < Chances.Length; Level++)
            {
                Chance += Chances[Level];
                if (rand < Chance)
                    break;
            }
            Level++;

            return Level;
        }
    }
    
}