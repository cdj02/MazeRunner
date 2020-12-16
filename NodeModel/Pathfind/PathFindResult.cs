using NodeModel.GraphModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeModel.Pathfind
{
    public class PathFindResult
    {
        public List<Node> Solution { get; set; } = new List<Node>();
       
        public List<Node> VisitedNodes { get; set; } = new List<Node>();

        public int PathWeight { get; set; }

        public long ElapsedMilliseconds { get; set; }

        public bool SolutionFound
        {
            get
            {
                return Solution.Count > 0;
            }
        }

    }
}
