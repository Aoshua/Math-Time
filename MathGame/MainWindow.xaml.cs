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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathGame
{   

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        /// <summary>
        /// This attribute holds all of the players since program launch.
        /// </summary>
        List<Player> PlayersList;

        /// <summary>
        /// This attribute will help us create IDs for the players.
        /// </summary>
        int playerIndexer = -1;

        /// <summary>
        /// This list of chars will help us check for invalid characters.
        /// </summary>
        List<Char> invalidChars = new List<Char>() { '+', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '/', '.' };

        /// <summary>
        /// A sound to signal the start of a game.
        /// </summary>
        SoundPlayer startSound = new SoundPlayer("Sounds/START_theme.wav");

        /// <summary>
        /// Class where game is played
        /// </summary>
        GameWindow gameWindow;
        #endregion

        /// <summary>
        /// A default constructor to build the MainWindow object.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;  // This closes application completely

            PlayersList = new List<Player>();

            startSound.Play(); 
        }

        #region Methods
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
                    if (txtName.Text.Contains(c) || txtAge.Text.Contains(c))
                    {
                        MessageBoxResult result = MessageBox.Show("Please only enter letters or numbers, respectively.", "Only Correct Characters", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                if (txtName.Text == "" || txtName.Text.Any(c => char.IsDigit(c)))
                {
                    MessageBoxResult result = MessageBox.Show("Please enter a your name (no numbers).", "Must Enter Name", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (txtAge.Text == "" || txtAge.Text.Any(c => char.IsLetter(c)) || Int16.Parse(txtAge.Text) < 3 || Int16.Parse(txtAge.Text) > 10)
                {
                    MessageBoxResult result = MessageBox.Show("Please enter a your age (no letters, and between 3 and 10).", "Must Enter Age", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (rdoAdd.IsChecked == false && rdoSubtract.IsChecked == false && rdoMultiply.IsChecked == false && rdoDivide.IsChecked == false)
                {
                    MessageBoxResult result = MessageBox.Show("Please select a game mode (add, subtract, multiply, or divide).", "Must Select Game Mode", MessageBoxButton.OK, MessageBoxImage.Error);
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

        /// <summary>
        /// This method adds player info to player list.
        /// </summary>
        private void AddPlayerInfo()
        {
            try
            {
                var name = txtName.Text;
                var age = Int16.Parse(txtAge.Text);

                // Add the player to the list, unless the player already exists
                if (!PlayersList.Exists(c => (c.Name == name) && (c.Age == age)))
                {
                    playerIndexer++;
                    Player TempPlayer = new Player(playerIndexer, name, age);
                    PlayersList.Add(TempPlayer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }            
        }

        /// <summary>
        /// This method determines if the operation is add, subtract, multiply, or divide.
        /// </summary>
        /// <returns></returns>
        private string DetermineGameType()
        {
            try
            {
                if ((bool)rdoAdd.IsChecked)
                {
                    return "+";
                }
                else if ((bool)rdoSubtract.IsChecked)
                {
                    return "-";
                }
                else if ((bool)rdoMultiply.IsChecked)
                {
                    return "*";
                }
                else
                {
                    return "/";
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
        /// This handler opens the Game window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlayGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    AddPlayerInfo();
                    this.Hide();
                    gameWindow = new GameWindow(DetermineGameType(), PlayersList[playerIndexer]); // Passing in the game type and the current player
                    gameWindow.ShowDialog();
                    this.Show();
                }
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
