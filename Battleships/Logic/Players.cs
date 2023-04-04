using Battleships.Models;
using System;

namespace Battleships.Logic
{
    /// <summary>
    /// Player related methods
    /// </summary>
    public static class Players
    {
        /// <summary>
        /// Creates new Player & calls methods to get necessary info and to generate necessary objects.
        /// </summary>
        /// <param name="playerTitle"></param>
        /// <returns></returns>
        public static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel modxl = new PlayerInfoModel();                         // New player

            if (playerTitle == "Computer")
            {
                modxl.UsersName = "Computer";                                          // Name is computer
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Player information for {playerTitle}");             // Specifies which user is being asked for their details

                modxl.UsersName = AskForUsersName();                                   // Ask the user for their name
            }
            Grids.InitialiseGrid(modxl);                                       // Load up the shot grid

            Ships.InitialiseShips(modxl);                                      // Load up the ships to the ship list

            //28-05-2020 AskManualOrAutoShipLocations(modxl);                                   // Allow user to choose manual or auto ship placement

            Console.Clear();                                                        // Clear screen

            return modxl;                                                          // Returns new player details (Name, ship locations and shotGrid)
        }

        /// <summary>
        /// Asks for the players name, which will be set as UsersName.
        /// Name must not be "Computer" and has to be at least 1 character long.
        /// </summary>
        /// <returns></returns>
        private static string AskForUsersName()
        
        {
            bool isValid;
            string output;
            do
            {
                Console.WriteLine("What is your name?");                                // Print to screen asking for user to input their name.
                output = Console.ReadLine();                                     // Set output as the user input.
                if (output == "Computer" || output.Length < 1)
                {
                    Console.WriteLine("Invalid choice, please enter another name...");
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }

            } while (!isValid);

            return output;                                                          // Returns what user input to CreatePlayer method.
        }

        /// <summary>
        /// Asks player if they would like to place their ships manually or automatically and then calls the corresponding method.
        /// </summary>
        /// <param name="modxl"></param>
        public static void AskManualOrAutoShipLocations(PlayerInfoModel modxl)
        {
            Console.WriteLine($"{modxl.UsersName}, would you like to place your ships manually? (Y or N)"); // Ask how they would like to place their ships
            string input = Console.ReadLine().ToUpper();                               // Read how they would like to place their ships

            //Ships LocalShip = null;
            if (input == "Y")                                                          // If like to do manually, do ManuallyPlaceShips method
            {
                Ships.ManuallyPlaceShips(modxl);
            }
            else if (input == "N")                                                     // If like to do automatically, do AutomaticallyPlaceShips method
            {
                Ships.AutomaticallyPlaceShips(modxl);
            }
            else
            {
                Console.WriteLine($"{input} is not a valid choice.");                  // Anything other than Y or N is invalid
                AskManualOrAutoShipLocations(modxl);                                    // Ask again.
            }
        }

        /// <summary>
        /// Used to see if player still active.
        /// If there are any grid spots that have a status of anything other than empty, hit or miss then they are still active.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool IsPlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.EMPTY && ship.Status != GridSpotStatus.HIT && ship.Status != GridSpotStatus.MISS)
                {
                    isActive = true;
                }
            }

            return isActive;
        }

        /// <summary>
        /// GetShotCount
        /// This is used to see how many shots it took for player to win.
        /// shotCount is the number of cells that aren't EMPTY.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotCount = 0;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.EMPTY)
                {
                    shotCount++;
                }
            }

            return shotCount;
        }
    }
}
