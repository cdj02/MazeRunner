using NodeModel.MazeGraph;
using NodeModel.GraphModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NodeModel.Pathfind;

namespace WpfAppN1.ViewModel
{
    public class MazeGraphViewModel : ObservableObject
    {
        
        public int Columns
        {
            get { return _cols; }
            set { _cols = value; RaisePropertyChangedEvent("Columns"); }       
        }
        private int _cols = 0;

        
        public int Rows
        {
            get { return _rows; }
            set { _rows = value; RaisePropertyChangedEvent("Rows"); }
        }
        private int _rows = 0;

        
        public ObservableCollection<NodeViewModel> NodeList
        {
            get { return _nodeList; }
            set { _nodeList = value; RaisePropertyChangedEvent("NodeList"); }
        }
        private ObservableCollection<NodeViewModel> _nodeList;

        
        public MazeGraph GridNodeMap
        {
            get { return _gridNodeMap; }
            set { _gridNodeMap = value; SetGridNodeMap(); }
        }
        private MazeGraph _gridNodeMap;


        public PathFindResult PathFindResult
        { 
            get { return _pathFindResult; }
            set { _pathFindResult = value; RaisePropertyChangedEvent("NodeList"); }
        }
        private PathFindResult _pathFindResult;

        public ViewOptions ViewOptions { get; set; }

        private void SetGridNodeMap()
        {
            var nodelist = new ObservableCollection<NodeViewModel>();

            // create nodeviewmodels for each node. important : the order in the grid is first columns, then rows. keep columnns for loop as inner loop
            for (int row = 0; row < _gridNodeMap.NumberRows; row++)
            {
                for (int column = 0; column < _gridNodeMap.NumberColumns; column++)
                {
                    // get node
                    var nodeId = MazeGraph.GenerateNoteId(column, row);
                    var node = _gridNodeMap.GetNodeById(nodeId);

                    var nodeView = new NodeViewModel(this)
                    {
                        Node = node,
                        Location = new Location(column,row)
                    };
                    nodelist.Add(nodeView);
                }
            }

            NodeList = nodelist;

            // set neighbour nodes for the nodeviewmodels
            foreach (var nodeViewModel in nodelist)
            {
                nodeViewModel.SetNeighbour(Direction.Left, GetNeighbourNode(nodeViewModel,Direction.Left));
                nodeViewModel.SetNeighbour(Direction.Top, GetNeighbourNode(nodeViewModel, Direction.Top));
                nodeViewModel.SetNeighbour(Direction.Right, GetNeighbourNode(nodeViewModel, Direction.Right));
                nodeViewModel.SetNeighbour(Direction.Bottom, GetNeighbourNode(nodeViewModel, Direction.Bottom));
            }
        }

        private NodeViewModel GetNeighbourNode(NodeViewModel nodeViewModel, Direction direction)
        {
            var location = Location.GetNeighbourLocation(direction, nodeViewModel.Location.X, nodeViewModel.Location.Y);
            var neighbourgNode = NodeList.Where(node => node.Location.X == location.X && node.Location.Y == location.Y).FirstOrDefault();

            return neighbourgNode;
        }

        public NodeViewModel GetNodeView(Node node)
        {
            return NodeList.Where(x => x.Node.Id == node.Id).FirstOrDefault();
        }

        public bool IsEndNode(NodeViewModel nodeViewModel)
        {
            return GridNodeMap.EndNode.Id == nodeViewModel.Node.Id;
        }

        public bool IsStartNode(NodeViewModel nodeViewModel)
        {
            return GridNodeMap.StartNode.Id == nodeViewModel.Node.Id;
        }

        public bool IsConnected(NodeViewModel nodeViewModel1, NodeViewModel nodeViewModel2)
        {
            return _gridNodeMap.HasConnection(nodeViewModel1.Node, nodeViewModel2.Node);
        }

        public NodeConnection GetConnection(NodeViewModel nodeViewModel1, NodeViewModel nodeViewModel2)
        {
            return _gridNodeMap.GetConnection(nodeViewModel1.Node, nodeViewModel2.Node);
        }

        public void ToggleConnection(NodeViewModel node1, NodeViewModel node2)
        {
            if (node1 == null || node2 == null)
                return;

            var connection = GetConnection(node1, node2);

            if (connection == null)
            {
                AddConnection(node1, node2);
                return;
            }

            if (connection.Weight < 5)
            {
                connection.Weight = 5;
                node1.SetNodeViewState();
                node2.SetNodeViewState();
                return;
            }

            if (connection.Weight >= 5)
            {
                RemoveConnection(node1, node2);
                return;
            }            
        }

        private void RemoveConnection(NodeViewModel nodeViewModel1, NodeViewModel nodeViewModel2)
        {
            var node1 = nodeViewModel1.Node;
            var node2 = nodeViewModel2.Node;


            if (_gridNodeMap.HasConnection(node1, node2))
            {
                var connectionId = MazeGraph.GenerateConnectionId(node1, node2);
                _gridNodeMap.RemoveConnection(connectionId);

                nodeViewModel1.SetNodeViewState();
                nodeViewModel2.SetNodeViewState();
            }
        }

        private void AddConnection(NodeViewModel nodeViewModel1, NodeViewModel nodeViewModel2)
        {
            var node1 = nodeViewModel1.Node;
            var node2 = nodeViewModel2.Node;


            if (!_gridNodeMap.HasConnection(node1, node2))
            {
                var connectionId = MazeGraph.GenerateConnectionId(node1, node2);
                _gridNodeMap.AddConnection(connectionId, node1, node2);

                nodeViewModel1.SetNodeViewState();
                nodeViewModel2.SetNodeViewState();
            }
        }

    }
}
