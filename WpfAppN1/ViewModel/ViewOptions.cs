using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace WpfAppN1.ViewModel
{
    public class ViewOptions : ObservableObject
    {
        /// <summary>
        /// indicates if the node ids should be visible
        /// </summary>
        public bool ShowNodeId { get; set; }

        /// <summary>
        /// Normal background color for a node
        /// </summary>
        public Brush NodeNormalBackground { get; set; }

        /// <summary>
        /// Normal background color for a node visited by an algoritme
        /// </summary>
        public Brush NodeVisitedBackground { get; set; }

        /// <summary>
        /// Normal background color for a node in the solution result of an alogrime
        /// </summary>
        public Brush NodeSolutionBackground { get; set; }


        /// <summary>
        /// border color when nodes are not connected
        /// </summary>
        public Brush BorderWeightNotConnected { get; set; }

        /// <summary>
        /// border color when connection weight is low
        /// </summary>
        public Brush BorderWeightLow { get; set; }

        /// <summary>
        /// border color when connection weight is high
        /// </summary>
        public Brush BorderWeightHigh { get; set; }


        /// <summary>
        /// Algorithm to use in calculation
        /// </summary>
        public AlgorithmEnum Algorithm
        {
            get { return _algorithm; }
            set { _algorithm = value; RaisePropertyChangedEvent("Algorithm"); }
        }
        private AlgorithmEnum _algorithm;


        public ViewOptions()
        {
            // defaults
            ShowNodeId = false;

            NodeNormalBackground = Brushes.BurlyWood;
            NodeVisitedBackground = Brushes.LightSkyBlue;
            NodeSolutionBackground = Brushes.BlanchedAlmond;

            BorderWeightNotConnected = Brushes.Black;
            BorderWeightLow = Brushes.Beige;
            BorderWeightHigh = Brushes.Orange;

            Algorithm = AlgorithmEnum.DFS;
        }
    }

}
