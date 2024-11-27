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
            button1 = new Button();
            textBox1 = new TextBox();
            chartContainer2 = new ChartContainer();
            chartContainer1 = new ChartContainer();
            SuspendLayout();
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
            // chartContainer2
            // 
            chartContainer2.AutoScroll = true;
            chartContainer2.Location = new Point(487, 761);
            chartContainer2.Name = "chartContainer2";
            chartContainer2.Size = new Size(1326, 701);
            chartContainer2.TabIndex = 4;
            // 
            // chartContainer1
            // 
            chartContainer1.AutoScroll = true;
            chartContainer1.Location = new Point(487, 12);
            chartContainer1.Name = "chartContainer1";
            chartContainer1.Size = new Size(1326, 701);
            chartContainer1.TabIndex = 6;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1865, 873);
            Controls.Add(chartContainer1);
            Controls.Add(chartContainer2);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "MainForm";
            Text = "HHO and HBA algorithms testing";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private TextBox textBox1;
        private ChartContainer chartContainer2;
        private ChartContainer chartContainer1;
    }
}