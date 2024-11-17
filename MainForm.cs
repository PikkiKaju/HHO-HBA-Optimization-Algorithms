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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Zastosowania_Sztucznej_Inteligencji
{
    public partial class MainForm : Form
    {
        // Get the algorithms' test results
        private RunAlgorithmsResult testsData;

        private string textBoxString = "";
        
        public MainForm()
        {
            InitializeComponent();

            // Populate ComboBox1
            comboBox1.Items.Add(new ChartContainer());
            comboBox1.Items.Add(new ChartContainer());
            comboBox1.Items.Add(new ChartContainer());
            comboBox1.Items.Add(new ChartContainer());
            comboBox1.Items.Add(new ChartContainer());
            comboBox1.Items.Add(new ChartContainer());

            // Populate ComboBox2
            comboBox2.Items.Add(new ChartContainer());
            comboBox2.Items.Add(new ChartContainer());
            comboBox2.Items.Add(new ChartContainer());
            comboBox2.Items.Add(new ChartContainer());
            comboBox2.Items.Add(new ChartContainer());
            comboBox2.Items.Add(new ChartContainer());
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (testsData is not null) testsData.Clear();

            testsData = RunAlgorithms.Run();
            int i = 0;

            foreach (var testResults in testsData.TestResultsList)
            {
                i++;
                textBox1.AppendText(testResults.ToString(5));
            }

            textBox1.AppendText($"\r\n---------------------------------------------------\r\n");
            textBox1.AppendText($"\r\nBest parameters set for functions best performance:\r\n");

            foreach (var bestFunction in testsData.BestFunctionsList)
            {
                textBox1.AppendText(bestFunction.ToString(5));
            }
            textBox1.AppendText($"\r\nNumber of tests completed: {i}");

            PlotResults(testsData);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void PlotResults(RunAlgorithmsResult testsData)
        {
            // Clear the chart
            chartHHO.Series.Clear();

            BindingList<TestResults> testsDataBL = new BindingList<TestResults>();
            foreach (var result in testsData.TestResultsList)
            {
                testsDataBL.Add(result);
            }

            int algorithmNr = 0;
            int functionNr = 0;

            foreach (var algorithmGroup in testsDataBL.GroupBy(r => r.Algorithm.Name))
            {
                var comboBox = algorithmNr == 0 ? comboBox1 : comboBox2;
                foreach (var functionGroup in algorithmGroup.GroupBy(r => r.Function.Name))
                {
                    ChartContainer chartContainer = (ChartContainer)comboBox.Items[1];
                    Chart chart = chartContainer.chart;
                    foreach (var popSizeGroup in functionGroup.GroupBy(r => r.PopulationSize))
                    {
                        foreach (var iterGroup in popSizeGroup.GroupBy(r => r.Iterations))
                        {
                            var series = new Series($"Algorithm: {algorithmGroup.Key}, Function: {functionGroup.Key}, PopSize: {popSizeGroup.Key}, Iter: {iterGroup.Key}");
                            
                            foreach (var result in iterGroup)
                            {
                                chart.Series[0].Points.AddXY(result.Mean, result.Iterations);
                            }
                        }
                    }
                    chart.ChartAreas[0].AxisX.Title = "Mean";
                    chart.ChartAreas[0].AxisY.Title = "Iterations";

                    functionNr++;
                }
                algorithmNr++;
            }
        }
    }
}
