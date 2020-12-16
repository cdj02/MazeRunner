using NodeModel.GraphModel;
using NodeModel.Pathfind;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PathfindAlgorithm.Dijkstra
{

    public class Dijkstra : IAlgorithm
    {

        private List<Node> processed = new List<Node>();
        private List<string> visited = new List<string>();

        public PathFindResult Calculate(Graph graph)
        {
            // reset graph values
            graph = ResetGraphValues(graph);

            // initialise results - Dijkstra starts here
            var sw = new Stopwatch();
            sw.Start();
            var result = new PathFindResult();

            // initialise start and end node
            var start = graph.GetNodeById(graph.StartNode.Id);

            start.Parent = null;
            var end = graph.GetNodeById(graph.EndNode.Id);

            int totalNodes = graph.Nodes.Count;
            int totalConnections = graph.Connections.Count;

            // initialise queue
            var q = new List<Node>();
            q.Add(start);


            while (!processed.Contains(end))
            {
                // pick node with minimum value and which is unprocessed
                q = q.OrderBy(x => x.CostFromStart).ToList();
                var currentNode = q.First();
                q.Remove(currentNode);
                visited.Add(currentNode.Id);

                // explore adjacent nodes
                currentNode.Connections.OrderBy(x => x.Weight);
                foreach(var connection in currentNode.Connections)
                {
                    var node = connection.NodeTo(currentNode);
                    var nextNode = graph.GetNodeById(node.Id);

                    if (nextNode == start)
                    {
                        // do nothing
                    }
                    else if (nextNode.CostFromStart == 0 || currentNode.CostFromStart + connection.Weight < node.CostFromStart)
                    {
                        nextNode.CostFromStart = currentNode.CostFromStart + connection.Weight;
                        nextNode.Parent = currentNode;
                        q.Add(nextNode);
                    }
                }
                // mark node as processed
                processed.Add(currentNode);
            }

            // shortest path found - Dijkstra ends here
            sw.Stop();
            result.ElapsedMilliseconds = sw.ElapsedMilliseconds;

            // construct shortest path
            var shortestPath = new List<Node>();
            var constructionNode = end;
            while (!shortestPath.Contains(start))
            {
                shortestPath.Add(constructionNode.Parent);
                constructionNode = constructionNode.Parent;
            }
            shortestPath.Add(end);
            shortestPath.Reverse();

            // conclude results
            result.PathWeight = end.CostFromStart;
            result.Solution = shortestPath;
            result.VisitedNodes = visited.Select(x => graph.GetNodeById(x)).ToList();

            return result;

        }

        private Graph ResetGraphValues(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                node.CostFromStart = 0;
            }
            return graph;
        }
    }
}
