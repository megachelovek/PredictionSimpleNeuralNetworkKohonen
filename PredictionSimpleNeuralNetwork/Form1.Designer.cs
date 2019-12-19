namespace PredictionSimpleNeuralNetwork
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonStart = new System.Windows.Forms.Button();
            this.chartData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBoxWindow = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.countOfNeuron = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxEpsilon = new System.Windows.Forms.TextBox();
            this.checkBoxNormalize = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonStart.Location = new System.Drawing.Point(934, 238);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(248, 58);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Загрузить данные и начать";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // chartData
            // 
            chartArea2.Name = "ChartArea1";
            this.chartData.ChartAreas.Add(chartArea2);
            this.chartData.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.chartData.IsSoftShadows = false;
            this.chartData.Location = new System.Drawing.Point(18, 18);
            this.chartData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chartData.Name = "chartData";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.chartData.Series.Add(series2);
            this.chartData.Size = new System.Drawing.Size(908, 655);
            this.chartData.TabIndex = 3;
            this.chartData.Text = "chart1";
            // 
            // textBoxWindow
            // 
            this.textBoxWindow.Location = new System.Drawing.Point(934, 43);
            this.textBoxWindow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxWindow.Name = "textBoxWindow";
            this.textBoxWindow.Size = new System.Drawing.Size(148, 26);
            this.textBoxWindow.TabIndex = 4;
            this.textBoxWindow.Text = "6";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(930, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Окно, количество значений";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(930, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Количество нейронов";
            // 
            // countOfNeuron
            // 
            this.countOfNeuron.Location = new System.Drawing.Point(934, 103);
            this.countOfNeuron.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.countOfNeuron.Name = "countOfNeuron";
            this.countOfNeuron.Size = new System.Drawing.Size(148, 26);
            this.countOfNeuron.TabIndex = 7;
            this.countOfNeuron.Text = "12";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(934, 138);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Точность, эпсилон";
            // 
            // textBoxEpsilon
            // 
            this.textBoxEpsilon.Location = new System.Drawing.Point(934, 163);
            this.textBoxEpsilon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxEpsilon.Name = "textBoxEpsilon";
            this.textBoxEpsilon.Size = new System.Drawing.Size(148, 26);
            this.textBoxEpsilon.TabIndex = 9;
            this.textBoxEpsilon.Text = "0.008";
            // 
            // checkBoxNormalize
            // 
            this.checkBoxNormalize.AutoSize = true;
            this.checkBoxNormalize.Location = new System.Drawing.Point(934, 203);
            this.checkBoxNormalize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxNormalize.Name = "checkBoxNormalize";
            this.checkBoxNormalize.Size = new System.Drawing.Size(157, 24);
            this.checkBoxNormalize.TabIndex = 10;
            this.checkBoxNormalize.Text = "Нормализовать";
            this.checkBoxNormalize.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.checkBoxNormalize);
            this.Controls.Add(this.textBoxEpsilon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.countOfNeuron);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxWindow);
            this.Controls.Add(this.chartData);
            this.Controls.Add(this.buttonStart);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Прогнозирующая сеть Кохонена";
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartData;
        private System.Windows.Forms.TextBox textBoxWindow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox countOfNeuron;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxEpsilon;
        private System.Windows.Forms.CheckBox checkBoxNormalize;
    }
}

