using NodeModel.GraphModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathfindAlgorithm.BFSAlgorithm
{
    public class TraverseList : List<Traverse>
    {

        public Traverse GetByChildNodeId(string nodeId)
        {
            foreach (var item in this)
            {
                if (item.ChildNodeId == nodeId)
                {
                    return item;
                }
            }

            // not found
            return null;
        }
    }
}
