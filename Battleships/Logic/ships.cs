using Battleships.Models;
using System;
using System.Collections.Generic;

namespace Battleships.Logic
{
    /// <summary>
    /// Ship related methods
    /// </summary>
    public class Ships
    {
        /// <summary>
        /// List of ships, List of how many of each ship & List of their lengths
        /// This then calls method to add each ship to PlayersShips
        /// </summary>
        /// <param name="model"></param>
        public static void InitialiseShips(PlayerInfoModel model)
        {
            // ShipNames.
            List<string> ships = new List<string>
            {
                "Cruiser"
                ,"Destroyer"
                ,"Battleship"
            };
            // How many of each ship
            List<int> shipnumbers = new List<int>
            {
                3
                ,2
                ,1
            };
            // Length of each ship
            List<int> shiplengths = new List<int>
            {
                3
                ,4
                ,5
            };

            int type = 0;
            foreach (string shiptype in ships) //for all the types of ship ...
            {
                for (int i = 0; i < shipnumbers[type]; i++) //for the number of ships of this type
                {
                    AddFleet(model, shiptype, shipnumbers[i], shiplengths[type]); // Pass data from list in to Add Fleet method
                }
                type++; // Move on to next ship type
            }
        }


        /// <summary>
        /// Adds each ship in InitialiseShips to PlayersShips
        /// </summary>
        /// <param name="model"></param>
        /// <param name="shipname"></param>
        /// <param name="shipnumbers"></param>
        /// <param name="shiplengths"></param>
        private static void AddFleet(PlayerInfoModel model, string shipname, int shipnumbers, int shiplengths)
        {
            ShipModel ships = new ShipModel                 // new ship
            {
                ShipNumber = Convert.ToInt32(shipnumbers),  // Ship number = number from list.
                ShipName = shipname,                        // Ship name = name from list.
                ShipLength = Convert.ToInt32(shiplengths),  // Ship length = length from list.
                //ShipHits = 0,
                ShipIsHorizontal = false,                   // Ship is not horizontal by default.
                //ShipIsSunk = false
            };
            model.PlayersShips.Add(ships);                  // Adds the ship to the list of PlayersShips
        }


        /// <summary>
        /// Method for manual ship placement.
        /// Calls methods that ask for ship orientation and coordinates and calls validation method which, if valid, places the ship.
        /// </summary>
        /// <param name="activePlayer"></param>
        public static void ManuallyPlaceShips(PlayerInfoModel activePlayer)
        {
            string sText;
            char row;
            int column;
            bool bValid;

            foreach (ShipModel ships in activePlayer.PlayersShips)  // for each of the ships in PlayersShips
            {
                string stext = $"{ships.ShipName}"; // variable that passes in the ship name

                Console.Clear();

                // Display the active player's ShipLocations grid
                Grids.DisplayShipLocationsGrid(activePlayer);

                do
                {
                    bool bHV = AskHorizontalorVertical(activePlayer, stext); // bHV true if horizontal, false if vertical
                    ships.ShipIsHorizontal = bHV;
                    if (bHV) { sText = " (leftmost square)"; } else { sText = " (top square)"; } // Set sText depending on horizontal or vertical

                    string location = Game.AskForCoordinate(activePlayer, $"for your {ships.ShipName} {sText}");    // Ask for coordinates for starting point of ship placement

                    (row, column) = Game.SplitIntoRowAndColumn(location); // split the input in to row and column

                    bValid = ValidateLocation(activePlayer, row, column, ships.ShipIsHorizontal, ships.ShipLength, ships.ShipName); // if valid location, bValid = true
                    if (!bValid)
                    {
                        Console.Clear();
                        Grids.DisplayShipLocationsGrid(activePlayer);
                        Console.WriteLine($"{row}{column} is an invalid location, please try again");   // If not valid location, say so
                    }
                } while (!bValid); // Repeat until valid location chosen


                //BODGEPlaceShip(activePlayer, row, column, ships.ShipIsHorizontal, ships.ShipLength, ships.ShipName);
                Console.Clear();
                //Grids.DisplayShipLocationsGrid(activePlayer);
                Console.WriteLine($"Successfully placed your {ships.ShipName}!");
            }
            Console.WriteLine($"Successfully placed your all Your ships!");
            Console.ReadLine().ToUpper();
        }


