using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppN1.ViewModel;

namespace WpfAppN1
{
    /// <summary>
    /// Interaction logic for ResultOverview.xaml
    /// </summary>
    public partial class ResultOverview : Window
    {
        public ResultOverview()
        {
            InitializeComponent();
            
        }

        private List<PathFindResultViewModel> _pathFindResultViewModel;


        public void SetPathFindResults(List<PathFindResultViewModel> pathFindResultViewModels)
        {
            lvResultOverview.ItemsSource = pathFindResultViewModels;
            _pathFindResultViewModel = pathFindResultViewModels;
        }
        

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "csv files (*.csv)|*.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                Save(saveFileDialog.FileName);
            }

        }

        private void Save(string fileName)
        {
            StringBuilder sb = new StringBuilder();

            var sep = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;


            // header
            sb.AppendLine($"algorithm{sep}Time (ms){sep}solution{sep}visited{sep}cost");

            foreach(var result in _pathFindResultViewModel)
            {
                sb.AppendLine($"{result.Algorithm}{sep}{result.ElapsedMilliseconds}{sep}{result.NodesInSolution}{sep}{result.NodesVisited}{sep}{result.Cost}");
            }

            File.WriteAllText(fileName, sb.ToString());         
        }
    }
}
