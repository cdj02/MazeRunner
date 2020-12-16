using NodeModel.GraphModel;
using NodeModel.MazeGraph;
using System;
using System.Windows;
using System.Windows.Media;

namespace WpfAppN1.ViewModel
{
    public class NodeViewModel : ObservableObject
    {

        /// <summary>
        /// the mazegraphViewmodel where this node is a part of 
        /// </summary>
        private MazeGraphViewModel _mazeGraphViewModel;

        /// <summary>
        /// neighbours nodes for the 4 directions (left,top,right,bottom)
        /// </summary>
        private NodeViewModel[] _neighbours = new NodeViewModel[4];

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gridNodeViewModel"></param>
        public NodeViewModel(MazeGraphViewModel gridNodeViewModel)
        {
            _mazeGraphViewModel = gridNodeViewModel;
        }
  
        /// <summary>
        /// Node background
        /// </summary>
        public Brush Background { 
            get { return _background; }
            set { _background = value; RaisePropertyChangedEvent("BackGround"); } 
        }
        private Brush _background;

        /// <summary>
        /// Node label
        /// </summary>
        public string Label
        {
            get { return _label; }
            set { _label = value; RaisePropertyChangedEvent("Label"); }
        }
        private string _label;


        /// <summary>
        /// borders for the 4 directions (left,top,right,bottom)
        /// </summary>
        private NodeBorderViewModel[] _nodeBorders = new NodeBorderViewModel[4] { new NodeBorderViewModel(), new NodeBorderViewModel(), new NodeBorderViewModel(), new NodeBorderViewModel() };

        public NodeBorderViewModel BorderLeft
        {
            get { return _nodeBorders[(int)Direction.Left]; } 
            set { _nodeBorders[(int)Direction.Left] = value; RaisePropertyChangedEvent("BorderLeft"); }
        }

        public NodeBorderViewModel BorderTop
        {
            get { return _nodeBorders[(int)Direction.Top]; }
            set { _nodeBorders[(int)Direction.Top] = value; RaisePropertyChangedEvent("BorderTop"); }
        }

        public NodeBorderViewModel BorderRight
        {
            get { return _nodeBorders[(int)Direction.Right]; }
            set { _nodeBorders[(int)Direction.Right] = value; RaisePropertyChangedEvent("BorderRight"); }
        }

        public NodeBorderViewModel BorderBottom
        {
            get { return _nodeBorders[(int)Direction.Bottom]; }
            set { _nodeBorders[(int)Direction.Bottom] = value; RaisePropertyChangedEvent("BorderBottom"); }
        }

        /// <summary>
        /// Node's location in the mazeRow
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// The node this view represents
        /// </summary>
        public Node Node
        {
            get { return _node; }
            set { _node = value; SetNodeViewState(); }
        }
        private Node _node;

        public NodeViewModel GetNeighbour(Direction direction)
        {
            var index = (int)direction;
            return _neighbours[index];
        }
        public void SetNeighbour(Direction direction, NodeViewModel value)
        {
            var index = (int)direction;
            _neighbours[index] = value;
            SetNodeViewState();
        }


        /// <summary>
        /// sets the node view state according to node state
        /// </summary>
        public void SetNodeViewState()
        {
            Label = GetLabel();
            Background = GetBackGround();

            foreach (var direction in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                SetBorderViewModel(direction);
            }
        }

        private Brush GetBackGround()
        {
            // start/end node
            if (_mazeGraphViewModel.IsStartNode(this) || _mazeGraphViewModel.IsEndNode(this))
            {
              return _mazeGraphViewModel.ViewOptions.NodeSolutionBackground;
            }

            if (_mazeGraphViewModel.PathFindResult != null && _mazeGraphViewModel.PathFindResult.Solution.Contains(Node))
            {
                return _mazeGraphViewModel.ViewOptions.NodeSolutionBackground;
            }

            if (_mazeGraphViewModel.PathFindResult != null && _mazeGraphViewModel.PathFindResult.VisitedNodes.Contains(Node))
            {
                return _mazeGraphViewModel.ViewOptions.NodeVisitedBackground;
            }

            return _mazeGraphViewModel.ViewOptions.NodeNormalBackground;
        }

        private string GetLabel()
        {

            if (_mazeGraphViewModel.IsStartNode(this))
            {
                return "Start";
            }

            if (_mazeGraphViewModel.IsEndNode(this))
            {
                return "Finish";
            }

            if (_mazeGraphViewModel.ViewOptions.ShowNodeId)
            {
                return Node.Id;
            }

            return string.Empty;
        }

        private void SetBorderViewModel(Direction direction)
        {
            var borderViewModel = _nodeBorders[(int)direction];
            borderViewModel.Brush = GetBorderBrush(direction);
            borderViewModel.SetThickness(GetBorderWidth(direction), direction);
        }

        private int GetBorderWidth(Direction direction)
        {      
            var neighbourNode = GetNeighbour(direction);

            // edge of maze : double size wall
            return (neighbourNode == null) ? 2 : 1;
        }

        private Brush GetBorderBrush(Direction direction)
        {
            var neighbourNode = GetNeighbour(direction);
            
            // edge of maze or not connected
            if (neighbourNode == null)
            {
                return _mazeGraphViewModel.ViewOptions.BorderWeightNotConnected;
            }

            var connection = _mazeGraphViewModel.GetConnection(this, neighbourNode);
            if (connection == null)
            {
                return _mazeGraphViewModel.ViewOptions.BorderWeightNotConnected;
            }

            if (connection.Weight < 5)
            {
                return _mazeGraphViewModel.ViewOptions.BorderWeightLow;
            }

            if (connection.Weight >= 5)
            {
                return _mazeGraphViewModel.ViewOptions.BorderWeightHigh;
            }

            // default
            return Brushes.Transparent;
        }

    }
}

