using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models
{
    /// <summary>
    /// The information stored/required for each player
    /// </summary>
    public class PlayerInfoModel
    {
        public string UsersName { get; set; }
        public List<GridSpotModel> ShipLocations { get; set; } = new List<GridSpotModel>();
        public List<GridSpotModel> ShotGrid { get; set; } = new List<GridSpotModel>();
        public List<ShipModel> PlayersShips { get; set; } = new List<ShipModel>();
        //public List<GridSpotModel> Test { get; set; } = new List<GridSpotModel>(); // DEBUG - Duplicate spot objects
    }
}
