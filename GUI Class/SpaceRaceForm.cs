using System;
using System.Drawing;
using System.Windows.Forms;
using Game_Logic_Class;
using Object_Classes;

namespace GUI_Class
{
    public partial class SpaceRaceForm : Form
    {
        // The numbers of rows and columns on the screen.
        const int NUM_OF_ROWS = 7;
        const int NUM_OF_COLUMNS = 8;
    
        // Enum to distinguish to of player location update
        enum TypeOfGuiUpdate { AddPlayer, RemovePlayer };
 
        public SpaceRaceForm()
        {
            InitializeComponent();
            Board.SetUpBoard();
            ResizeGUIGameBoard();
            SetUpGUIGameBoard();
            SetupPlayersDataGridView();
            DetermineNumberOfPlayers();
            SpaceRaceGame.SetUpPlayers();
            PrepareToPlay();
        }


        /// <summary>
        /// Handle the Exit button being clicked.
        /// Pre:  the Exit button is clicked.
        /// Post: the game is terminated immediately
        /// </summary>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


        /// <summary>
        /// Resizes the entire form, so that the individual squares have their correct size, 
        /// as specified by SquareControl.SQUARE_SIZE.  
        /// This method allows us to set the entire form's size to approximately correct value 
        /// when using Visual Studio's Designer, rather than having to get its size correct to the last pixel.
        /// Pre:  none.
        /// Post: the board has the correct size.
        /// </summary>
        private void ResizeGUIGameBoard()
        {
            const int SQUARE_SIZE = SquareControl.SQUARE_SIZE;
            int currentHeight = tableLayoutPanel.Size.Height;
            int currentWidth = tableLayoutPanel.Size.Width;
            int desiredHeight = SQUARE_SIZE * NUM_OF_ROWS;
            int desiredWidth = SQUARE_SIZE * NUM_OF_COLUMNS;
            int increaseInHeight = desiredHeight - currentHeight;
            int increaseInWidth = desiredWidth - currentWidth;
            this.Size += new Size(increaseInWidth, increaseInHeight);
            tableLayoutPanel.Size = new Size(desiredWidth, desiredHeight);

        }// ResizeGUIGameBoard


        /// <summary>
        /// Creates a SquareControl for each square and adds it to the appropriate square of the tableLayoutPanel.
        /// Pre:  none.
        /// Post: the tableLayoutPanel contains all the SquareControl objects for displaying the board.
        /// </summary>
        private void SetUpGUIGameBoard()
        {
            for (int squareNum = Board.START_SQUARE_NUMBER; squareNum <= Board.FINISH_SQUARE_NUMBER; squareNum++)
            {
                Square square = Board.Squares[squareNum];
                SquareControl squareControl = new SquareControl(square, SpaceRaceGame.Players);
                AddControlToTableLayoutPanel(squareControl, squareNum);
            }//endfor
        }// end SetupGameBoard

        private void AddControlToTableLayoutPanel(Control control, int squareNum)
        {
            int screenRow = 0;
            int screenCol = 0;
            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            tableLayoutPanel.Controls.Add(control, screenCol, screenRow);
        }// end Add Control


        /// <summary>
        /// For a given square number, tells you the corresponding row and column number
        /// on the TableLayoutPanel.
        /// Pre:  none.
        /// Post: returns the row and column numbers, via "out" parameters.
        /// </summary>
        /// <param name="squareNumber">The input square number.</param>
        /// <param name="rowNumber">The output row number.</param>
        /// <param name="columnNumber">The output column number.</param>
        private static void MapSquareNumToScreenRowAndColumn(int squareNum, out int screenRow, out int screenCol)
        {
            int tableRow;
            int tableCol;

            // Set tableRow and tableCol values
            tableRow = squareNum / 8;
            tableCol = squareNum % 8;

            // Map even rows to screen
            if (tableRow % 2 == 0)
            {
                screenRow = 6 - tableRow;
                screenCol = tableCol;
            }
            else
            {
                // Map odd rows to screen
                screenRow = 6 - tableRow;
                screenCol = 7 - tableCol;
            }
            
        }//end MapSquareNumToScreenRowAndColumn

