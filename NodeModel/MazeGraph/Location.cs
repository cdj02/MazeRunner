using System;
using System.Collections.Generic;
using System.Text;

namespace NodeModel.MazeGraph
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }


        public Location()
        {
        }

        public Location(int column, int row)
        {
            X = column;
            Y = row;
        }

        public static Location GetNeighbourLocation(Direction direction, int column, int row)
        {
            switch (direction)
            {
                case Direction.Left: return new Location(column - 1, row);
                case Direction.Top: return new Location(column, row - 1);
                case Direction.Right: return new Location(column + 1, row);
                case Direction.Bottom: return new Location(column, row + 1);
                default: return new Location(column, row);                    
            }
            
        }

        public static List<Location> GetNeighbourLocations(int column, int row)
        {
            return new List<Location>
            {
                GetNeighbourLocation(Direction.Left,column,row),
                GetNeighbourLocation(Direction.Top,column,row),
                GetNeighbourLocation(Direction.Right,column,row),
                GetNeighbourLocation(Direction.Bottom,column,row),
            };

        }

    }
}
