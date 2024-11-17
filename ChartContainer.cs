using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Zastosowania_Sztucznej_Inteligencji
{
    public partial class ChartContainer : UserControl
    {
        public Chart chart { get; set; }
        public ChartContainer()
        {
            InitializeComponent();

            chart = chart1;
        }
    }
}
