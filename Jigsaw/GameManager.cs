using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Jigsaw.Score;

namespace Jigsaw
{
    /// <summary>
    /// Singleton that is used for navigating through multiple Games.
    /// </summary>
    public sealed class GameManager
    {
        private static GameManager instance = null;
        private static readonly object padlock = new object();

        List<Game> games;
        int currentGame;

        Control gameChanger;

        private GameManager()
        {
            currentGame = 0;
            games = null;

            gameChanger = Finder.FindElementWithTag("GameChanger");

            gameChanger.MouseClick += StartCurrentGame;
        }

        /// <summary> Returns the Instance of the GameManager Singleton. </summary>
        public static GameManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameManager();
                    }
                    return instance;
                }
            }
        }

        /// <summary> Set of commands to be run when there are no games left. </summary>
        private void theEnd()
        {
            DialogResult dialogResult = MetroFramework.MetroMessageBox.Show(gameChanger.Parent, "Your score is: " + ScoreInterface.Instance.ScoreEngine.Score + "\n Play Again?", "JIGSAW GAME", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Restart();
            }
            else if (dialogResult == DialogResult.No)
            {
                Application.Exit();
            }
        }

        /// <summary> Sets the Games field. </summary>
        public void SetGames(List<Game> games) { this.games = games; }

        /// <summary> Returns the current game in play. </summary>
        public Game GetCurrentGame()
        {
            if(games != null)
                return games[currentGame];

            return null;
        }

        /// <summary> Starts the current game to be played. </summary>
        public void StartCurrentGame(object sender, EventArgs e)
        {
            if (games != null)
            {
                if(currentGame != 0)
                    games[currentGame - 1].DisableGame();

                //Console.WriteLine(games[currentGame].GetType());
                if (games[currentGame] != null)
                    games[currentGame].Start();

                gameChanger.Enabled = false;
            }
        }

        /// <summary> Changes to the next game. </summary>
        public void NextGame()
        {
            ScoreInterface.Instance.ResetTimeBar();

            if (currentGame != games.Count - 1)
            {
                currentGame++;
                gameChanger.Enabled = true;
            }
            else
                theEnd();
        }

    }
}
