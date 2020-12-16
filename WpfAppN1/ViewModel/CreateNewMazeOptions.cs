using System;
using System.Collections.Generic;
using System.Text;

namespace WpfAppN1.ViewModel
{
    public class CreateNewMazeOptions
    {
        public int NumberRows { get; set; }
        public int NumberColumns { get; set; }
        public bool CreateBlankMaze { get; set; }


        public CreateNewMazeOptions()
        {
            // default
            NumberRows = 5;
            NumberColumns = 5;
        }
    }
}
