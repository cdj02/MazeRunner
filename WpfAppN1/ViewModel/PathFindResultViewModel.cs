using NodeModel.Pathfind;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfAppN1.ViewModel
{
    public class PathFindResultViewModel
    {
        public AlgorithmEnum Algorithm { get; set; }

        public long ElapsedMilliseconds { get; set; }

        public int NodesVisited { get; set; }

        public int NodesInSolution { get; set; }

        public int Cost { get; set; }

        public static PathFindResultViewModel From(AlgorithmEnum algorithm, PathFindResult result)
        {
            return new PathFindResultViewModel
            {
                Algorithm = algorithm,
                ElapsedMilliseconds = result.ElapsedMilliseconds,
                NodesInSolution = result.Solution.Count,
                NodesVisited = result.VisitedNodes.Count,
                Cost = result.PathWeight
            };

        }
    }
}