        /// <summary>
        /// Changes the status of the grid spot from empty to the ship type for each spot the ship fills.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="ShipIsHorizontal"></param>
        /// <param name="ShipLength"></param>
        /// <param name="ShipName"></param>
        public static void PlaceShip(PlayerInfoModel activeplayer, char row, int column, bool ShipIsHorizontal, int ShipLength, string ShipName)
        {
            int countlength = 0;    // Start count at 0

            foreach (var spot in activeplayer.ShipLocations)   // For each spot in the active player's ShipLocations grid
            {
                if (ShipIsHorizontal)   // If the ship is being placed horizontally...
                {
                    if (spot.SpotLetter == row && spot.SpotNumber == column && countlength < ShipLength) // If the spot's letter = the input coordinate row and the spot's number = the input coordinate column & count is less than the ship length
                    {
                        if (ShipName == "Battleship")                   // If the ship's name is Battleship...
                        {
                            spot.Status = GridSpotStatus.BATTLESHIP;    // Set the ShipLocation spot status to Battleship.
                        }
                        else if (ShipName == "Destroyer")               // If the ship's name is Destroyer...
                        {
                            spot.Status = GridSpotStatus.DESTROYER;     // Set the ShipLocation spot status to Destroyer.
                        }
                        else if (ShipName == "Cruiser")                 // If the ship's name is Cruiser...
                        {
                            spot.Status = GridSpotStatus.CRUISER;       // Set the ShipLocation spot status to Cruiser.
                        }
                        column++;   // Add 1 to column
                        countlength++;  // Add 1 to countlength
                    }
                }
                else // If the ship is being placed vertically...
                {
                    if (spot.SpotLetter == row && spot.SpotNumber == column && countlength < ShipLength)    // If the spot's letter = the input coordinate row and the spot's number = the input coordinate column & count is less than the ship length
                    {
                        if (ShipName == "Battleship")                   // If the ship's name is Battleship...
                        {
                            spot.Status = GridSpotStatus.BATTLESHIP;    // Set the ShipLocation spot status to Battleship.
                        }
                        else if (ShipName == "Destroyer")               // If the ship's name is Destroyer...
                        {
                            spot.Status = GridSpotStatus.DESTROYER;     // Set the ShipLocation spot status to Destroyer.
                        }
                        else if (ShipName == "Cruiser")                 // If the ship's name is Cruiser...
                        {
                            spot.Status = GridSpotStatus.CRUISER;       // Set the ShipLocation spot status to Cruiser.
                        }
                        row++;  // Add 1 to the row
                        countlength++; // Add 1 to the countlength
                    }
                }
            }
        }


        /// <summary>
        /// Method for automatic ship placement
        /// Generates random coordinates and random ship orientation. Calls validation method which, if valid, places the ship.
        /// </summary>
        /// <param name="activePlayer"></param>
        public static void AutomaticallyPlaceShips(PlayerInfoModel activePlayer)
        {
            Random rand = new Random();
            bool bValid;
            char row;
            int column;

            foreach (ShipModel ships in activePlayer.PlayersShips)  // For each of the ships in the active player's list of ships...
            {

                do
                {
                    row = (char)rand.Next('A', 'K');    // Auto pick row between A and J.
                    column = rand.Next(1, 11);          // Auto pick column between 1 and 10
                    int HorV = rand.Next(0, 2);         // Auto pick value of 0 or 1 to determine ship orientation

                    if (HorV == 0)
                    {
                        ships.ShipIsHorizontal = true;  // If HorV is 0, ship is horizontal
                    }
                    else
                    {
                        ships.ShipIsHorizontal = false; // If HorV is 1, ship is vertical (not hoizontal)
                    }

                    bValid = ValidateLocation(activePlayer, row, column, ships.ShipIsHorizontal, ships.ShipLength,ships.ShipName);   // if valid location, bValid = true

                } while (!bValid);  // Repeat while ship location is not valid

                Console.Clear();

                // DEBUG - Display the active player's Ship Locations grid.
                //Grids.DisplayShipLocationsGrid(activePlayer);

            }
            if (activePlayer.UsersName != "Computer")
            {
                Grids.DisplayShipLocationsGrid(activePlayer); // If not computer, show your final ship placements
                Console.WriteLine($"Successfully placed your all Your ships! Enter to continue.");  // Tell player that all ships have been placed
                Console.ReadLine().ToUpper();
            }

        }


        /// <summary>
        /// Asks player for ship orientation and converts input to a bool isHorizontal.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="AddText"></param>
        /// <returns></returns>
        public static bool AskHorizontalorVertical(PlayerInfoModel model, string AddText)
        {
            Console.WriteLine($"{model.UsersName}, would you like to place your " + AddText + " horizontally (H) or vertically (V)?"); // AddText is the ship name
            string horizontalOrVertical = Console.ReadLine().ToUpper(); // Set horizontalOrVertical to user input (converted to upper case).
            bool isHorizontal = true;   // Default value

            if (horizontalOrVertical == "H")
            {
                isHorizontal = true;    // If user input is H, isHorizontal is true
            }
            else if (horizontalOrVertical == "V")
            {
                isHorizontal = false;    // If user input is V, isHorizontal is false
            }
            else
            {
                Console.WriteLine("Invalid choice, please try again.");
                AskHorizontalorVertical(model, AddText);    // If invalid choice, ask again.
            }

            return isHorizontal;    // Return true or false
        }


