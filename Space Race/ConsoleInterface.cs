using System;
using Game_Logic_Class;
using Object_Classes;


namespace Space_Race
{
    class Console_Class
    {        
        /// <summary>
        /// High level algorithm to play console based version of
        /// Space Race Game.
        /// </summary>
        static void Main(string[] args)
        {
            bool anotherGame = true;

            DisplayIntroductionMessage();            
            
            // loop to run game until user selects "N" when propted
            do
            {
                SetPlayerNumber();
                Board.SetUpBoard();
                SpaceRaceGame.SetUpPlayers();
                PlayGame();
                anotherGame = PlayAgain();
            } while (anotherGame == true);

            ExitGame();
        }//end Main


        /// <summary>
        /// Display a welcome message to the console
        /// Pre:    none.
        /// Post:   A welcome message is displayed to the console.
        /// </summary>
        static void DisplayIntroductionMessage()
        {
            Console.WriteLine("\tWelcome to Space Race.\n");
        } //end DisplayIntroductionMessage

        
        /// <summary>
        /// Displays a prompt and waits for a keypress.
        /// Pre:  none
        /// Post: a key has been pressed.
        /// </summary>
        static void ExitGame()
        {
            Console.WriteLine("\n\n\tThank you for playing Space Race.");
            Console.Write("\n\tPress Enter to terminate program ...");
            Console.ReadLine();
        } // end PressAny


        /// <summary>
        /// Prompts the user to enter a vald number of players.
        /// Pre:  none
        /// Post:  sets number of players in SpaceRaceGame class
        /// </summary>
        static void SetPlayerNumber() {
            
            string choice;
            int playerOption;
            bool okayChoice = false;

            do {
                Console.WriteLine("\tThis game is for 2 to 6 players.");
                Console.Write("\tHow many players (2-6): ");
                choice = Console.ReadLine();
                    okayChoice = int.TryParse(choice, out playerOption);
                    if (!okayChoice || playerOption < SpaceRaceGame.MIN_PLAYERS || playerOption > SpaceRaceGame.MAX_PLAYERS)
                    {
                        okayChoice = false;
                        Console.WriteLine("\nError: invalid number of players entered.\n");
                    }
                } while (!okayChoice);

            SpaceRaceGame.NumberOfPlayers = playerOption;
        } // end SetPlayerNumber
                
        
        /// <summary>
        /// Asks user if they would like to play another game.
        /// Pre:  User has just completed a full game
        /// Post:  Either resets flags and initiates another game, or begins exit procedure.
        /// </summary>
        static bool PlayAgain()
        {
            string userEntry;
            
            Console.Write("\n\n\n\n\n\nPlay Again ? (Y or N): ");
            userEntry = Console.ReadLine();
            if (userEntry == "Y" || userEntry == "y")
            {
                SpaceRaceGame.GameComplete = false;
                SpaceRaceGame.allPlayersZeroFuel = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        
        /// <summary>
        /// Loops over the PlayOneRound method until game is complete.
        /// Calls DisplayResults method to print winners and final positions
        /// Pre:  board and players have been initialised.
        /// Post:  leads in to play again option.
        /// </summary>
        public static void PlayGame()
        {
            int roundCounter = 0;  // tracks how many individual round have been played.

            while (SpaceRaceGame.GameComplete == false)
            {
                Console.WriteLine("\n\nPress Enter to play a round ...\n");
                Console.ReadLine();

                if (roundCounter == 0) {
                    Console.WriteLine("\tFirst Round\n");
                }
                else {
                    Console.WriteLine("\tNext Round\n");
                }
                
                SpaceRaceGame.PlayOneRound();
                roundCounter = roundCounter + 1;
                DisplayPositionAndFuel();
            }
            DisplayResults();
        }// end PlayGame


        /// <summary>
        /// Displays the current position and remaining fuel for all players
        /// at the end of each round.
        /// Pre:  Players have completed a round
        /// </summary>
        public static void DisplayPositionAndFuel()
        {
            // loops through all players and provides console feedback on current location and fuel
            for (int i = 0; i < SpaceRaceGame.NumberOfPlayers; i++)
            {
                Console.WriteLine("\t{0} on square {1} with {2} yottawatt of power remaining",
                    SpaceRaceGame.Players[i].Name, SpaceRaceGame.Players[i].Position, SpaceRaceGame.Players[i].RocketFuel);
            }
        }


        /// <summary>
        /// Outputs the winner and finishing stats to console
        /// </summary>
        public static void DisplayResults()
        {
            // Announces the winners of the game.
            Console.WriteLine("\n\n\tThe following player(s) finished the game\n");
            for (int i = 0; i < SpaceRaceGame.NumberOfPlayers; i++)
            {
                if (SpaceRaceGame.Players[i].AtFinish == true)
                {
                    Console.WriteLine("\t\t{0}", SpaceRaceGame.Players[i].Name);
                }
            }

            // Notification if all players ran out of fuel
            if (SpaceRaceGame.allPlayersZeroFuel)
            {
                Console.WriteLine("\n\n\tAll players ran out of fuel.");
            }

            // Final position and fuel for all players
            Console.WriteLine("\n\n\tIndividual players finished at the locations specified.\n");
            for (int j = 0; j < SpaceRaceGame.NumberOfPlayers; j++)
            {
                Console.WriteLine("\t\t{0} with {1} yottawatts of power at square {2}\n", SpaceRaceGame.Players[j].Name,
                    SpaceRaceGame.Players[j].RocketFuel, SpaceRaceGame.Players[j].Position);
            }
            Console.WriteLine("\nPress Enter to contine ...");
            Console.ReadLine();
        }

    }//end Console class
}