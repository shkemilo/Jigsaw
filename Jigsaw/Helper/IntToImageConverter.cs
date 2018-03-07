using System.Drawing;

namespace Jigsaw
{

    /// <summary>
    /// Helper class used for converting ints specifed Images.
    /// </summary>
    public sealed class IntToImageConverter
    {
        private static IntToImageConverter instance = null;
        private static readonly object padlock = new object();

        Image[] images;

        private IntToImageConverter()
        {
            images = new Image[] { null, Properties.Resources.Logo32, Properties.Resources.Clubs32v2, Properties.Resources.Spades, Properties.Resources.Hearts, Properties.Resources.Diamonds, Properties.Resources.Star };
        }

        public static IntToImageConverter Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new IntToImageConverter();
                    }
                    return instance;
                }
            }
        }

        /// <summary> Converts and array of ints to an array of Images. </summary>
        public Image[] Convert(int[] target)
        {
            Image[] temp = new Image[target.Length];

            for (int i = 0; i < target.Length; i++)
                temp[i] = images[target[i]];

            return temp;
        }

        /// <summary> Converts an int to an Image. </summary>
        public Image Convert(int target)
        {
            return images[target];
        }
    }
}
