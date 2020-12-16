using NodeModel.GraphModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NodeModel.MazeGraph
{
    /// <summary>
    /// A maze graph is a graph where all nodes are arranged in a rectangle grid and a node only has a connections with one ore more direct neighbour
    /// </summary>
    public class MazeGraph : Graph
    {
        /// <summary>
        /// Number of columns in maze
        /// </summary>
        public int NumberColumns { get; set; } = 5;

        /// <summary>
        /// Number of rows in maze
        /// </summary>
        public int NumberRows { get; set; } = 5;

        /// <summary>
        /// Genererate an id for a node based on the location in the maze
        /// </summary>
        /// <param name="column">node location column</param>
        /// <param name="row">node location row</param>
        /// <returns></returns>
        public static string GenerateNoteId(int column, int row)
        { 
            var colTxt = GetColumnName(column);
            var id =  $"{colTxt}_{row + 1}";
            return id;
        }

        /// <summary>
        /// returns a columnnumber name for the given number (excel style :  A..Z, AA, AB, AC.. etc)
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        private static string GetColumnName(int columnNumber)
        {
            int dividend = columnNumber + 1;
            string columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar('A'  + modulo).ToString() + columnName;
                dividend = ((dividend - modulo) / 26);
            }

            return columnName;
        }


        /// <summary>
        /// Genererate an id for a connection based on the connected nodes
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static string GenerateConnectionId(Node node1, Node node2)
        {
            // sort the nodes, so a connection from node1 <-> node2 wil have the same id as  node2 <-> node1
            var sortedList = new List<string> { node1.Id, node2.Id }
            .OrderBy(id => id)
            .ToArray();

            var id1 = sortedList[0];
            var id2 = sortedList[1];

            return $"{id1} <=> {id2}";
        }
  

        public List<Node> GetNeighbours(Location location)
        {
            // Make a list of neighbours (left,top,right,bottom)
            var result = new List<Node>();

            foreach (var loc in Location.GetNeighbourLocations(location.X, location.Y))
            {
                // get the node for the current neighbour location
                var neighbourNodeId = GenerateNoteId(loc.X, loc.Y);
                var neighbourNode = GetNodeById(neighbourNodeId);

                // if found, add to the result
                if (neighbourNode!= null)
                {
                    result.Add(neighbourNode);
                }
            }

            return result;
        }

        public Node GetNeighbour(Location location, Direction direction)
        {
            var neighbourLocation = Location.GetNeighbourLocation(direction, location.X, location.Y);

            // get the node for the current neighbour location
            var neighbourNodeId = GenerateNoteId(neighbourLocation.X, neighbourLocation.Y);
            var neighbourNode = GetNodeById(neighbourNodeId);

            return neighbourNode;
        }

    }

}
