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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void loadData(string algorithmName, List<TestResults> results)
        {
            // Name of the charts' form
            textBox1.Text = algorithmName;

            int i = 0;
            var groupedByFunctions = results.GroupBy(r => r.Function.Name);
            foreach (var functionResults in groupedByFunctions)
            {
                // Configure chart area
                var chartArea = new ChartArea("MainArea");

                // Add a title to the chart area
                var chartTitle = new Title
                {
                    Text = functionResults.First().Function.Name,
                    Docking = Docking.Top,
                    Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                    ForeColor = System.Drawing.Color.Black
                };

                // Bottom X-Axis (Population Size)
                chartArea.AxisX.Title = "Wielkość populacji";
                chartArea.AxisX.IsLabelAutoFit = true;

                // Top X-Axis (Iterations)
                chartArea.AxisX2.Title = "Ilość iteracji";
                chartArea.AxisX2.Enabled = AxisEnabled.True;
                chartArea.AxisX2.IsLabelAutoFit = true;

                // Y-Axis (Fitness Value)
                chartArea.AxisY.Title = "Wartość funkcji dopasowania";
                
                // Add chart area to the chart
                charts[i].ChartAreas.Clear();
                charts[i].Titles.Clear();
                charts[i].ChartAreas.Add(chartArea);
                charts[i].Titles.Add(chartTitle);

                // Configure series
                var series = new Series("FitnessValue")
                {
                    ChartType = SeriesChartType.Line,
                    XValueType = ChartValueType.Int32,
                    YValueType = ChartValueType.Double
                };

                int itLabelCount = 0;

                foreach (var testResult in functionResults)
                {
                    // Map PopulationSize to X and FitnessValue to Y
                    series.Points.AddXY(itLabelCount, testResult.ResultF < 10 ? testResult.ResultF : 10);

                    // Add Iterations as a label for the bottom X-Axis
                    charts[i].ChartAreas["MainArea"].AxisX.CustomLabels.Add(0, 5, "10");
                    charts[i].ChartAreas["MainArea"].AxisX.CustomLabels.Add(6, 11, "20");
                    charts[i].ChartAreas["MainArea"].AxisX.CustomLabels.Add(12, 17, "40");
                    charts[i].ChartAreas["MainArea"].AxisX.CustomLabels.Add(19, 23, "80");
                    charts[i].ChartAreas["MainArea"].AxisX2.CustomLabels.Add(0, 5, "80");
                    charts[i].ChartAreas["MainArea"].AxisX2.CustomLabels.Add(6, 11, "80");
                    charts[i].ChartAreas["MainArea"].AxisX2.CustomLabels.Add(12, 17, "80");
                    charts[i].ChartAreas["MainArea"].AxisX2.CustomLabels.Add(19, 23, "80");

                    itLabelCount++;
                }

                // Add the series to the chart
                charts[i].Series.Clear();
                charts[i].Series.Add(series);
                
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
