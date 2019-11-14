using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MathGame
{
    public class Game
    {
        #region Fields
        /// <summary>
        /// This field holds a collection of random numbers.
        /// </summary>
        private int[] _randomNumbers;

        /// <summary>
        /// A field to represent game type (+, -, *, %).
        /// </summary>
        private string _gameType;

        /// <summary>
        /// This field stores the answers, depending on the numbers and game type.
        /// </summary>
        private int[] _answers;

        /// <summary>
        /// This field tracks the player's score for this game.
        /// </summary>
        private int _score;

        /// <summary>
        /// This field tracks the player's time for this game.
        /// </summary>
        private TimeSpan _time;
        #endregion

        #region Properties
        /// <summary>
        /// This property gets and sets the _randomNumbers integer array.
        /// </summary>
        public int[] RandomNumbers { get => _randomNumbers;  set => _randomNumbers = value; }

        /// <summary>
        /// This property gets and sets the _gameType string.
        /// </summary>
        public string GameType { get => _gameType; set => _gameType = value; }

        /// <summary>
        /// A property to get and set the _answers integer array.
        /// </summary>
        public int[] Answers { get => _answers; set => _answers = value; }

        /// <summary>
        /// A property to get and set the _score integer.
        /// </summary>
        public int Score { get => _score; set => _score = value; }

        /// <summary>
        /// A property to get and set the _score integer.
        /// </summary>
        public TimeSpan Time { get => _time; set => _time = value; }
        #endregion

        /// <summary>
        /// A constructor to build a game object depending on game type.
        /// </summary>
        /// <param name="gameType"></param>
        public Game(string gameType)
        {
            GameType = gameType;
            RandomNumbers = GenerateRandomNumbers(gameType);
            Answers = DetermineAnswers(_randomNumbers, gameType);
            Score = 10;
        }

        #region Methods
        /// <summary>
        /// This method generates and array of random numbers tailored to game type.
        /// </summary>
        /// <param name="gameType"></param>
        /// <returns></returns>
        private int[] GenerateRandomNumbers(string gameType)
        {
            try
            {
                int[] numbers = new int[20];
                Random random = new Random();
                if (gameType == "+" || gameType == "*")
                {
                    // Addition & Multiplication: Any Numbers will do
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        numbers[i] = random.Next(1, 11);
                    }
                }
                else if (gameType == "-")
                {
                    // Subtraction: Ensure that the second number is smaller than the first
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        int a = random.Next(1, 11);
                        int b = random.Next(1, 11);

                        if (a > b)
                        {
                            numbers[i] = a;
                            numbers[i + 1] = b;
                        }
                        else
                        {
                            numbers[i] = b;
                            numbers[i + 1] = a;
                        }
                        i++;
                    }
                }
                else
                {
                    // Division: Ensure that the numbers can be divided nicely
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        int a;
                        int b;

                        do
                        {
                            a = random.Next(1, 11);
                            b = random.Next(1, 11);
                        } while (a % b != 0);

                        numbers[i] = a;
                        numbers[i + 1] = b;
                        i++;
                    }
                }
                return numbers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }            
        }

        /// <summary>
        /// This method determins if the player's answer was correct.
        /// </summary>
        /// <param name="roundNum"></param>
        /// <param name="userAnswer"></param>
        /// <returns></returns>
        public bool IsCorrectAnswer(int roundNum, int userAnswer)
        {
            try
            {
                if (Answers[roundNum] == userAnswer)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }            
        }

        /// <summary>
        /// This method finds the correct answers for the array of random random numbers.
        /// </summary>
        /// <param name="randomNumbers"></param>
        /// <param name="gameType"></param>
        /// <returns></returns>
        private int[] DetermineAnswers(int[] randomNumbers, string gameType)
        {
            try
            {
                int[] answers = new int[10];
                for (int i = 0; i < 10; i++)
                {
                    switch (gameType)
                    {
                        case "+":
                            answers[i] = (randomNumbers[2 * i] + randomNumbers[(2 * i) + 1]);
                            break;
                        case "-":
                            answers[i] = (randomNumbers[2 * i] - randomNumbers[(2 * i) + 1]);
                            break;
                        case "*":
                            answers[i] = (randomNumbers[2 * i] * randomNumbers[(2 * i) + 1]);
                            break;
                        case "/":
                            answers[i] = (randomNumbers[2 * i] / randomNumbers[(2 * i) + 1]);
                            break;
                    }
                }
                return answers;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
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
