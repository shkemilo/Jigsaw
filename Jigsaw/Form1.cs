using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework.Controls;
using MetroFramework;
using Jigsaw.LetterOnLetter;
using Jigsaw.Jumper;
using Jigsaw.Score;

namespace Jigsaw
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();

        }

        List<Game> Games;
            
        List<Control> AllControls = new List<Control>();

        void ChangeFocus(object sender, EventArgs e)
        {
            ActiveControl = label1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ActiveControl = label1;
            GetAllControls(this, AllControls);
            AllControls.Reverse();
            List<Timer> tempTimers = new List<Timer>() {TimeController, AnimationController, DrawController, EngineController };

            Finder.SetTimers(tempTimers); 
            Finder.SetAllControls(AllControls);

            ScoreInterface.Instance.GetType();

            Games = new List<Game>();
            Games.Add(new LoLGame((MetroPanel)Finder.FindElementWithTag("WoWPanel1")));
            Games.Add(new LoLGame((MetroPanel)Finder.FindElementWithTag("WoWPanel2")));
            Games.Add(new JumperGame((MetroPanel)Finder.FindElementWithTag("JumperPanel1")));
            Games.Add(new JumperGame((MetroPanel)Finder.FindElementWithTag("JumperPanel2")));

            GameManager.Instance.SetGames(Games);

            AnimationController.Start();
            DrawController.Start();

            foreach (Control c in AllControls)
                if (c is MetroTile)
                    c.Click += ChangeFocus;


            metroToggle1.Checked = true;
            metroComboBox1.SelectedIndex = 0;
        }

        void GetAllControls(Control c, List<Control> list)
        {
            foreach (Control control in c.Controls)
            {
                list.Add(control);

                if (control.GetType() == typeof(MetroPanel))
                    GetAllControls(control, list);
            }
        }

        private void AnimationController_Tick(object sender, EventArgs e)
        {
            if (GameManager.Instance.GetCurrentGame().ToAnimate() != null)
                foreach (Animateable anim in GameManager.Instance.GetCurrentGame().ToAnimate())
                    anim.Animate();
        }

        private void DrawController_Tick(object sender, EventArgs e)
        {
            ScoreInterface.Instance.DrawScoreInterface();

            if(GameManager.Instance.GetCurrentGame().ToDraw() != null)
                foreach (GUIElement g in GameManager.Instance.GetCurrentGame().ToDraw())
                    g.Show();
        }

        private void metroToggle1_CheckedChanged(object sender, EventArgs e)
        {
            if (metroToggle1.Checked)
            {
                metroStyleManager1.Theme = MetroThemeStyle.Dark;
                Theme = MetroThemeStyle.Dark;
                Refresh();
            }
            else
            { 
                metroStyleManager1.Theme = MetroThemeStyle.Light;
                Theme = MetroThemeStyle.Light;
                Refresh();
            }
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            metroStyleManager1.Style = (MetroColorStyle)metroComboBox1.SelectedIndex;
            Style = (MetroColorStyle)metroComboBox1.SelectedIndex;
            Refresh();
        }

        private void metroTile50_Click(object sender, EventArgs e)
        {

        }

        private void metroPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
