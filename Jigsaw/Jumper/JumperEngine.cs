using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Jigsaw.Jumper
{
    /// <summary>
    /// Engine for Jumper Game.
    /// </summary>
    public class JumperEngine : Engine
    {
        int numberOfFields;
        int[] combination;

        Image[] userControlImages;

        Random randomSeed;

        public JumperEngine(int numberOfFields)
        {
            this.numberOfFields = numberOfFields;

            combination = new int[numberOfFields];

            randomSeed = new Random(Guid.NewGuid().GetHashCode());

            userControlImages = new Image[] { null, Properties.Resources.Logo32, Properties.Resources.Clubs32v2, Properties.Resources.Spades, Properties.Resources.Hearts, Properties.Resources.Diamonds, Properties.Resources.Star };

            generateCombination();

            for (int i = 0; i < numberOfFields; i++)
                Console.WriteLine(combination[i]);
        }

        /// <summary> Generates a combination to be found by the user. </summary>
        private void generateCombination()
        {
            for (int i = 0; i < numberOfFields; i++)
                combination[i] = randomSeed.Next(1, 7);
        }

        /// <summary> Helper function. Adds a specifed color to a list n times. </summary>
        private void addColors(ref List<Color> list, int n, Color c)
        {
            int currentListLength = list.Count;

            for (int i = currentListLength; i < n + currentListLength; i++)
                list.Add(c);
        }

        /// <summary> Returns the generated combination. </summary>
        public int[] GetCombination()
        {
            return combination;
        }

        /// <summary> Creates a color list based on the answer the user gives. </summary>
        public Color[] CheckFeedback(int[] answer)
        {
            List<int> tempCombination = combination.ToList();
            List<int> tempAnswer = answer.ToList();

            List<Color> tempList = new List<Color>();

            int greenCount = 0;
            int yellowCount = 0;

            for (int i = 0; i < numberOfFields; i++)
                if (tempCombination[i] == answer[i])
                {
                    greenCount++;

                    tempCombination[i] = -1;
                    tempAnswer.Remove(answer[i]);
                }

            foreach (int a in tempAnswer)
                if (tempCombination.Contains(a))
                {
                    yellowCount++;

                    tempCombination[tempCombination.IndexOf(a)] = -1;
                }

            addColors(ref tempList, greenCount, Color.Green);
            addColors(ref tempList, yellowCount, Color.Yellow);
            addColors(ref tempList, numberOfFields - greenCount - yellowCount, Color.Gray);

            return tempList.ToArray();
        }

        /// <summary> Function to check if the current answer is correct. </summary>
        public bool Check(int[] answer)
        {
            bool goodAnswer = true;

            for (int i = 0; i < answer.Length; i++)
                if (combination[i] != answer[i])
                {
                    goodAnswer = false;
                    break;
                }

            return goodAnswer;
        }
    }
}
