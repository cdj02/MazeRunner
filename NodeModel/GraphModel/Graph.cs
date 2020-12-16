using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeModel.GraphModel

{
    public class Graph
    {

        /// <summary>
        /// nodes in the graph
        /// </summary>
        public List<Node> Nodes { get; set; } = new List<Node>();

        /// <summary>
        /// connections in the graph
        /// </summary>
        public List<NodeConnection> Connections { get; set; } = new List<NodeConnection>();

        /// <summary>
        /// starting point in the graph
        /// </summary>
        public Node StartNode { get; set; }

        /// <summary>
        /// finish point in the graph
        /// </summary>
        public Node EndNode { get; set; }

        /// <summary>
        /// resets the connections inside the nodes themselfs, to be used when nodes or nodecollection was reset (p.e. after json deserialization)
        /// </summary>
        public void ResetNodeConnections()
        {
            // reset start and endnode by selecting nodes from nodelist
            StartNode = GetNodeById(StartNode.Id);
            EndNode = GetNodeById(EndNode.Id);

            // clear existing node connections
            foreach (var node in Nodes)
            {
                node.Connections.Clear();
            }

            foreach (var connection in Connections)
            {
                var node1 = GetNodeById(connection.Node1.Id);
                var node2 = GetNodeById(connection.Node2.Id);

                node1.Connections.Add(connection);
                node2.Connections.Add(connection);
            }
        }


        /// <summary>
        /// Gets the node with the given id
        /// </summary>
        /// <param name="id">node identifier</param>
        /// <returns>node with the given id, null when node is not found</returns>
        public Node GetNodeById(string id)
        {
            return Nodes
                .Where(node => node.Id == id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the connection with the given id
        /// </summary>
        /// <param name="id">connection identifier</param>
        /// <returns>connection with the given id, null when connection is not found</returns>
        public NodeConnection GetNodeConnectionById(string id)
        {
            return Connections
                .Where(node => node.Id == id)
                .FirstOrDefault();
        }


        /// <summary>
        /// Gets the connection for the given nodes
        /// </summary>
        /// <param name="node1">node 1</param>
        /// <param name="node2">node 2</param>
        /// <returns>the connection between node1 and node2 </returns>
        public NodeConnection GetConnection(Node node1, Node node2)
        {
            // if one of the nodes is null, there is no connection
            if (node1 == null || node1 == null)
            {
                return null;
            }

            // search a connection for node1 -> node2 or the 'reverse' connection node2 -> node1
            var connection = Connections
                .Where(c => c.Node1.Id == node1.Id && c.Node2.Id == node2.Id ||
                            c.Node1.Id == node2.Id && c.Node2.Id == node1.Id)
                .FirstOrDefault();

            return connection;
        }


        /// <summary>
        /// Add a connecion between node1 and node2 in the graph
        /// </summary>
        /// <param name="id">connection id</param>
        /// <param name="firstNode">first node</param>
        /// <param name="secondNode">second node</param>
        /// <param name="connectionWeight">connection weight</param>
        public void AddConnection(string id, Node firstNode, Node secondNode, int connectionWeight)
        {
            if (firstNode == null || secondNode == null)
            {
                throw new ArgumentException($"Cannot add connection if a node is null (first {firstNode == null} , second  {secondNode == null}");
            }

            // create the connection and add it to the graph
            var connection = new NodeConnection(id)
            {
                Node1 = firstNode,
                Node2 = secondNode,
                Weight = connectionWeight
            };
            Connections.Add(connection);

            // update the connection list in the nodes themselfs
            firstNode.Connections.Add(connection);
            secondNode.Connections.Add(connection);
        }


        /// <summary>
        /// Add a connecion between node1 and node2 in the graph
        /// </summary>
        /// <param name="id">connection id</param>
        /// <param name="firstNode">first node</param>
        /// <param name="secondNode">second node</param>
        public void AddConnection(string id, Node firstNode, Node secondNode)
        {
            AddConnection(id, firstNode, secondNode, 1);
        }


        /// <summary>
        /// remove a connecionin the graph
        /// </summary>
        /// <param name="id">connection id</param>
        public void RemoveConnection(string id)
        {
            // get the connection
            var connection = GetNodeConnectionById(id);

            if (connection != null)
            {
                // remove the connection from the nodes
                connection.Node1.Connections.Remove(connection);
                connection.Node2.Connections.Remove(connection);

                // remove the connection 
                Connections.Remove(connection);
            }
        }


        /// <summary>
        /// Indicates if a connection exists between the first and second node
        /// </summary>
        /// <param name="firstNode">first node</param>
        /// <param name="secondNode"></param>
        /// <returns></returns>
        public bool HasConnection(Node firstNode, Node secondNode)
        {
            return GetConnection(firstNode, secondNode) != null;
        }

    }

}
