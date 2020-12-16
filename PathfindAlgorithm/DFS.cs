using NodeModel.GraphModel;
using NodeModel.Pathfind;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PathfindAlgorithm
{
    public class DFS :IAlgorithm
    {

        private List<string> currentChainNodeIDs = new List<string>();
        private List<string> visited = new List<string>();

        public PathFindResult Calculate(Graph graph)
        {
            // DFS starts here
            var sw = new Stopwatch();
            sw.Start();

            var result = new PathFindResult();

            var current = graph.GetNodeById(graph.StartNode.Id);
            var end = graph.GetNodeById(graph.EndNode.Id);

            while (current.Id != end.Id)
            {
                
                visited.Add(current.Id);

                if (!currentChainNodeIDs.Contains(current.Id))
                {
                    currentChainNodeIDs.Add(current.Id);
                }

                current = GetNodeToVisit(current, graph); 

                if (current == null)
                {
                    var last = currentChainNodeIDs.Last();
                    currentChainNodeIDs.Remove(last);

                    current = graph.GetNodeById(currentChainNodeIDs.Last());
                }
                
            }

            // path found - DFS ends here
            sw.Stop();
            result.ElapsedMilliseconds = sw.ElapsedMilliseconds;

            //conclude results
            result.Solution = currentChainNodeIDs.Select(x => graph.GetNodeById(x)).ToList();
            result.Solution.Add(end);
            result.VisitedNodes = visited.Select(x => graph.GetNodeById(x)).ToList();

            Node prevNode = null;
            foreach (var node in result.Solution)
            {
                foreach (var connection in node.Connections)
                {
                    if (connection.NodeTo(node) != prevNode && result.Solution.Contains(connection.NodeTo(node)))
                    {
                        prevNode = node;
                        result.PathWeight = result.PathWeight + connection.Weight;
                        break;
                    }
                }
            }

            return result;
        }

        private Node GetNodeToVisit(Node current, Graph graph)
        {
            var connectionNodesIds = current.Connections.Select(x => x.NodeTo(current).Id).ToList();
            var possibleVisits = connectionNodesIds.Where(x => !visited.Contains(x)).ToList();

            if (possibleVisits.Count == 0)
            {
                return null;
            }

            //set random
            var random = new Random();
            var indexNextConnection = random.Next(possibleVisits.Count);

            var nextVisit = possibleVisits[indexNextConnection];

            if (visited.Contains(nextVisit) || String.IsNullOrEmpty(nextVisit))
            {
                return null;
            }
            else
            {
                 return graph.GetNodeById(nextVisit);
            }

            
        }

    }
}
