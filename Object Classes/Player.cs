using System;
using System.Drawing;
using System.Diagnostics;

namespace Object_Classes {
    
    /// <summary>
    /// A player who is currently located  on a particular square 
    ///   with a certain amount of rocket fuel remaining
    /// </summary>
    public class Player {
        public const int INITIAL_FUEL_AMOUNT = 60;

        // name of the player
        private string name;
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }
        //position on board
        private int position;
        public int Position {
            get {
                return position;
            }
            set {
                position = value;
            }
        }

        // current square that player is on
        private Square location;
        public Square Location {
            get {
                return location;
            }
            set {
                location = value;
            }
        }

        // amount of rocket fuel remaining for this player
        private int fuelLeft;
        public int RocketFuel {
            get {
                return fuelLeft;
            }
            set {
                fuelLeft = value;
            }
        }

        // true if fuelLeft > 0
        // otherwise false
        private bool hasPower;
        public bool HasPower {
            get {
                return hasPower;
            }
            set {
                hasPower = value;
            }
        }

        // true if player has reached final square
        private bool atFinish;
        public bool AtFinish {
            get {
                return atFinish;
            }
            set {
                atFinish = value;
            }
        }

        private Brush playerTokenColour;
        public Brush PlayerTokenColour {
            get {
                return playerTokenColour;
            }
            set {
                playerTokenColour = value;
                playerTokenImage = new Bitmap(1, 1);
                using (Graphics g = Graphics.FromImage(PlayerTokenImage))
                {
                    g.FillRectangle(playerTokenColour, 0, 0, 1, 1);
                }
            }
        }

        private Image playerTokenImage;
        public Image PlayerTokenImage {
            get {
                return playerTokenImage;
            }
        }

        /// <summary>
        /// Parameterless constructor.
        /// Do not want the generic default constructor to be used
        /// as there is no way to set the player's name.
        /// This overwrites the compiler's generic default constructor.
        /// Pre:  none
        /// Post: ALWAYS throws an ArgumentException.
        /// </summary>
        /// <remarks>NOT TO BE USED!</remarks>
        public Player() {
            throw new ArgumentException("Parameterless constructor invalid.");
        } // end Player constructor

        /// <summary>
        /// Constructor with initialising parameters.
        /// Pre:  name to be used for this player.
        /// Post: player object has name
        /// </summary>
        /// <param name="name">Name for this player</param>
        public Player(String name)//, Square initialLocation)
        {
            Name = name;
        } // end Player constructor

        /// <summary>
        /// Rolls the two dice to determine 
        ///     the number of squares to move forward;
        ///     moves the player position on the board; 
        ///     updates the player's location to new position; and
        ///     determines the outcome of landing on this square.
        /// Pre: the dice are itialised
        /// Post: the player is moved along the board and the effect
        ///     of the location the player landed on is applied.
        /// </summary>
        /// <param name="d1">first die</param>
        /// <param name="d2">second die</param>
        public void Play(Die d1, Die d2) {
            
            int diceValue;

            // roll dice and update position only if player has fuel and not at finish
            if (HasPower && !AtFinish)
            {               
                diceValue = d1.Roll() + d2.Roll(); //roll dice
                Position = Position + diceValue; //update position

                //Determine if player has reached final square and update AtFinish boolean
                if (ReachedFinalSquare() == false)
                {
                    Location = Board.Squares[Position];
                }
                else
                {
                    //Places player on finish square if new position equal or greater than final square no.
                    Position = Board.FINISH_SQUARE_NUMBER;
                    Location = Board.Squares[Position];
                }

                //call LandOn method to update square
                Location.LandOn(this);
            }
        } // end Play.


        // <summary>
        /// Consumes specified amount of fuel.
        /// 
        /// if insufficent fuel remains, fuel set to zero
        ///  
        /// </summary>
        /// <param name="amount">amount of fuel used</param>
        public void ConsumeFuel(int amount) {
            Debug.Assert(amount > 0, "amount > 0");
            if (fuelLeft > amount) {
                fuelLeft -= amount;
            } else {
                fuelLeft = 0;
                HasPower = false;
            }
        } //end ConsumeFuel;


        /// <summary>
        ///  Checks if this player has reached the end of the game
        ///  Pre:  Dice are rolled for player and position updated
        ///  Post:  
        /// </summary>
        /// <returns>true if reached the Final Square</returns>
        private bool ReachedFinalSquare() {

            if (Position < Board.FINISH_SQUARE_NUMBER)
            {
                return false;
            }
            else
            {
                AtFinish = true;
                return true;
            }
        } //end ReachedFinalSquare

    } //end class Player
}
