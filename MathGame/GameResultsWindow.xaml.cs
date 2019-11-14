using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MathGame
{
    /// <summary>
    /// Interaction logic for HighScoresWindow.xaml
    /// </summary>
    public partial class GameResultsWindow : Window
    {
        #region Fields
        /// <summary>
        /// A sound for when the player does a great job.
        /// </summary>
        SoundPlayer greatSound = new SoundPlayer("Sounds/WIN_That_was_tops.wav");

        /// <summary>
        /// This sound is played when the player does poorly.
        /// </summary>
        SoundPlayer badSound = new SoundPlayer("Sounds/LOSE_shmowtow_dude.wav");

        /// <summary>
        /// This sound is played when the player did alright.
        /// </summary>
        SoundPlayer notBadSound = new SoundPlayer("Sounds/MEH_yeah_I_guess.wav");
        #endregion

        /// <summary>
        /// A construtor to build our games results window.
        /// </summary>
        /// <param name="playerScore"></param>
        /// <param name="playerTime"></param>
        /// <param name="thePlayer"></param>
        public GameResultsWindow(int playerScore, TimeSpan playerTime, Player thePlayer)
        {
            InitializeComponent();

            SetUIByScore(playerScore, playerTime, thePlayer);
        }

        #region Methods
        /// <summary>
        /// This method Sets the UI depending on the scores.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="time"></param>
        /// <param name="player"></param>
        private void SetUIByScore(int score, TimeSpan time, Player player)
        {
            try
            {
                ImageBrush backgroundImage;
                if (score > 7)
                {
                    // Great!
                    lblPhrase.Content = "Well done!";
                    greatSound.Play();
                    backgroundImage = new ImageBrush(new BitmapImage(new Uri(@"Images/goodGame.jpg", UriKind.Relative)));
                    this.Background = backgroundImage;
                }
                else if (score > 4 && score < 8)
                {
                    // Okay
                    lblPhrase.Content = "You're getting the hang of this...";
                    notBadSound.Play();
                    backgroundImage = new ImageBrush(new BitmapImage(new Uri(@"Images/mehGame.png", UriKind.Relative)));
                    this.Background = backgroundImage;
                }
                else
                {
                    // Bad
                    lblPhrase.Content = "Better luck next time!";
                    badSound.Play();
                    backgroundImage = new ImageBrush(new BitmapImage(new Uri(@"Images/badGame.jpg", UriKind.Relative)));
                    this.Background = backgroundImage;
                }

                lblName.Content = player.Name;
                lblAge.Content = player.Age;
                lblCorrect.Content = score;
                lblIncorrect.Content = 10 - score;
                lblTime.Content = String.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }            
        }

        /// <summary>
        /// This event hanlder ensures that the window closes properly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.Hide();
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }            
        }

        /// <summary>
        /// This handler closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This method handles any errors that may be thrown.
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="Method"></param>
        /// <param name="Message"></param>
        private void HandleError(string Class, string Method, string Message)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show(Class + "." + Method + " -> " + Message);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
        #endregion        
    }
}
