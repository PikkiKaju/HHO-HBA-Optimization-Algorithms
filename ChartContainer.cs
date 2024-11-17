using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace Zastosowania_Sztucznej_Inteligencji
{
    public partial class ChartContainer : UserControl
    {
        private List<Chart> charts = new List<Chart>();
        public ChartContainer()
        {
            InitializeComponent();

            charts.Add(chart1);
            charts.Add(chart2);
            charts.Add(chart3);
            charts.Add(chart4);
            charts.Add(chart5);
            charts.Add(chart6);

            foreach (var chart in charts)
            {
                chart.ChartAreas[0].AxisY.Title = "Wartość funkcji dopasowania";
                chart.ChartAreas[0].AxisX.Title = "Wielkość populacji";
                chart.Series.Add("Ilość iteracji");
                chart.Series[1].ChartType = SeriesChartType.Line;
                chart.Legends[0].Enabled = false;

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void loadData(string algorithmName, List<TestResults> results)
        {
            textBox1.Text = algorithmName;

            int i = 0;
            var groupedByFunctions = results.GroupBy(r => r.Function.Name);
            textBox1.Text = algorithmName;
            foreach (var functionResults in groupedByFunctions)
            {
                charts[i].Titles.Add(functionResults.First().Function.Name);
                foreach (var testResult in functionResults)
                {
                    charts[i].Series[0].Points.AddXY(testResult.PopulationSize, testResult.ResultF);
                    charts[i].Series[1].Points.AddXY(testResult.Iterations + 80 * i, testResult.ResultF);
                }
                i++;
            }
        }

        private void ChartContainer_Load(object sender, EventArgs e)
        {
            
        }

        private void ChartContainer_Load_1(object sender, EventArgs e)
        {
            
        }
    }
}
