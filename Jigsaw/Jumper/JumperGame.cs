using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetroFramework.Controls;
using Jigsaw.Score;

namespace Jigsaw.Jumper
{

    /// <summary>
    /// Class used for representing the Jumper Game.
    /// </summary>
    public class JumperGame : Game
    {
        JumperEngine engine;
        JumperDisplay mainDisp;

        JumperDisplayComponent correctCombinationDisplay;

        List<Control> userControls;

        Control nextRowButton;

        int numberOfRows;

        int[] answer;

        public JumperGame(MetroPanel gamePanel, int numberOfRows = 6) : base(new JumperEngine(4), gamePanel)
        {
            List<Control> jumperControls = Finder.GetAllElementsInPanel(gamePanel);
            engine = mainEngine as JumperEngine;

            userControls = Finder.FindElementsWithTag(jumperControls, "UserControls");
            foreach (Control c in userControls)
            {
                c.MouseClick += addToAnswer;
                c.Enabled = false;
            }

            this.numberOfRows = numberOfRows;

            answer = new int[] { 0, 0, 0, 0 };

            setMainDisp(jumperControls);
            setCorrectCombinationDisplay(jumperControls);

            nextRowButton = Finder.FindElementWithTag(jumperControls, "NextRowButton");
            nextRowButton.MouseClick += startGame;
            nextRowButton.Enabled = true;



            engine.Subscribe(mainDisp);
            GUIElements.Add(mainDisp);
        }

        /// <summary> Starts the game when the button is clicked. </summary>
        private void startGame(object sender, EventArgs e)
        {
            ScoreInterface.Instance.StartTimeControler();

            (sender as Control).Text = "Next Row";
            foreach (Control c in userControls)
                c.Enabled = true;


            nextRowButton.MouseClick -= startGame;
            nextRowButton.MouseClick += nextRow;
            nextRowButton.Enabled = false;
        }

        /// <summary> Sets the display of the correct combination. </summary>
        private void setCorrectCombinationDisplay(List<Control> jumperControls)
        {
            List<Control> tempElements = Finder.FindElementsWithTag(jumperControls, "CorrectCombination");
            List<JumperDisplayElement> elementHolder = new List<JumperDisplayElement>();

            foreach (Control c in tempElements)
                elementHolder.Add(new JumperDisplayElement(c));

            correctCombinationDisplay = new JumperDisplayComponent(elementHolder.ToArray());
        }

        /// <summary> Sets the main dispaly of the game. </summary>
        private void setMainDisp(List<Control> jumperControls)
        {
            List<JumperDisplayComponent> mainDispList = new List<JumperDisplayComponent>();
            List<JumperCheckerComponent> mainCheckList = new List<JumperCheckerComponent>();

            for (int i = 0; i < numberOfRows; i++)
            {
                List<Control> tempDispList = Finder.FindElementsWithTag(jumperControls, "JumperDisplay" + (i + 1).ToString());
                List<Control> tempCheckList = Finder.FindElementsWithTag(jumperControls, "JumperFeedback" + (i + 1).ToString());

                List<JumperDisplayElement> dispHolder = new List<JumperDisplayElement>();
                List<JumperCheckerElement> checkHolder = new List<JumperCheckerElement>();

                foreach (Control c in tempDispList)
                {
                    c.MouseClick += undo;
                    dispHolder.Add(new JumperDisplayElement(c));
                }

                foreach (Control c in tempCheckList)
                    checkHolder.Add(new JumperCheckerElement(c));

                mainDispList.Add(new JumperDisplayComponent(dispHolder.ToArray()));
                mainCheckList.Add(new JumperCheckerComponent(checkHolder.ToArray()));
            }

            mainDisp = new JumperDisplay(mainDispList.ToArray(), mainCheckList.ToArray());
        }

        /// <summary> Enable/Disable the field at the specified index n. </summary>
        private void setFieldAtIndexEnabled(int n, bool b)
        {
            if (mainDisp.GetCurrentRow().GetFieldAtIndex(n) != null)
                mainDisp.GetCurrentRow().GetFieldAtIndex(n).SetEnabled(b);
        }

        /// <summary> Adds a element to the answer. </summary>
        private void addToAnswer(object sender, EventArgs e)
        {
            if (mainDisp.GetCurrentRow().CurrentElement < 4)
            {
                if (mainDisp.GetCurrentRow().CurrentElement != 0)
                    setFieldAtIndexEnabled(mainDisp.GetCurrentRow().CurrentElement - 1, false);

                setFieldAtIndexEnabled(mainDisp.GetCurrentRow().CurrentElement, true);

                answer[mainDisp.GetCurrentRow().CurrentElement] = userControls.IndexOf((sender as Control)) + 1;
                engine.Broadcast(IntToImageConverter.Instance.Convert(answer[mainDisp.GetCurrentRow().CurrentElement]));


                mainDisp.GetCurrentRow().CurrentElement++;

                if (mainDisp.GetCurrentRow().CurrentElement == 4)
                    nextRowButton.Enabled = true;
            }
        }

        /// <summary> Changes to the next row. </summary>
        private void nextRow(object sender, EventArgs e)
        {
            if (mainDisp.CurrentRow < numberOfRows - 1)
            {
                setFieldAtIndexEnabled(mainDisp.GetCurrentRow().CurrentElement - 1, false);

                engine.Broadcast(engine.CheckFeedback(answer));

                if (engine.Check(answer))
                    GameOver();

                mainDisp.ManualChekerShow();
                mainDisp.CurrentRow++;

                answer = new int[] { 0, 0, 0, 0 };

                nextRowButton.Enabled = false;
            }
            else
            {
                engine.Broadcast(engine.CheckFeedback(answer));
                mainDisp.ManualChekerShow();
                setFieldAtIndexEnabled(mainDisp.GetCurrentRow().CurrentElement - 1, false);

                nextRowButton.Enabled = false;
                GameOver();
            }
        }

        /// <summary> Undo the last operation. </summary>
        private void undo(object sender, EventArgs e)
        {
            mainDisp.GetCurrentRow().CurrentElement--;
            mainDisp.GetActiveElement().SetEnabled(false);

            answer[mainDisp.GetCurrentRow().CurrentElement] = 0;
            engine.Broadcast(IntToImageConverter.Instance.Convert(answer[mainDisp.GetCurrentRow().CurrentElement]));

            if (mainDisp.GetCurrentRow().CurrentElement != 0)
                setFieldAtIndexEnabled(mainDisp.GetCurrentRow().CurrentElement - 1, true);

            nextRowButton.Enabled = false;

        }

        /// <summary> Finishes the game. </summary>
        public override void GameOver()
        {
            ScoreInterface.Instance.StopTimeControler();

            foreach (Control c in userControls)
                c.Enabled = false;

            GUIElements.Remove(mainDisp);

            for (int i = 0; i < correctCombinationDisplay.NumberOfElements; i++)
            {
                correctCombinationDisplay.Update(IntToImageConverter.Instance.Convert(engine.GetCombination()[i]));
                correctCombinationDisplay.Show();

                correctCombinationDisplay.CurrentElement++;
            }

            if (engine.Check(answer))
                Grader();

            GameManager.Instance.NextGame();
        }

        /// <summary> Gives points depending on when the combination was solved. </summary>
        public override void Grader()
        {
            ScoreInterface.Instance.ScoreEngine.ChangePoints((numberOfRows - mainDisp.CurrentRow) * 5);
        }
    }

}
