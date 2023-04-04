using Battleships.Models;
using System;
using System.Linq;
using System.Media;

namespace Battleships.Logic
{
    /// <summary>
    /// Methods for running the game, game flow and user options.
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// Method that runs the game by calling the other methods in order to flow.
        /// </summary>
        public static void RunGame()
        {
            SoundPlayer backmusic = new SoundPlayer
            {
                SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\gamemusic.wav"
            };
            backmusic.Play();

            Console.Title = "Battleships";
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            WelcomeMessage();

            PlayerInfoModel activePlayer;
            PlayerInfoModel opponent;

            // if it is a one player game do this.
            if (ChooseNoOfPlayers())
            {
                activePlayer = Players.CreatePlayer("Player 1");                // Create new player - Player 1
                Players.AskManualOrAutoShipLocations(activePlayer);             // Allow user to choose manual or auto ship placement
                opponent = Players.CreatePlayer("Computer");                    // Create new player - Player 2
                Ships.AutomaticallyPlaceShips(opponent);                        // Computer ships placed automatically
            }
            // else it is a two player game and this is done.
            else
            {
                activePlayer = Players.CreatePlayer("Player 1");                // Create new player - Player 1
                Players.AskManualOrAutoShipLocations(activePlayer);             // Allow Player 1 to choose manual or auto ship placement
                opponent = Players.CreatePlayer("Player 2");                    // Create new player called Player 2
                Players.AskManualOrAutoShipLocations(opponent);                 // Allow Player 2 to choose manual or auto ship placement
            }

            PlayerInfoModel winner = null;                                      // Set no winner at start

            do
            {
                RecordPlayerShot(activePlayer, opponent);                       // Change coordinate to hit (X) or miss (O)

                bool doesGameContinue = Players.IsPlayerStillActive(opponent);  // if opponent still has ships, doesGameContinue is true

                if (doesGameContinue)                                           // If game is to continue do the following
                {
                    //Swap player using temp variable
                    PlayerInfoModel tempHolder = opponent;                      // Set tempHolder as opponent
                    opponent = activePlayer;                                    // make the opponent the active player
                    activePlayer = tempHolder;                                  // make the active player the opponent (opponent stored in tempHolder)

                    // (activePlayer, opponent) = (opponent, activePlayer);     // Swap player using Tuple (cuurently not in use, would replace the above temp variable method)
                }
                else
                {
                    winner = activePlayer;                                          // If game does not continue then active player is winner
                }
            } while (winner == null);                                               // repeat the do while there is no winner

            IdentifyWinner(winner);                                                 // Prints details of who won to screen and no. of shots it took to win

            Console.ReadLine();                                                     // End of program

            // Could add 'Play Again?' feature here.
        }

