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
        private RunAlgorithmsResult testsData = new RunAlgorithmsResult { TestResultsList = new List<TestResults>(), BestFunctionsList = new List<BestFunction>() };

        private string textBoxString = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (testsData is not null)
            {
                testsData.Clear();
                stopButton.Enabled = true;

                testsData = await RunAlgorithms.RunAsync();
                stopButton.Enabled = false;

                PrintResults(testsData);
                PlotResults(testsData);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (RunAlgorithms.IsRunning()) 
            {
                textBox1.AppendText($"\r\n---------------------------------------------------\r\n");
                textBox1.AppendText($"\r\nCalculations aborted.\r\n");
                textBox1.AppendText($"\r\n---------------------------------------------------\r\n");
                RunAlgorithms.StopAsync();
            }
        }

        private void PrintResults(RunAlgorithmsResult testsData) 
        {
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
        }

        private void PlotResults(RunAlgorithmsResult testsData)
        {
            BindingList<TestResults> testsDataBL = new BindingList<TestResults>();
            foreach (var result in testsData.TestResultsList)
            {
                testsDataBL.Add(result);
            }

            int algorithmNr = 0;

            foreach (var algorithmGroup in testsDataBL.GroupBy(r => r.Algorithm.Name))
            {
                var chartContainer = algorithmNr == 0 ? chartContainer1 : chartContainer2;

                chartContainer.loadData(algorithmGroup.Key, algorithmGroup.ToList());

                algorithmNr++;
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
