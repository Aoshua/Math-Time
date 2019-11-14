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
using System.Windows.Threading;

namespace MathGame
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        #region Fields
        /// <summary>
        /// This object represents our game.
        /// </summary>
        Game game;

        /// <summary>
        /// This timer allows us to refresh the time label.
        /// </summary>
        DispatcherTimer Timer;

        /// <summary>
        /// This Stopwatch allows us to track the ammount of time spent answering the 10 questions.
        /// </summary>
        System.Diagnostics.Stopwatch Watch;

        /// <summary>
        /// This attribute tracks the current time.
        /// </summary>
        string currentTime;

        /// <summary>
        /// This attribute tacks the round number.
        /// </summary>
        int roundNumber = -1;

        /// <summary>
        /// This list of chars will help us check for invalid characters.
        /// </summary>
        List<Char> invalidChars = new List<Char>() { '+', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '/', '.' };

        /// <summary>
        /// A window for the game results.
        /// </summary>
        GameResultsWindow gameResultsWindow;

        /// <summary>
        /// This object represents our player
        /// </summary>
        Player player;

        /// <summary>
        /// This is a sound for when the player gets an answer correct.
        /// </summary>
        SoundPlayer correctSound = new SoundPlayer("Sounds/CORRECT_mathematical.wav");

        /// <summary>
        /// A sound for when the player gets an answer incorrect.
        /// </summary>
        SoundPlayer wrongSound = new SoundPlayer("Sounds/WRONG_crab_apples.wav");  
        #endregion

        /// <summary>
        /// This constructor builds the game window.
        /// </summary>
        public GameWindow(string gameType, Player currentPlayer)
        {
            InitializeComponent();

            game = new Game(gameType);
            //gameResultsWindow = new GameResultsWindow();
            player = currentPlayer;

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);   //Timer goes every second
            Timer.Tick += Timer_Tick;

            Watch = new System.Diagnostics.Stopwatch();
            currentTime = "";            
        }

        #region Methods
        /// <summary>
        /// A method to increment round number and display the question each round.
        /// </summary>
        private void LoadQuestion()
        {
            txtAnswer.Text = "";
            roundNumber++;
            lblQuestionNum.Content = (roundNumber + 1).ToString() + ":";            

            lblOperator.Content = game.GameType;
            switch (roundNumber)
            {
                case 0:
                    lblFirstNum.Content = game.RandomNumbers[0];
                    lblSecondNum.Content = game.RandomNumbers[1];                    
                    break;
                case 1:
                    lblFirstNum.Content = game.RandomNumbers[2];
                    lblSecondNum.Content = game.RandomNumbers[3];
                    break;
                case 2:
                    lblFirstNum.Content = game.RandomNumbers[4];
                    lblSecondNum.Content = game.RandomNumbers[5];
                    break;
                case 3:
                    lblFirstNum.Content = game.RandomNumbers[6];
                    lblSecondNum.Content = game.RandomNumbers[7];
                    break;
                case 4:
                    lblFirstNum.Content = game.RandomNumbers[8];
                    lblSecondNum.Content = game.RandomNumbers[9];
                    break;
                case 5:
                    lblFirstNum.Content = game.RandomNumbers[10];
                    lblSecondNum.Content = game.RandomNumbers[11];
                    break;
                case 6:
                    lblFirstNum.Content = game.RandomNumbers[12];
                    lblSecondNum.Content = game.RandomNumbers[13];
                    break;
                case 7:
                    lblFirstNum.Content = game.RandomNumbers[14];
                    lblSecondNum.Content = game.RandomNumbers[15];
                    break;
                case 8:
                    lblFirstNum.Content = game.RandomNumbers[16];
                    lblSecondNum.Content = game.RandomNumbers[17];
                    break;
                case 9:
                    lblFirstNum.Content = game.RandomNumbers[18];
                    lblSecondNum.Content = game.RandomNumbers[19];
                    break;
            }
        }

        /// <summary>
        /// This method takes the player's answer and decides where to go from there.
        /// </summary>
        private void TakeUserAnswer()
        {
            try
            {
                if (ValidateInput())
                {
                    if (game.IsCorrectAnswer(roundNumber, Int16.Parse(txtAnswer.Text)))
                    {
                        correctSound.Play();
                        lblAccuracy.Content = "Was Correct!";
                        lblAccuracy.Foreground = Brushes.Green;
                    }
                    else
                    {
                        wrongSound.Play();
                        lblAccuracy.Content = "Was Wrong";
                        lblAccuracy.Foreground = Brushes.Red;
                        game.Score--;
                    }
                }

                if (roundNumber < 9)
                {
                    LoadQuestion();
                }
                else
                {
                    Watch.Stop();
                    Timer.Stop();
                    game.Time = Watch.Elapsed;
                    this.Hide();
                    gameResultsWindow = new GameResultsWindow(game.Score, game.Time, player); // Here I want to pass in the game.Score and player
                    gameResultsWindow.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }            
        }

        /// <summary>
        /// This method ensures that the user enters valid input.
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput()
        {
            try
            {
                foreach (char c in invalidChars)
                {
                    if (txtAnswer.Text.Contains(c))
                    {
                        MessageBoxResult result = MessageBox.Show("Please enter a number.", "Must Enter Number", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                if (txtAnswer.Text == "" || txtAnswer.Text.Any(c => char.IsLetter(c)))
                {
                    MessageBoxResult result = MessageBox.Show("Please enter a number.", "Must Enter Number", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }            
        }

        #region Event Handlers
        /// <summary>
        /// A method to incrememnt the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Watch.IsRunning)
                {
                    TimeSpan timeSpan = Watch.Elapsed;
                    currentTime = String.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
                    lblTimer.Content = currentTime;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }            
        }

        /// <summary>
        /// A handler to start the round of 10 questions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Start the Watch and Timer:
                Watch.Start();
                Timer.Start();

                LoadQuestion();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This handler calls TakeUserAnswer() on click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TakeUserAnswer();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            }

        /// <summary>
        /// This handler calls TakeUserAnswer() on hitting the enter key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    TakeUserAnswer();
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }            
        }

        /// <summary>
        /// This allows the window to close entirely.
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
        private void btnCancel_Click(object sender, RoutedEventArgs e)
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

        #endregion

    }
}
