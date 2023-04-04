using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models
{
    /// <summary>
    /// Model of the information stored for each spot on shot and loactions grids
    /// </summary>
    public class GridSpotModel
    {
        public int SpotNumber { get; set; }     // Column
        public char SpotLetter { get; set; }      // Row
        public GridSpotStatus Status = GridSpotStatus.EMPTY;    // Spot is empty
    }
}
