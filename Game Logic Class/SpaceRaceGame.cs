using System; // custom add by MD to test software
using System.Drawing;
using System.ComponentModel;
using Object_Classes;


namespace Game_Logic_Class
{
    public static class SpaceRaceGame
    {
        // Keeps track of which player to move in single step mode
        static public int playerTurnCounter = 0;

        // Bool to determine if ALL players out of fuel.
        static public bool allPlayersZeroFuel = false;

        // Minimum and maximum number of players.
        public const int MIN_PLAYERS = 2;
        public const int MAX_PLAYERS = 6;

        private static int numberOfPlayers;
        public static int NumberOfPlayers {
            get {
                return numberOfPlayers;
            }
            set {
                numberOfPlayers = value;
            }
        }

        public static string[] names = { "One", "Two", "Three", "Four", "Five", "Six" };  // default values

        // Only used in Part B - GUI Implementation, the colours of each player's token
        private static Brush[] playerTokenColours = new Brush[MAX_PLAYERS] { Brushes.Yellow, Brushes.Red,
                                                                       Brushes.Orange, Brushes.White,
                                                                      Brushes.Green, Brushes.DarkViolet};
        /// <summary>
        /// A BindingList is like an array which grows as elements are added to it.
        /// </summary>
        private static BindingList<Player> players = new BindingList<Player>();
        public static BindingList<Player> Players {
            get {
                return players;
            }
        }

        // The pair of die
        private static Die die1 = new Die(), die2 = new Die();


        /// <summary>
        /// Set up the conditions for this game as well as
        ///   creating the required number of players, adding each player 
        ///   to the Binding List and initialize the player's instance variables
        ///   except for playerTokenColour and playerTokenImage in Console implementation.        ///     
        /// Pre:  none
        /// Post:  required number of players have been initialsed for start of a game.
        /// </summary>
        public static void SetUpPlayers()
        {
            // Clears list each time method is call if repeated games required.
            Players.Clear();

            // Loop to initialise players
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Players.Add(new Player(SpaceRaceGame.names[i]));
                Players[i].Position = Board.START_SQUARE_NUMBER;
                Players[i].Location = Board.Squares[Board.START_SQUARE_NUMBER];
                Players[i].RocketFuel = Player.INITIAL_FUEL_AMOUNT;
                Players[i].HasPower = true;
                Players[i].AtFinish = false;
                Players[i].PlayerTokenColour = playerTokenColours[i];
            }
        }


        /// <summary>
        /// Sets complete conditon to break loop in Main
        /// </summary>
        private static bool gameComplete = false;  //default value
        public static bool GameComplete {
            get {
                return gameComplete;
            }
            set {
                gameComplete = value;
            }
        }


        /// <summary>
        ///  Plays one round of a game.
        ///  All active players take 1 step
        /// </summary>
        public static void PlayOneRound()
        {
            //Determines when one round is finished 
            if (playerTurnCounter >= NumberOfPlayers)
            {
                playerTurnCounter = 0;
            }

            //Calls play for each player
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i].Play(die1, die2);

                if (players[i].AtFinish == true)
                {
                    GameComplete = true;
                }

                playerTurnCounter++;
            }

            // checks all players for remaining fuel and modifies gameComplete bool
            CheckAllPlayerFuel();  
        }


        /// <summary>
        /// Plays game stepping through a single player at a time.
        /// Pre:  Yes radio box selected and roll dice clicked.
        /// Post:  A single players properties are updated.
        /// </summary>
        static public void PlayRoundSingleStep()
        {
            // If round is finished reset player turn counter
            if (playerTurnCounter >= NumberOfPlayers)
            {
                playerTurnCounter = 0;
            }

            // Check if all players are out of fuel.
            CheckAllPlayerFuel();

            // This loop checks to see if current player is out of fuel and steps to next active player
            while ((playerTurnCounter < NumberOfPlayers - 1) && players[playerTurnCounter].HasPower == false)
            {
                playerTurnCounter++;
            }

            // Calls play method for single active player
            players[playerTurnCounter].Play(die1, die2);

            // Sets gameComplete flag if current player reaches finish square
            if (players[playerTurnCounter].AtFinish == true)
            {
                GameComplete = true;
            }

            playerTurnCounter++;
        }

        /// <summary>
        /// Checks each player to see if they are out of fuel and sets GameComplete bool
        /// </summary>
        public static void CheckAllPlayerFuel()
        {
            int counter = 0;

            foreach (var player in Players)
            {
                if (player.HasPower == false)
                {
                    counter++;
                }
            }

            //If all players out of fuel game is complete
            if (counter == NumberOfPlayers)
            {
                GameComplete = true;
                allPlayersZeroFuel = true;
            }
        }

    }//end SpaceRaceGame
}