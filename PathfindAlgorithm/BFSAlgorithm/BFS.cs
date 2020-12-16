using NodeModel.GraphModel;
using NodeModel.Pathfind;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PathfindAlgorithm.BFSAlgorithm
{

    public class BFS : IAlgorithm
    {

        //private List<string> currentChainNodeIDs = new List<string>();
        private List<string> visited = new List<string>();

        public PathFindResult Calculate(Graph graph)
        {
            // initialise results - BFS starts here
            var sw = new Stopwatch();
            sw.Start();
            var result = new PathFindResult();

            // initialise start and end node
            var start = graph.GetNodeById(graph.StartNode.Id);
            var end = graph.GetNodeById(graph.EndNode.Id);

            // create queue and parent connections
            LinkedList<Node> q = new LinkedList<Node>();
            q.AddFirst(start);
            visited.Add(start.Id);

            Node nodeVisit = null;

            // the traverse list with all 
            var traverseList = new TraverseList();


            while (q != null)
            {
                // note down parent and remove from list, since all connections will be searched
                var node = q.First();

                // find all possible connections
                var connectionNodesIds = node.Connections.Select(x => x.NodeTo(node).Id).ToList();
                var possibleVisits = connectionNodesIds.Where(x => !visited.Contains(x)).ToList();

                // for each connection
                if (possibleVisits.Count == 0)
                {
                    nodeVisit = q.First();
                }
                else if (possibleVisits.Contains(end.Id))
                {
                    traverseList.Add(new Traverse { ParentNodeId = node.Id, ChildNodeId = end.Id });
                    visited.Add(end.Id);
                    break;
                }
                else
                {
                    for (int i = 0; i < possibleVisits.Count; i++)
                    {
                        traverseList.Add(new Traverse { ParentNodeId = node.Id, ChildNodeId = possibleVisits[i] });
                        nodeVisit = graph.GetNodeById(possibleVisits[i]);
                        q.AddLast(nodeVisit);
                        visited.Add(nodeVisit.Id);
                    }
                }

                // finish processing queue node
                q.RemoveFirst();
            }

            // path found - BFS stops here
            sw.Stop();
            result.ElapsedMilliseconds = sw.ElapsedMilliseconds;

            //conclude results
            foreach (var solutionNodeId in GetSolutionFromTraveseList(traverseList, start.Id, end.Id))
            {
                result.Solution.Add(graph.GetNodeById(solutionNodeId));            
            }
            end.Parent = result.Solution.Last();
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
                        result.PathWeight += connection.Weight;
                        break;
                    }
                }
            }

            return result;

        }

        private List<string> GetSolutionFromTraveseList(TraverseList traverseList, string startNodeId, string endNodeId)
        {
            var result = new List<string>();

            var currentTraverse = traverseList.GetByChildNodeId(endNodeId);

            while (currentTraverse.ParentNodeId != startNodeId)
            {
                currentTraverse = traverseList.GetByChildNodeId(currentTraverse.ParentNodeId);
                result.Add(currentTraverse.ChildNodeId);
            }

            result.Add(startNodeId);

            result.Reverse();
            return result;

        }
    }
}
