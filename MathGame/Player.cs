using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame
{
    public class Player
    {
        #region Fields
        /// <summary>
        /// This holds the player's id.
        /// </summary>
        private int _id;

        /// <summary>
        /// This holds the player's name.
        /// </summary>
        private string _name;

        /// <summary>
        /// This holds the player's age.
        /// </summary>
        private int _age;
        #endregion

        #region Properties
        /// <summary>
        /// This property gets and sets _id.
        /// </summary>
        public int ID { get => _id; set => _id = value; }

        /// <summary>
        /// This property gets and sets _name.
        /// </summary>
        public string Name { get => _name; set => _name = value; }

        /// <summary>
        /// This property gets and sets _age.
        /// </summary>
        public int Age { get => _age; set => _age = value; }
        #endregion

        /// <summary>
        /// A constructor to build a player object with full information.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <param name="Age"></param>
        public Player(int id, string name, int age)
        {
            ID = id;
            Name = name;
            Age = age;
        }
    }
}
