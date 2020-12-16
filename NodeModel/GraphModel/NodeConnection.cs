using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeModel.GraphModel
{
    public class NodeConnection
    {
        /// <summary>
        /// unique identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The connection's first node
        /// </summary>
        public Node Node1 { get; set; }

        /// <summary>
        /// The connection's second node
        /// </summary>
        public Node Node2 { get; set; }

        /// <summary>
        /// The cost of the connection's use. default = 1
        /// </summary>
        public int Weight { get; set; } = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        public NodeConnection()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">identifier</param>
        public NodeConnection(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Returns the node the connection is pointing to viewed from the given nodefrom's perspective
        /// </summary>
        /// <param name="nodeFrom"></param>
        /// <returns></returns>
        public Node NodeTo(Node nodeFrom)
        {
            if (nodeFrom.Id == Node1.Id) return Node2;
            if (nodeFrom.Id == Node2.Id) return Node1;

            throw new ArgumentException($"nodeFrom {nodeFrom.Id} is not a part of this connection {Id}");
        }

    }
}