        //Sets up the dataGridView with player's position and fuel
        private void SetupPlayersDataGridView()
        {
            // Stop the playersDataGridView from using all Player columns.
            playersDataGridView.AutoGenerateColumns = false;
            // Tell the playersDataGridView what its real source of data is.
            playersDataGridView.DataSource = SpaceRaceGame.Players;

        }// end SetUpPlayersDataGridView



        /// <summary>
        /// Obtains the current "selected item" from the ComboBox
        ///  and
        ///  sets the NumberOfPlayers in the SpaceRaceGame class.
        ///  Pre: none
        ///  Post: NumberOfPlayers in SpaceRaceGame class has been updated
        /// </summary>
        private void DetermineNumberOfPlayers()
        {
            // Store the SelectedItem property of the ComboBox in a string
            int selectedNumber;

            // Parse string to a number
            selectedNumber = Convert.ToInt32(comboBox1.Text);

            // Set the NumberOfPlayers in the SpaceRaceGame class to that number
            SpaceRaceGame.NumberOfPlayers = selectedNumber;

        }//end DetermineNumberOfPlayers

        /// <summary>
        /// The players' tokens are placed on the Start square
        /// </summary>
        private void PrepareToPlay()
        {
            
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);

        }//end PrepareToPlay()


        /// <summary>
        /// Tells you which SquareControl object is associated with a given square number.
        /// Pre:  a valid squareNumber is specified; and
        ///       the tableLayoutPanel is properly constructed.
        /// Post: the SquareControl object associated with the square number is returned.
        /// </summary>
        /// <param name="squareNumber">The square number.</param>
        /// <returns>Returns the SquareControl object associated with the square number.</returns>
        private SquareControl SquareControlAt(int squareNum)
        {
            int screenRow;
            int screenCol;

            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            return (SquareControl)tableLayoutPanel.GetControlFromPosition(screenCol, screenRow);

        }


        /// <summary>
        /// Tells you the current square number of a given player.
        /// Pre:  a valid playerNumber is specified.
        /// Post: the square number of the player is returned.
        /// </summary>
        /// <param name="playerNumber">The player number.</param>
        /// <returns>Returns the square number of the player.</returns>
        private int GetSquareNumberOfPlayer(int playerNumber)
        {
            int squareNumber;
            squareNumber = SpaceRaceGame.Players[playerNumber].Position;

            return squareNumber;
        }//end GetSquareNumberOfPlayer


        /// <summary>
        /// When the SquareControl objects are updated (when players move to a new square),
        /// the board's TableLayoutPanel is not updated immediately.  
        /// Each time that players move, this method must be called so that the board's TableLayoutPanel 
        /// is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the board's TableLayoutPanel shows the latest information 
        ///       from the collection of SquareControl objects in the TableLayoutPanel.
        /// </summary>
        private void RefreshBoardTablePanelLayout()
        {
                  tableLayoutPanel.Invalidate(true);
        }

        /// <summary>
        /// When the Player objects are updated (location, etc),
        /// the players DataGridView is not updated immediately.  
        /// Each time that those player objects are updated, this method must be called 
        /// so that the players DataGridView is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the players DataGridView shows the latest information 
        ///       from the collection of Player objects in the SpaceRaceGame.
        /// </summary>
        private void UpdatesPlayersDataGridView()
        {
            SpaceRaceGame.Players.ResetBindings();
        }