        /// <summary>
        /// Method to print a welcome message to console.
        /// </summary>
        public static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleships!");
            Console.WriteLine("Created by Rosie Ledbrook");
            Console.WriteLine("Music: https://www.bensound.com");
            Console.WriteLine();
        }

        /// <summary>
        /// Method which allows user to choose a 1 or 2 player game.
        /// Returns a bool, true = 1 player and false = 2 player.
        /// </summary>
        /// <returns></returns>
        public static bool ChooseNoOfPlayers()
        {
            Console.WriteLine("Press '1' for a single player game or '2' for a two player game");
            string input = Console.ReadLine();      // set input as user input
            bool output = true;                     // defaut bool of true (1 Player)
            if (input == "1")
            {
                output = true;                      // if input is 1 output is true
            }
            else if (input == "2")
            {
                output = false;                     // if input is 2 output is false
            }
            else
            {
                Console.WriteLine($"{input} is not a valid choice.");
                ChooseNoOfPlayers();                // if input is not "1" or "2", ask again
            }

            return output;                          // return true or false
        }

        /// <summary>
        /// Called each time a player takes a shot
        /// Calls methods required to take shot coordinate, validate the coordinates, make the shot and display the shot.
        /// </summary>
        /// <param name="activePlayer"></param>
        /// <param name="opponent"></param>
        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot;
            char row = 'A';     // default value
            int column = 0;     // default value
            string shot;

            Console.Clear();

            // Display the active player's ship locations
            Console.WriteLine("Your Fleet");
            Grids.DisplayShipLocationsGrid(activePlayer);

            // DEBUG - Display the opponent's ship locations
            //Console.WriteLine("Opponent ships");            //DEBUG
            //Grids.DisplayShipLocationsGrid(opponent);       //DEBUG

            // Display the active player's shot grid (shots at opponent ships)
            Console.WriteLine("Your Opponent's Fleet");    
            Grids.DisplayShotGrid(activePlayer);
            
            // DEBUG - Display the opponent's shot grid
            //Console.WriteLine("Opponent shotgrid");         //DEBUG
            //Grids.DisplayShotGrid(opponent);                //DEBUG

            do
            {
                shot = AskForCoordinate(activePlayer, "for your shot (e.g. A1)");  // sets shot as the coordinates

                try
                {
                    (row, column) = SplitIntoRowAndColumn(shot);                    // Sets Row and column as the reults of splitting the shot string in two.
                    isValidShot = ValidateShot(activePlayer, row, column);          // Validates the shot.
                }
                catch (Exception)
                {
                    isValidShot = false;                                            // Catches invalid shot
                }

                if (activePlayer.UsersName != "Computer")                           // If it's not the computer's turn
                {
                    if (!isValidShot)
                    {
                        Console.WriteLine("Invalid shot coordinates, please try again.");   // If invalid shot, throw error message
                    }
                }
            } while (isValidShot == false);                                                 // Repeat while shot is invalid

            bool isAHit = IdentifyShotResult(opponent, row, column);            // True if shot hit a ship or false if shot hit an empty spot

            MarkShotResult(activePlayer, opponent, row, column, isAHit);        // Changes spot status to hit or miss

            DisplayShotResults(activePlayer, opponent, row, column, isAHit);    // Display results of shot
        }

        /// <summary>
        /// Asks player for coordinates for shot or calls method for computer to make coordinates.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="AddText"></param>
        /// <returns></returns>
        public static string AskForCoordinate(PlayerInfoModel player, string AddText)
        {
            if (player.UsersName == "Computer")
            {
                string output = ComputerTakeShot(player);   // Method which randomly picks coordinate
                return output;                              // Returns the random coordinates
            }
            else
            {
                Console.WriteLine($"{player.UsersName}, please enter the coordinates " + AddText + ":");    // Asks for shot coordinates
                string output = Console.ReadLine().ToUpper();                                               // Read user input
                return output;                                                                              // Returns users coordinates
            }
        }

        /// <summary>
        /// Splits the coordinate string in to row and column.
        /// E.g. A1 to A and 1.
        /// </summary>
        /// <param name="shot"></param>
        /// <returns></returns>
        public static (char row, int column) SplitIntoRowAndColumn(string shot)
        {
            char row = 'A'; // default value
            int column = 0; // default value

            try
            {
                char[] shotArray = shot.ToArray();                  // Splits each character in the string in to an array. E.g. A10 would be 'A','1','0'

                if (shot.Length == 2)                               // If length of string 'shot' is 2
                {
                    row = shotArray[0];                             // row is equal to the first value in the array
                    column = int.Parse(shotArray[1].ToString());    // column is equal to second value in the array
                }
                if (shot.Length == 3)                               // If length of string 'shot' is 3
                {
                    if (shotArray[1] == '1' && shotArray[2] == '0') // If second value in array is 1 and third value is 0 then...
                    {
                        row = shotArray[0];                         // row is first value in array
                        column = 10;                                // column is 10
                    }
                    else
                    {
                        Console.WriteLine("Invalid shot type");     // Else invalid shot
                    }
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid shot type", "shot", e); // Catch invalid string
            }

            return (row, column);   // return row and column
        }

        /// <summary>
        /// If spot hasn't already been shot at, it is valid shot.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool ValidateShot(PlayerInfoModel activeplayer, char row, int column)
        {
            bool isValidShot = false;   // invalid shot as default

            foreach (var gridSpot in activeplayer.ShotGrid)   // For each grid spot in the active player's shot grid
            {
                if (gridSpot.SpotLetter == row && gridSpot.SpotNumber == column) // if the spot's letter is the input row AND the spot's number is the input column
                {
                    if (gridSpot.Status != GridSpotStatus.HIT && gridSpot.Status != GridSpotStatus.MISS)
                    {
                        isValidShot = true; // If the spot's status is not already Hit or Missed it is a valid shot
                    }
                }
            }

            return isValidShot;
        }

        /// <summary>
        /// Changes the spot status in opponents Ship Locations to hit.
        /// </summary>
        /// <param name="opponent"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IdentifyShotResult(PlayerInfoModel opponent, char row, int column)
        {
            bool isAHit = false;    // Default value

            foreach (var spot in opponent.ShipLocations)    // For each spot in opponent's Ship Locations grid
            {
                if (spot.SpotLetter == row && spot.SpotNumber == column)    // If the spot's letter = row of shot and the spot's number = column of shot
                {
                    if (spot.Status == GridSpotStatus.BATTLESHIP || spot.Status == GridSpotStatus.CRUISER || spot.Status == GridSpotStatus.DESTROYER)
                    {
                        isAHit = true;                      // If the spot status is Battleship, Cruiser or Destroyer it is a hit
                        spot.Status = GridSpotStatus.HIT;   // Change spot status from ship type to hit, done in MarkShotResult
                    }
                    else
                    {
                        spot.Status = GridSpotStatus.MISS; // If the spot is empty, change the spot status from empty to miss
                    }
                }
            }
            return isAHit;  // Returns true (ship hit by shot) or false (shot missed)
        }

        /// <summary>
        /// Changes the spot status in active players shot grid.
        /// </summary>
        /// <param name="activePlayer"></param>
        /// <param name="opponent"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="isAHit"></param>
        public static void MarkShotResult(PlayerInfoModel activePlayer, PlayerInfoModel opponent, char row, int column, bool isAHit)
        {

            foreach (var spot in activePlayer.ShotGrid)     // For each spot in the active player's shot grid
            {
                if (spot.SpotLetter == row && spot.SpotNumber == column)    // if the spot's letter is shot row and the spot's number is the shot column
                {
                    if (isAHit)
                    {
                        spot.Status = GridSpotStatus.HIT;   // If it was a hit, change spot status on shot grid to hit
                    }
                    else
                    {
                        spot.Status = GridSpotStatus.MISS;  // If miss, change spot status on shot grid to miss
                    }
                }
            }
        }

        /// <summary>
        /// Displays shot result to player on grid and "..is a hit" or "...is a miss".
        /// </summary>
        /// <param name="activePlayer"></param>
        /// <param name="opponent"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="isAHit"></param>
        private static void DisplayShotResults(PlayerInfoModel activePlayer, PlayerInfoModel opponent, char row, int column, bool isAHit)
        {
            Console.Clear();

            // If not a computer, display your ShipLocations grid
            if (activePlayer.UsersName != "Computer")
            {
                // Display active player's ship locations
                Console.WriteLine("Your Fleet");
                Grids.DisplayShipLocationsGrid(activePlayer);
                // Display the active player's shot grid
                Console.WriteLine("Your Opponent's Fleet");
                Grids.DisplayShotGrid(activePlayer);
            }
            else
            {
                // Display human player's fleet
                Console.WriteLine("Your Fleet");
                Grids.DisplayShipLocationsGrid(opponent);
                // Display the Computer's shot grid
                Console.WriteLine("Computer's shots");
                Grids.DisplayShotGrid(activePlayer);
            }


            // Print results of the shot to the console
            if (isAHit)
            {
                Console.WriteLine($"{activePlayer.UsersName}'s shot, {row}{column}, is a hit!");
            }
            else
            {
                Console.WriteLine($"{activePlayer.UsersName}'s shot {row}{column} is a miss.");
            }

            if (opponent.UsersName != "Computer")
            {
                Console.WriteLine($"Now it's {opponent.UsersName}'s turn, press any key and pass to your opponent...");  // Tell player it's other users go now. Not needed if opponent is computer.
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine($"{opponent.UsersName}, press any key to begin your turn...");
            }
            else
            { Console.WriteLine("Press any key to continue..."); }

            Console.ReadKey();  // Any key press will clear screen for next player turn
            Console.Clear();
        }

        /// <summary>
        /// Generates random coordinate string (A-J and 1-10)
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private static string ComputerTakeShot(PlayerInfoModel player)
        {
            Random rand = new Random();                             // New random
            char randXLocation = (char)rand.Next('A', 'J');         // Random char between a-j, x coordinate
            int randYLocation = rand.Next(1, 10);                   // Random int between 1-10, y coordonate

            string XLocation = randXLocation.ToString();            // Convert char to string
            string YLocation = randYLocation.ToString();            // Convert int to string

            string output = XLocation + YLocation;                  // Concatenate string to make coordinates

            return output;                                          // Returns coordinates
        }

        /// <summary>
        /// Adds up number of spots on shot grid that aren't empty to get winners shot count.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotCount = 0;  // Start at 0
            foreach (var spot in player.ShotGrid)   // For each spot in player's shot grid...
            {
                if (spot.Status != GridSpotStatus.EMPTY)
                {
                    shotCount++;    // If not empty, add 1 to shot count
                }
            }

            return shotCount;   // return shot count
        }

        /// <summary>
        /// Displays the winner to players
        /// </summary>
        /// <param name="winner"></param>
        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations {winner.UsersName}, you are the winner!");
            Console.WriteLine($"{winner.UsersName} took {GetShotCount(winner)} shots to win.");
        }
    }
}
