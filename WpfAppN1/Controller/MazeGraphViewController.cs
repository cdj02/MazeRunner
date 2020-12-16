using NodeModel.MazeGraph;
using PathfindAlgorithm;
using PathfindAlgorithm.BFSAlgorithm;
using PathfindAlgorithm.Dijkstra;
using PathfindAlgorithm.A_star;
using System.IO;
using System.Text.Json;
using WpfAppN1.ViewModel;

namespace WpfAppN1.Controller
{
    public class MazeGraphViewController
    {


        private readonly MazeGraphViewModel _mazeGraphViewModel;

        public MazeGraphViewController(MazeGraphViewModel gridNodeViewModel)
        {
            _mazeGraphViewModel = gridNodeViewModel;
        }


        public void Create(int nrCol, int nrRow, bool createBlankMaze)
        {
            var gridNodeMap = MazeGraphFactory.CreateGridNodeMap(nrCol, nrRow, createBlankMaze);

            _mazeGraphViewModel.Columns = gridNodeMap.NumberColumns;
            _mazeGraphViewModel.Rows = gridNodeMap.NumberRows;
            _mazeGraphViewModel.GridNodeMap = gridNodeMap;
        }

        public void Save(string file)
        {

            var json = JsonSerializer.Serialize(_mazeGraphViewModel.GridNodeMap);
            File.WriteAllText(file, json);
        }

        public void Open(string file)
        {
            var json = File.ReadAllText(file);
            var gridNodeMap = JsonSerializer.Deserialize<MazeGraph>(json);
            gridNodeMap.ResetNodeConnections();

            _mazeGraphViewModel.Columns = gridNodeMap.NumberColumns;
            _mazeGraphViewModel.Rows = gridNodeMap.NumberRows;
            _mazeGraphViewModel.GridNodeMap = gridNodeMap;
        }

        public void SetStartNode(NodeViewModel node)
        {
            if (node == null)
            {
                return;
            }

            var currentStartNode = _mazeGraphViewModel.GetNodeView(_mazeGraphViewModel.GridNodeMap.StartNode);
            if (currentStartNode.Node != null && currentStartNode.Node.Id != node.Node.Id)
            {

                // set the start node
                _mazeGraphViewModel.GridNodeMap.StartNode = node.Node;

                // reset the node states of the involved nodes
                node.SetNodeViewState();
                currentStartNode.SetNodeViewState();
            }

        }
        public void SetEndNode(NodeViewModel node)
        {
            if (node == null)
            {
                return;
            }

            var currentEndNode = _mazeGraphViewModel.GetNodeView(_mazeGraphViewModel.GridNodeMap.EndNode);
            if (currentEndNode.Node != null && currentEndNode.Node.Id != node.Node.Id)
            {

                // set the start node
                _mazeGraphViewModel.GridNodeMap.EndNode = node.Node;

                // reset the node states of the involved nodes
                node.SetNodeViewState();
                currentEndNode.SetNodeViewState();
            }

        }

        public void Calculate(AlgorithmEnum algorithmToUse)
        {
            ResetView();

            IAlgorithm algorithm = GetAlgorithm(algorithmToUse);

            var result = algorithm.Calculate(_mazeGraphViewModel.GridNodeMap);
            _mazeGraphViewModel.PathFindResult = result;

            foreach (var node in result.Solution)
            {
                var x = _mazeGraphViewModel.GetNodeView(node);
                x.SetNodeViewState();
            }

            foreach (var node in result.VisitedNodes)
            {
                var x = _mazeGraphViewModel.GetNodeView(node);
                x.SetNodeViewState();
            }

        }

        private IAlgorithm GetAlgorithm(AlgorithmEnum algorithmToUse)
        {
            switch (algorithmToUse)
            {
                case AlgorithmEnum.BFS : return new BFS();
                case AlgorithmEnum.DFS: return new DFS();
                case AlgorithmEnum.Dijkstra: return new Dijkstra();
                case AlgorithmEnum.A_star: return new A_star();
            }

            return null;
        }

        private void ResetView()
        {
            _mazeGraphViewModel.PathFindResult = new NodeModel.Pathfind.PathFindResult();


            foreach (var viewNode in _mazeGraphViewModel.NodeList)
            {                
                viewNode.SetNodeViewState();
            }
        }
    }
}
