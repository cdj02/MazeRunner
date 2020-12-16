using NodeModel.MazeGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NodeModel.GraphModel
{
    public class Node
    {

        /// <summary>
        /// unique identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// node location
        /// </summary>
        public Location Location { get; set; }

        [JsonIgnore]
        /// <summary>
        /// keeps track of shortest cost from start
        /// </summary>
        public int CostFromStart { get; set; }

        [JsonIgnore]
        /// <summary>
        /// heuristic function from the point of view of start
        /// </summary>
        public double GCost { get; set; }

        [JsonIgnore]
        /// <summary>
        /// heuristic function from point of view of end
        /// </summary>
        public double HCost { get; set; }

        [JsonIgnore]
        /// <summary>
        /// G and H parameters added together
        /// </summary>
        public double FCost { get; set; }

        /// <summary>
        /// keeps track of parent
        /// </summary>
        [JsonIgnore]
        public Node Parent { get; set; }

        /// <summary>
        /// the connections this node is part of (could be connection.node1 of node2)
        /// </summary>
        /// <remarks>cannot be used in json (de)serialization and should be (re)set after deserialization</remarks>
        [JsonIgnore]
        public List<NodeConnection> Connections { get; set; } = new List<NodeConnection>();

        /// <summary>
        /// constructor 
        /// </summary>
        public Node()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id">identifier</param>
        public Node(string id)
        {
            Id = id;
        }

    }

}
