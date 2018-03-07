using System;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace Jigsaw.Score
{
    /// <summary>
    /// Singleton that is used for combining the Score Engine with its GUI.
    /// </summary>
    public sealed class ScoreInterface
    {
        private static ScoreInterface instance = null;
        private static readonly object padlock = new object();

        ScoreEngine scoreEngine;
        public ScoreEngine ScoreEngine { get => scoreEngine; }

        Display scoreDisplay;
        
        MetroProgressBar progressBar;

        Timer timeControler;

        private ScoreInterface(Timer timeControler)
        {
            scoreEngine = new ScoreEngine();
            scoreDisplay = new Display((MetroTextBox)Finder.FindElementWithTag("ScoreDisplay"));
            progressBar = (MetroProgressBar)Finder.FindElementWithTag("TimeBar");

            scoreEngine.Subscribe(scoreDisplay);

            scoreEngine.Broadcast(scoreEngine.GetScore());

            this.timeControler = timeControler;
            this.timeControler.Tick += timeControlerTick;
        }

        public static ScoreInterface Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ScoreInterface(Finder.FindTimerWithTag("Time"));
                    }
                    return instance;
                }
            }
        }

        /// <summary> When time is up stop the current game. </summary>
        void stopCurrentGame()
        {
            GameManager.Instance.GetCurrentGame().GameOver();
        }

        /// <summary> Tick event used for simulating time, and animating the progress bar. </summary>
        void timeControlerTick(object sender, EventArgs e)
        {
            if (progressBar.Value != progressBar.Maximum)
                progressBar.PerformStep();
            else
            {
                stopCurrentGame();
                timeControler.Stop();
            }
        }

        /// <summary> Draws the score interface GUI elements. </summary>
        public void DrawScoreInterface()
        {
            scoreDisplay.Show();
        }

        /// <summary> Startstime. </summary>
        public void StartTimeControler()
        {
            timeControler.Start();
        }

        /// <summary> Stops time. </summary>
        public void StopTimeControler()
        {
            timeControler.Stop();
        }

        /// <summary> Resets the time bar to 0. </summary>
        public void ResetTimeBar()
        {
            progressBar.Value = 0;
        }

    }
}
