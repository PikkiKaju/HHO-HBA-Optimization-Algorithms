namespace Zastosowania_Sztucznej_Inteligencji
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            chartHHO = new System.Windows.Forms.DataVisualization.Charting.Chart();
            button1 = new Button();
            textBox1 = new TextBox();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)chartHHO).BeginInit();
            SuspendLayout();
            // 
            // chartHHO
            // 
            chartArea1.Name = "ChartArea1";
            chartHHO.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            chartHHO.Legends.Add(legend1);
            chartHHO.Location = new Point(500, 12);
            chartHHO.Name = "chartHHO";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chartHHO.Series.Add(series1);
            chartHHO.Size = new Size(577, 302);
            chartHHO.TabIndex = 0;
            chartHHO.Text = "chart1";
            chartHHO.Click += chart1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(34, 41);
            button1.Name = "button1";
            button1.Size = new Size(120, 43);
            button1.TabIndex = 1;
            button1.Text = "Uruchom testy";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.AcceptsReturn = true;
            textBox1.AcceptsTab = true;
            textBox1.Location = new Point(34, 124);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(431, 644);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(500, 338);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(577, 23);
            comboBox1.TabIndex = 5;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(500, 470);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(577, 23);
            comboBox2.TabIndex = 6;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1341, 797);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(chartHHO);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "MainForm";
            Text = "HHO and HBA algorithms testing";
            ((System.ComponentModel.ISupportInitialize)chartHHO).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartHHO;
        private Button button1;
        private TextBox textBox1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
    }
}