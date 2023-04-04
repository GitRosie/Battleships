using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
	/// <summary>
	/// Enumerated type
	/// This is used for the status of each spot in the playing grid.
	/// </summary>
	public enum GridSpotStatus
    {
		EMPTY,
		BATTLESHIP,
		DESTROYER,
		CRUISER,
		//add other ships as desired
		HIT,
		MISS
	}
}
