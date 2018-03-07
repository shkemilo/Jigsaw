using System;
using System.Collections.Generic;
using System.Linq;

namespace Jigsaw
{
    

    /// <summary>
    /// Singlton that is used for storing the Word List used for the Word on Word game.
    /// </summary>
    public sealed class WordList //TODO: Save whole object and just load when game starts, to prevent high cpu usage.
    {
        private static WordList instance = null;
        private static readonly object padlock = new object();

        Random rnd;

        string[] wordList;
        List<string> WoWSeeds;

        private WordList()
        {
            wordList = (Properties.Resources.WordList).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            WoWSeeds = new List<string>();

            getWoWSeeds();

            rnd = new Random(Guid.NewGuid().GetHashCode());
        }

        public static WordList Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new WordList();
                    }
                    return instance;
                }
            }
        }

        /// <summary> Finds all words with 12 length. </summary>
        private void getWoWSeeds()
        { 
            for (int i = 0; i < wordList.Length; i++)
                if (wordList[i].Length == 12)
                    WoWSeeds.Add(wordList[i].ToUpper());
        }

        /// <summary> Returns a random 12 letter word. </summary>
        public string GetWoWSeed()
        {
            return WoWSeeds.ElementAt(rnd.Next(0, WoWSeeds.Count - 1));
        }

        /// <summary> Check if the word is inside the Word list. </summary>
        public bool Check(string s)
        {
            if(s != null)
                for (int i = 0; i < wordList.Length; i++)
                    if (s.ToUpper() == wordList[i].ToUpper())
                        return true;

            return false;
        }
    }
}
