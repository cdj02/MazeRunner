using NodeModel.Pathfind;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfAppN1.ViewModel
{
    public class PathFindResultController : ObservableObject
    {

        public List<PathFindResultViewModel> PathFindResults { get; } = new List<PathFindResultViewModel>();

        public string LastResultText
        {
            get { return _result; }
            set { _result = value; RaisePropertyChangedEvent("LastResultText"); }
        }
        private string _result;


        public void Add(AlgorithmEnum algorithm, PathFindResult result)
        {
            PathFindResults.Add(PathFindResultViewModel.From(algorithm, result));
            RaisePropertyChangedEvent("PathFindResults");

            LastResultText = $"Algorithm {algorithm}{Environment.NewLine}" +
                 $"Time : {result.ElapsedMilliseconds} ms{Environment.NewLine}" +
                 $"Amount of visited nodes : {result.VisitedNodes.Count}{Environment.NewLine}" +
                 $"Amount of nodes in solution : {result.Solution.Count}{Environment.NewLine}" +
                 $"Cost of found path : {result.PathWeight}";

        }
    }
}
