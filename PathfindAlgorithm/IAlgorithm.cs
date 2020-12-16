using NodeModel.GraphModel;
using NodeModel.Pathfind;

namespace PathfindAlgorithm
{
    public interface IAlgorithm
    {
        PathFindResult Calculate(Graph graph);
    }
}