        /// <summary>
        /// At several places in the program's code, it is necessary to update the GUI board,
        /// so that player's tokens are removed from their old squares
        /// or added to their new squares. E.g. at the end of a round of play or 
        /// when the Reset button has been clicked.
        /// 
        /// Moving all players from their old to their new squares requires this method to be called twice: 
        /// once with the parameter typeOfGuiUpdate set to RemovePlayer, and once with it set to AddPlayer.
        /// In between those two calls, the players locations must be changed. 
        /// Otherwise, you won't see any change on the screen.
        /// 
        /// Pre:  the Players objects in the SpaceRaceGame have each players' current locations
        /// Post: the GUI board is updated to match 
        /// </summary>
        private void UpdatePlayersGuiLocations(TypeOfGuiUpdate typeOfGuiUpdate)
        {           
            int squareNumber;
            SquareControl squareControl;

            for (int i = 0; i < SpaceRaceGame.NumberOfPlayers; i++)
            {
                squareNumber = GetSquareNumberOfPlayer(i);
                squareControl = SquareControlAt(squareNumber);

                if (typeOfGuiUpdate == TypeOfGuiUpdate.AddPlayer)
                {
                    squareControl.ContainsPlayers[i] = true;
                }
                else
                {
                    squareControl.ContainsPlayers[i] = false;
                }
            }

            RefreshBoardTablePanelLayout();//must be the last line in this method. Do not put inside above loop.
        } //end UpdatePlayersGuiLocations


        /// <summary>
        /// Event handler for roll button. Plays either single round or single step.
        /// Pre:  No of players and single step option selected
        /// Post:  tableLayoutPanel and dataGridView updated 
        /// </summary>
        private void rolButton_Click(object sender, EventArgs e)
        {
          
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);

            // Split functionality based on groupBox selection
            if (yesRadioButton.Checked == true)   // single step mode
            {                
                SpaceRaceGame.PlayRoundSingleStep();
            }
            else   // play complete round
            {
                SpaceRaceGame.PlayOneRound();                                             
            }

            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);
            UpdatesPlayersDataGridView();

            // Update GUI controls
            if (SpaceRaceGame.playerTurnCounter == SpaceRaceGame.NumberOfPlayers)
            {
                resetButton.Enabled = true;
                exitButton.Enabled = true;
            }
            else
            {
                resetButton.Enabled = false;
                exitButton.Enabled = false;
            }

            playersDataGridView.Enabled = false;
            comboBox1.Enabled = false;
            
            // check if users have completed the game on current round           
            if (SpaceRaceGame.GameComplete == true && SpaceRaceGame.playerTurnCounter == SpaceRaceGame.NumberOfPlayers)
            {
                rolButton.Enabled = false;
                displayWinners();
            }
        }

        /// <summary>
        /// Creates message box to display winners
        /// Pre:  GameComplete flag is true and a round completed.
        /// Post:  none
        /// </summary>
        private void displayWinners()
        {
            string winners = "The following player(s) finished the game:\n\t";

            foreach (var player in SpaceRaceGame.Players)
            {
                if (player.AtFinish == true)
                {
                    winners = winners + player.Name + "\n\t";
                }
            }

            if (SpaceRaceGame.allPlayersZeroFuel)
            {
                winners = winners + "All players ran out of fuel";
            }

            MessageBox.Show(winners);
        }


        /// <summary>
        /// Event handler for reset button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            //Update GUI controls
            resetButton.Enabled = false;
            playersDataGridView.Enabled = true;
            comboBox1.Text = "6";
            comboBox1.Enabled = true;
            groupBox1.Enabled = true;
            yesRadioButton.Checked = false;
            noRadioButton.Checked = false;
            exitButton.Enabled = true;
            rolButton.Enabled = false;

            //Restore game to original set-up
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
            SpaceRaceGame.GameComplete = false;
            SpaceRaceGame.allPlayersZeroFuel = false;
            DetermineNumberOfPlayers();
            SpaceRaceGame.SetUpPlayers();
            SetupPlayersDataGridView();
            PrepareToPlay();
            SpaceRaceGame.playerTurnCounter = 0;
        }


        /// <summary>
        /// Event handler for change of value in comboBox1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;

            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
            DetermineNumberOfPlayers();
                        
            PrepareToPlay();
        }


        /// <summary>
        /// Event handler for user selecting yes in Single Step control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void yesRadioButton_Click(object sender, EventArgs e)
        {
            // Housekeeping for form controls
            groupBox1.Enabled = false;
            rolButton.Enabled = true;
        }

     
        /// <summary>
        /// Event handler for user selecting no in Single Step control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noRadioButton_Click(object sender, EventArgs e)
        {
            //Update for form controls
            groupBox1.Enabled = false;
            rolButton.Enabled = true;
        }

    }// end class
}
