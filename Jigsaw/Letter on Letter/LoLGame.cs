using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Controls;
using Jigsaw.Score;

namespace Jigsaw.LetterOnLetter
{
    /// <summary>
    /// Class used for controling a Letter on Letter Game.
    /// </summary>
    public class LoLGame : Game
    {
        LoLEngine engine;
        LoLDisplay mainDisp;

        Control submit;
        Control undo;
        Control check;

        List<Control> letterDisp;
        List<Control> usedLetters;

        Display currentWordDisplay;
        Display longestWordDisplay;

        Control checkBox;

        string answer;

        public LoLGame(MetroPanel gamePanel) : base(new LoLEngine(12), gamePanel)
        {
            List<Control> WoWControls = Finder.GetAllElementsInPanel(gamePanel);
            letterDisp = Finder.FindElementsWithTag(WoWControls, "CharacterDisplayButton");
            foreach (Control b in letterDisp)
            {
                b.MouseClick += addToAnswer;
                b.Enabled = false;
            }
            usedLetters = new List<Control>();

            currentWordDisplay = new Display(Finder.FindElementWithTag(WoWControls, "CurrentWord"));
            longestWordDisplay = new Display(Finder.FindElementWithTag(WoWControls, "LongestWord"));

            mainDisp = new LoLDisplay(letterDisp.ToArray(), letterDisp.Count);

            engine = mainEngine as LoLEngine;

            engine.Subscribe(mainDisp);

            GUIElements.Add(mainDisp); GUIElements.Add(currentWordDisplay); GUIElements.Add(longestWordDisplay);

            anims.Add(mainDisp);

            checkBox = Finder.FindElementWithTag(WoWControls, "CheckFeedback");

            check = Finder.FindElementWithTag(WoWControls, "CheckerButton");
            check.MouseClick += wordFeedback;
            submit = Finder.FindElementWithTag(WoWControls, "SSButton");
            submit.MouseClick += ssClick;
            undo = Finder.FindElementWithTag(WoWControls, "UndoButton");
            undo.MouseClick += undoLastLetter;

            answer = "";

            engine.Broadcast(engine.GetLetters());
        }

        /// <summary> Adds a letter to the answer. </summary>
        void addToAnswer(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button currentButton = (Button)sender;
                if (currentButton.Enabled == true)
                {
                    answer += currentButton.Text;
                    currentWordDisplay.Update(answer);

                    currentButton.Enabled = false;

                    usedLetters.Add(currentButton);
                    checkBox.Text = "";
                }
            }
        }

        /// <summary> Undos the last letter added to the answer. </summary>
        void undoLastLetter(object sender, EventArgs e)
        {
            if (usedLetters.Count != 0)
            {
                usedLetters.Last().Enabled = true;
                usedLetters.RemoveAt(usedLetters.Count - 1);
                answer = answer.Remove(answer.Length - 1);

                currentWordDisplay.Update(answer);
                checkBox.Text = "";
            }
        }

        /// <summary> Gives information if the current word is a valid word. </summary>
        void wordFeedback(object sender, EventArgs e)
        {
            if (mainDisp.Finished())
                if (engine.Check(answer))
                    checkBox.Text = "Ova rec postoji u recniku";
                else
                    checkBox.Text = "Ova rec ne postoji u recniku";
        }

        /// <summary> Handles the stopping and submiting. </summary>
        void ssClick(object sender, EventArgs e)
        {
            if (!mainDisp.Finished())
                mainDisp.UncoverLetter();

            if (mainDisp.Finished())
                if (submit.Text == "STOP")
                    startWordOnWord();
                else if (submit.Text == "SUBMIT")
                    GameOver();
        }

        /// <summary> Starts the game. </summary>
        void startWordOnWord()
        {
            ScoreInterface.Instance.StartTimeControler();

            submit.Text = "SUBMIT";

            foreach (Control c in letterDisp)
                c.Enabled = true;
        }

        /// <summary> Finishes the game. </summary>
        public override void GameOver()
        {
            ScoreInterface.Instance.StopTimeControler();
            checkBox.Text = "";
            check.Enabled = false;
            submit.Enabled = false;
            undo.Enabled = false;

            foreach (Control c in letterDisp)
                c.Enabled = false;

            longestWordDisplay.Update(engine.GetLongestWord());
            longestWordDisplay.Show();

            if (engine.Check(answer))
            {
                checkBox.Text = "Ova rec postoji u recniku";

                Grader();
            }
            else
            {
                checkBox.Text = "Ova rec ne postoji u recniku";
            }

            GameManager.Instance.NextGame();
        }

        /// <summary> Gives points based on the length of the word. </summary>
        public override void Grader()
        {
            score += answer.Length * 2;

            if (answer.Length == engine.GetLongestWord().Length)
                score += 6;

            ScoreInterface.Instance.ScoreEngine.ChangePoints(score);
        }
    }

}