        /// <summary>
        /// Checks that each spot of the ship and the surrounding spots are empty. If all empty, the spot is valid and a ship is placed.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="ShipIsHorizontal"></param>
        /// <param name="ShipLength"></param>
        /// <param name="ShipName"></param>
        /// <returns></returns>
        private static bool ValidateLocation(PlayerInfoModel activeplayer, char row, int column, bool ShipIsHorizontal, int ShipLength,string ShipName)
        {
            bool isValidLocation = true;    // valid until detected as invalid
            int rowNum = Convert.ToInt32(row) - 64; //Ascii to integer kludge
            int colNum = column;    // copy of column value to ensure the input column value is not altered

            // variables used to check the ship location and it's surrounding spots are empty, so ships can't touch.
            int startrow;
            int startcol;
            int maxrow;
            int maxcol;

            //***Check ship will fit in grid at specified start coordinate.***
            // If ship is horizontal check accross
            if (ShipIsHorizontal)
            {
                if (rowNum > 1) { startrow = rowNum - 1; } else { startrow = 1; }
                if (colNum > 1) { startcol = colNum - 1; } else { startcol = 1; }
                maxrow = rowNum + 1;
                maxcol = column + ShipLength;
            }
            // Else check downwards
            else
            {
                if (rowNum > 1) { startrow = rowNum - 1; } else { startrow = 1; }
                if (column > 1) { startcol = colNum - 1; } else { startcol = 1; }
                maxrow = rowNum + ShipLength;
                maxcol = column + 1;
            }
            // If ship placed on outer edge of grid
            if (startrow == 0) { startrow = 1; }
            if (startcol == 0) { startcol = 1; }
            if (maxrow == 11) { maxrow = 10; }
            else
            {
                if (maxrow > 11) { isValidLocation = false; } // If ship goes outside grid it is not valid location
            }
            if (maxcol == 11) { maxcol = 10; }
            else
            {
                if (maxcol > 11) { isValidLocation = false; } // If ship goes outside grid it is not valid location
            }

            //***If the ship will fit in the grid, now check the location and surrounding spots are empty.***
            if (isValidLocation)
            {
                foreach (var spot in activeplayer.ShipLocations)   // For each spot in the active player's ShipLocations grid...
                {
                    int rowAsNum = Convert.ToInt32(spot.SpotLetter) - 64; // Ascii to integer kludge

                    if (spot.SpotNumber >= startcol && spot.SpotNumber <= maxcol)   // If the spot's number is greater than the startcol & the spot's number is less than the maxcol
                    {
                        if (rowAsNum >= startrow && rowAsNum <= maxrow) // If the row is greater than the startrow && less or equal to max row...
                        {
                            if (spot.Status != GridSpotStatus.EMPTY)
                            {
                                isValidLocation = false;    // If the grid spot is not empty, invalid location.
                            }
                        }
                    }
                }
            }
            if (isValidLocation)
            {
                PlaceShip(activeplayer, row, column, ShipIsHorizontal, ShipLength, ShipName);   // If is valid location, place ship.
            }
            return isValidLocation; // Return true or false
        }


        // ***************UNUSED METHODS******************
        // These are methods which require further development, will be used to determine when and which ship has been sunk by a hit.
        /// <summary>
        /// ShipTakenHit
        /// This method adds one to the number of hits on the ship.
        /// This is then passes on to another method (didHitSinkShip)to see if the hit sunk the ship.
        /// </summary>
        //public void shipTakenAHit(int ShipHits, int ShipLength)
        //{
        //    ShipHits++;
        //    didHitSinkShip(ShipHits, ShipLength);
        //}

        /// <summary>
        /// didHitSinkShip
        /// This method is to see if the hit sunk the ship.
        /// Once the number of hits that the ship has taken is greater than or equal to the ship length, then the ship sinks.
        /// </summary>
        //public bool didHitSinkShip(int hits, int length)
        //{
        //    bool isSunk = false;
        //    if (hits >= length)
        //    {
        //        isSunk = true;
        //    }
        //    else
        //    {
        //        isSunk = false;
        //    }
        //    return isSunk;

        //}
    }
}

