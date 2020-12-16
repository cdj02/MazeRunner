
using NodeModel.MazeGraph;
using System.Windows;
using System.Windows.Media;

namespace WpfAppN1.ViewModel
{
    public class NodeBorderViewModel : ObservableObject
    {
        public void SetThickness(int borderWidth, Direction direction)
        {
            double left = direction == Direction.Left ? borderWidth : 0;
            double top = direction == Direction.Top ? borderWidth : 0;
            double right = direction == Direction.Right ? borderWidth : 0;
            double bottom = direction == Direction.Bottom ? borderWidth : 0;
            Thickness = new Thickness(left, top, right, bottom);
        }

        public Thickness Thickness
        {
            get { return _thickness; }
            set { _thickness = value; RaisePropertyChangedEvent("Thickness"); }
        }
        private Thickness _thickness;


        /// <summary>
        /// Border brush 
        /// </summary>
        public Brush Brush
        {
            get { return _brush; }
            set { _brush = value; RaisePropertyChangedEvent("Brush"); }
        }
        private Brush _brush;
    }
}
