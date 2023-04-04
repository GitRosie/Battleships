using Battleships.Models;
using System;
using System.Collections.Generic;

namespace Battleships.Logic
{
    /// <summary>
    /// Grid related methods
    /// </summary>
    public static class Grids
    {
        /// <summary>
        /// List of letters A to J and a List of Numbers 1 to 10
        /// Calls Method to add spots to grid
        /// </summary>
        /// <param name="model"></param>
        public static void InitialiseGrid(PlayerInfoModel model)
        {
            // Rows.
            List<char> letters = new List<char>
            {
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'I',
                'J'
            };

            // Columns.
            List<int> numbers = new List<int>
            {
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10
            };

            // For each letter and number in the grid add a grid spot.
            foreach (char letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddGridSpot(model, letter, number);
                }
            }

        }

        /// <summary>
        /// Adds a grid spot for each of the list items in InitialiseGrid to ShotGrid & ShipLocations grid.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="letter"></param>
        /// <param name="number"></param>
        private static void AddGridSpot(PlayerInfoModel model, char letter, int number)
        {
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.EMPTY
            };
            GridSpotModel locationspot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.EMPTY
            };

            model.ShotGrid.Add(spot);
            model.ShipLocations.Add(locationspot);
        }

        /// <summary>
        /// Displays the active player's ShipLocations grid. Shows the players ships and opponents shots (hit 'X' and miss 'O').
        /// </summary>
        /// <param name="activePlayer"></param>
        public static void DisplayShipLocationsGrid(PlayerInfoModel activePlayer)
        {
            char currentRow = activePlayer.ShipLocations[0].SpotLetter;
            string spaces;
            string ShipChars;

            foreach (var gridSpot in activePlayer.ShipLocations)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.SpotNumber == 10) { spaces = "   "; } else { spaces = "  "; }  // Add extra space for column 10
                ShipChars = " " + gridSpot.Status.ToString("G").Substring(0, 1) + spaces;   // Convert enum spot status to first letter

                switch (gridSpot.Status)    // Switch colours and 'labels' for each spot status
                {
                    case GridSpotStatus.EMPTY:
                        ShipChars = " " + gridSpot.SpotLetter + gridSpot.SpotNumber + " ";  // Print spot letter, spot number
                        Console.Write(ShipChars, Console.BackgroundColor = ConsoleColor.Blue);
                        break;
                    case GridSpotStatus.BATTLESHIP:
                        Console.Write(ShipChars, Console.BackgroundColor = ConsoleColor.Magenta);
                        break;
                    case GridSpotStatus.DESTROYER:
                        Console.Write(ShipChars, Console.BackgroundColor = ConsoleColor.DarkYellow);
                        break;
                    case GridSpotStatus.CRUISER:
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(ShipChars, Console.BackgroundColor = ConsoleColor.Yellow);
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case GridSpotStatus.HIT:
                        ShipChars = " X" + spaces;
                        Console.Write(ShipChars, Console.BackgroundColor = ConsoleColor.Red);
                        break;
                    case GridSpotStatus.MISS:
                        ShipChars = " O" + spaces;
                        Console.Write(ShipChars, Console.BackgroundColor = ConsoleColor.DarkBlue);
                        break;
                    default:
                        ShipChars = " ?" + spaces;
                        Console.Write(ShipChars, Console.BackgroundColor = ConsoleColor.Black);
                        break;
                }
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Displays the active player's ShotGrid. Shows the active player's hits and misses against opponent.
        /// </summary>
        /// <param name="activePlayer"></param>
        public static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            char currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                switch (gridSpot.Status)
                {
                    case GridSpotStatus.HIT:
                        Console.Write(" X  ", Console.BackgroundColor = ConsoleColor.Red);
                        break;
                    case GridSpotStatus.MISS:
                        Console.Write(" O  ", Console.BackgroundColor = ConsoleColor.DarkBlue);
                        break;
                    default:
                        Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ", Console.BackgroundColor = ConsoleColor.Blue);
                        break;
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();                                                        // Spacing
            Console.WriteLine();                                                        // Spacing
        }
    }
}
