using NodeModel.GraphModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeModel.MazeGraph
{
    public class MazeGraphFactory
    {

        public static MazeGraph CreateGridNodeMap(int nrColumns, int nrRows, bool createBlank)
        {
            var result = new MazeGraph
            {
                NumberColumns = nrColumns,
                NumberRows = nrRows
            };

            CreateNodes(result);
            if (createBlank)
            {
                CreateAllNodeConnections(result);
            }
            else
            {
                CreateRandomNodeConnections(result);
            }

            result.StartNode = result.Nodes.First();
            result.EndNode = result.Nodes.Last();

            return result;
        }

        private static void CreateNodes(MazeGraph nodeMap)
        {
            for (int column = 0; column < nodeMap.NumberColumns; column++)
            {
                for (int row = 0; row < nodeMap.NumberRows; row++)
                {
                    // create Node
                    var nodeId = MazeGraph.GenerateNoteId(column, row);
                    var node = new Node(nodeId)
                    {
                        Location = new Location(column, row)
                    };
                    nodeMap.Nodes.Add(node);
                }
            }

        }

        private static void CreateAllNodeConnections(MazeGraph gridNodeMap)
        {

            for (int column = 0; column < gridNodeMap.NumberColumns; column++)
            {
                for (int row = 0; row < gridNodeMap.NumberRows; row++)
                {

                    // Get the current node
                    var currentNodeId = MazeGraph.GenerateNoteId(column, row);
                    var currentNode = gridNodeMap.GetNodeById(currentNodeId);

                    // add  connections to neighbours
                    foreach (var neighbourNode in gridNodeMap.GetNeighbours(new Location(column, row)))
                    {
                        if (neighbourNode == null)
                            continue;

                        // add the connection is it not already present
                        if (!gridNodeMap.HasConnection(currentNode, neighbourNode))
                        {
                            var connectionId = MazeGraph.GenerateConnectionId(currentNode, neighbourNode);
                            gridNodeMap.AddConnection(connectionId, currentNode, neighbourNode);
                        }
                    }

                }

            }
        }


        private static void CreateRandomNodeConnections(MazeGraph gridNodeMap)
        {

            var PredecessorIdByNodeId = gridNodeMap.Nodes.ToDictionary(x => x.Id, x => string.Empty);
            var possibleConnections = new List<NodeConnection>();


            Random rand = new Random();

            var root = gridNodeMap.Nodes.First(); // random ?
            PredecessorIdByNodeId[root.Id] = root.Id;

            foreach (var neighbour in gridNodeMap.GetNeighbours(root.Location))
            {
                possibleConnections.Add(new NodeConnection() { Node1 = root, Node2 = neighbour });
            }


            while (possibleConnections.Count > 0)
            {

                int randomIndex = rand.Next(0, possibleConnections.Count - 1);
                var selected = possibleConnections[randomIndex];

                possibleConnections.RemoveAt(randomIndex);

                var fromNode = selected.Node1;
                var toNode = selected.Node2;

                PredecessorIdByNodeId[toNode.Id] = fromNode.Id;

                // -- 
                var toremove = possibleConnections.Where(pc => PredecessorIdByNodeId[pc.Node2.Id] != string.Empty).ToList();

                possibleConnections.RemoveAll(i => toremove.Contains(i));

                var location = toNode.Location;
                foreach (var neighbour in gridNodeMap.GetNeighbours(location))
                {
                    if (PredecessorIdByNodeId[neighbour.Id] == string.Empty)
                    {
                        possibleConnections.Add(new NodeConnection() { Node1 = toNode, Node2 = neighbour });
                    }
                }

            }

            foreach (var c in PredecessorIdByNodeId)
            {
                var nodeid1 = c.Key;
                var nodeid2 = c.Value;
                if (nodeid1 == nodeid2)
                    continue;

                var n1 = gridNodeMap.GetNodeById(nodeid1);
                var n2 = gridNodeMap.GetNodeById(nodeid2);

                var id = MazeGraph.GenerateConnectionId(n1, n2);
                gridNodeMap.AddConnection(id, n1, n2);
            }

        }

    }
}
