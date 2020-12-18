using Microsoft.Win32;
using NodeModel.MazeGraph;
using PathfindAlgorithm;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppN1.Controller;
using WpfAppN1.ViewModel;

namespace WpfAppN1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MazeGraphViewModel MazeGraphViewModel { get; set; }
        public MazeGraphViewController MazeGraphViewController { get; set; }

        public ViewOptions ViewOptions { get; set; }


        public PathFindResultController PathFindResultController { get; set; }

        public MainWindow()
        {
            ViewOptions = new ViewOptions();
            PathFindResultController = new PathFindResultController();
            MazeGraphViewModel = new MazeGraphViewModel()
            {
                ViewOptions = ViewOptions
            };

            InitializeComponent();

            DataContext = this;
            MazeGraphViewController = new MazeGraphViewController(MazeGraphViewModel);
        }


        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {

            var options = new CreateNewMazeOptions();

            // Instantiate the dialog box
            var dlg = new CreateNewMazeDialog
            {
                // Configure the dialog box
                Owner = this,
                Options = options
            };

            // Open the dialog box modally
            var dialogResult = dlg.ShowDialog();

            //  process the result
            if (dialogResult.HasValue && dialogResult.Value == true)
            {
                MazeGraphViewController.Create(options.NumberColumns, options.NumberRows, options.CreateBlankMaze);
            }
        }


        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var border = (Border)sender;
            var positionClicked = e.GetPosition(border);
            ToggleConnection(border, positionClicked);
        }

        private void ToggleConnection(Border border, Point positionClicked)
        {
            // margin for clicking at edge of border
            int clickMargin = 5;

            var height = border.ActualHeight;
            var with = border.ActualWidth;

            var distanceTop = positionClicked.Y;
            var distanceRight = with - positionClicked.X;
            var distanceBottom = height - positionClicked.Y;
            var distanceLeft = positionClicked.X;


            var node = border.DataContext as NodeViewModel;

            // top 
            if (distanceTop <= clickMargin)
            {
                MazeGraphViewModel.ToggleConnection(node, node.GetNeighbour(Direction.Top));
                return;
            }

            if (distanceRight <= clickMargin)
            {
                MazeGraphViewModel.ToggleConnection(node, node.GetNeighbour(Direction.Right));
                return;
            }

            if (distanceBottom <= clickMargin)
            {
                MazeGraphViewModel.ToggleConnection(node, node.GetNeighbour(Direction.Bottom));
                return;
            }

            if (distanceLeft <= clickMargin)
            {
                MazeGraphViewModel.ToggleConnection(node, node.GetNeighbour(Direction.Left));
                return;
            }

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "json files (*.json)|*.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                MazeGraphViewController.Save(saveFileDialog.FileName);
            }

        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {

            var openFileDialog = new OpenFileDialog()
            {
                Filter = "json files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                MazeGraphViewController.Open(openFileDialog.FileName);
            }

        }

        private void MenuItem_SetStartClick(object sender, RoutedEventArgs e)
        {

            var node = ((MenuItem)sender).DataContext as NodeViewModel;
            MazeGraphViewController.SetStartNode(node);

        }

        private void MenuItem_SetEndClick(object sender, RoutedEventArgs e)
        {
            var node = ((MenuItem)sender).DataContext as NodeViewModel;
            MazeGraphViewController.SetEndNode(node);

        }

        private void ButtonCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (MazeGraphViewModel.NodeList == null || MazeGraphViewModel.NodeList.Count ==0)
            {
                MessageBox.Show("Load or generate a maze before calculating", "Maze runner", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MazeGraphViewController.Calculate(ViewOptions.Algorithm);

            PathFindResultController.Add(ViewOptions.Algorithm, MazeGraphViewModel.PathFindResult);

        }

        private void buttonResultOverview_Click(object sender, RoutedEventArgs e)
        {

            // Instantiate the dialog box
            var dlg = new ResultOverview()
            {
                // Configure the dialog box
                Owner = this,                
            };

            //dlg.SetPathFindResults(PathFindResultController.PathFindResults);
            dlg.SetPathFindResults(PathFindResultController.PathFindResults);

            // Open the dialog box modally
            dlg.ShowDialog();

        }
    }
}
