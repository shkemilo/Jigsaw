using System;
using System.Windows.Forms;

namespace Jigsaw.LetterOnLetter
{
    /// <summary>
    /// Display for the user input GUI of Letter on Letter Game.
    /// </summary>
    public class LoLDisplay : GUIElement, Animateable
    {
        Control[] fields;

        int numberOfFields;
        int shownLetters;
        char[] lettersToShow;
        char[] letters;
        Random rnd;

        public LoLDisplay(Control[] fields, int numberOfFields)
        {
            this.fields = fields;
            this.numberOfFields = numberOfFields;

            lettersToShow = new char[numberOfFields];
            shownLetters = 0;

            rnd = new Random(Guid.NewGuid().GetHashCode());
        }

        /// <summary> Updates the display of the letters to be shown. </summary>
        public override void Update<T>(T message)
        {
            if (message is char[])
                letters = message as char[];
            else
                throw new Exception("This function only accepts char[]");
        }

        /// <summary> Animates a field of the main display. </summary>
        public void Animate()
        {
            if (shownLetters < numberOfFields)
                lettersToShow[shownLetters] = (char)('A' + rnd.Next(0, 26));
        }

        /// <summary> Show the letter on the current field. </summary>
        public override void Show()
        {
            if (shownLetters < numberOfFields)
                fields[shownLetters].Text = lettersToShow[shownLetters].ToString();
        }

        /// <summary> Stops animating the current letter, and a </summary>
        public void UncoverLetter()
        {
            if ((shownLetters < numberOfFields) && (letters != null))
            {
                lettersToShow[shownLetters] = letters[shownLetters];
                Show();

                shownLetters++;
            }
        }

        /// <summary> Return true if the display has shown all letters. </summary>
        public bool Finished()
        {
            if (shownLetters == numberOfFields)
                return true;
            else
                return false;
        }
    }

}
