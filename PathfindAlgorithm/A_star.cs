using NodeModel.GraphModel;
using NodeModel.Pathfind;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PathfindAlgorithm.A_star
{

    public class A_star : IAlgorithm
    {

        private List<Node> processed = new List<Node>();
        private List<string> visited = new List<string>();

        public PathFindResult Calculate(Graph graph)
        {
            // reset graoph values
            graph = ResetGraphValues(graph);

            // initialise results - a* starts here
            var sw = new Stopwatch();
            sw.Start();
            var result = new PathFindResult();

            // initialise start and end node
            var start = graph.GetNodeById(graph.StartNode.Id);
            var end = graph.GetNodeById(graph.EndNode.Id);

            int totalNodes = graph.Nodes.Count;
            int totalConnections = graph.Connections.Count;

            // initialise queue
            var q = new List<Node>();
            q.Add(start);


            while (!processed.Contains(end))
            {

                // calculate f and h costs if applicable
                if (!q.Contains(start))
                {
                    foreach (var node in q)
                    {
                        node.HCost = CalculateHeuristic(node, end);
                        node.FCost = node.HCost + node.GCost;
                    }
                }

                // pick node with minimum value and which is unprocessed
                q = q.OrderBy(x => x.FCost).ToList();
                var qNode = q.First();
                var currentNode = graph.GetNodeById(qNode.Id);

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
                    else if (nextNode.CostFromStart == 0 || currentNode.CostFromStart + connection.Weight < nextNode.CostFromStart)
                    {
                        nextNode.CostFromStart = currentNode.CostFromStart + connection.Weight;
                        nextNode.GCost = CalculateGCost(nextNode, start, connection);
                        nextNode.Parent = currentNode;
                        q.Add(nextNode);
                    }
                }
                // mark node as processed
                processed.Add(currentNode);
            }

            // shortest path found - a* stops here
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

        private double CalculateHeuristic(Node node, Node end)
        {
            // pythagoras
            int x = end.Location.X - node.Location.X;
            int y = end.Location.Y - node.Location.Y;
            double h = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return h;
        }

        private double CalculateGCost(Node nextNode, Node start, NodeConnection connection)
        {
            // pythagoras
            int x = start.Location.X - nextNode.Location.X;
            int y = start.Location.Y - nextNode.Location.Y;
            double g = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) + connection.Weight;
            return g;
        }
        private Graph ResetGraphValues(Graph graph)
        {
            Node resetNode = null;
            foreach (var node in graph.Nodes)
            {
                resetNode = graph.GetNodeById(node.Id);
                node.CostFromStart = 0;
                node.FCost = 0;
                node.GCost = 0;
                node.HCost = 0;
                node.Parent = null;
                node.Location = resetNode.Location;
            }
            return graph;
        }
    }
}
