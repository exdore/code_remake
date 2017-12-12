namespace MAClassification
{
    partial class Main
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.antsCount = new System.Windows.Forms.NumericUpDown();
            this.convergenceStopValue = new System.Windows.Forms.NumericUpDown();
            this.maxUncoveredCasesCount = new System.Windows.Forms.NumericUpDown();
            this.minNumberPerRule = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.antCountLabel = new System.Windows.Forms.Label();
            this.convergenceCountLabel = new System.Windows.Forms.Label();
            this.uncoveredCasesLabel = new System.Windows.Forms.Label();
            this.casesPerRuleLabel = new System.Windows.Forms.Label();
            this.testButton = new System.Windows.Forms.Button();
            this.trainingPathLabel = new System.Windows.Forms.Label();
            this.testPathLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.entropy = new System.Windows.Forms.RadioButton();
            this.density = new System.Windows.Forms.RadioButton();
            this.euristicFunctionType = new System.Windows.Forms.GroupBox();
            this.PheromonesUpdateMethod = new System.Windows.Forms.GroupBox();
            this.evaporation = new System.Windows.Forms.RadioButton();
            this.normalization = new System.Windows.Forms.RadioButton();
            this.RulesPruningStatus = new System.Windows.Forms.GroupBox();
            this.pruningInactive = new System.Windows.Forms.RadioButton();
            this.pruningActive = new System.Windows.Forms.RadioButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trainingCount = new System.Windows.Forms.Label();
            this.testingCount = new System.Windows.Forms.Label();
            this.trackBarValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.antsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.convergenceStopValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxUncoveredCasesCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minNumberPerRule)).BeginInit();
            this.euristicFunctionType.SuspendLayout();
            this.PheromonesUpdateMethod.SuspendLayout();
            this.RulesPruningStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // antsCount
            // 
            this.antsCount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.antsCount.Location = new System.Drawing.Point(185, 15);
            this.antsCount.Margin = new System.Windows.Forms.Padding(4);
            this.antsCount.Name = "antsCount";
            this.antsCount.Size = new System.Drawing.Size(160, 22);
            this.antsCount.TabIndex = 0;
            this.antsCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // convergenceStopValue
            // 
            this.convergenceStopValue.Location = new System.Drawing.Point(185, 47);
            this.convergenceStopValue.Margin = new System.Windows.Forms.Padding(4);
            this.convergenceStopValue.Name = "convergenceStopValue";
            this.convergenceStopValue.Size = new System.Drawing.Size(160, 22);
            this.convergenceStopValue.TabIndex = 1;
            this.convergenceStopValue.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // maxUncoveredCasesCount
            // 
            this.maxUncoveredCasesCount.Location = new System.Drawing.Point(184, 79);
            this.maxUncoveredCasesCount.Margin = new System.Windows.Forms.Padding(4);
            this.maxUncoveredCasesCount.Name = "maxUncoveredCasesCount";
            this.maxUncoveredCasesCount.Size = new System.Drawing.Size(160, 22);
            this.maxUncoveredCasesCount.TabIndex = 2;
            this.maxUncoveredCasesCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // minNumberPerRule
            // 
            this.minNumberPerRule.Location = new System.Drawing.Point(184, 111);
            this.minNumberPerRule.Margin = new System.Windows.Forms.Padding(4);
            this.minNumberPerRule.Name = "minNumberPerRule";
            this.minNumberPerRule.Size = new System.Drawing.Size(160, 22);
            this.minNumberPerRule.TabIndex = 3;
            this.minNumberPerRule.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(16, 153);
            this.startButton.Margin = new System.Windows.Forms.Padding(4);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 28);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Train";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(16, 386);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1167, 180);
            this.listBox1.TabIndex = 5;
            // 
            // antCountLabel
            // 
            this.antCountLabel.AutoSize = true;
            this.antCountLabel.Location = new System.Drawing.Point(16, 17);
            this.antCountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.antCountLabel.Name = "antCountLabel";
            this.antCountLabel.Size = new System.Drawing.Size(77, 17);
            this.antCountLabel.TabIndex = 6;
            this.antCountLabel.Text = "Ants Count";
            // 
            // convergenceCountLabel
            // 
            this.convergenceCountLabel.AutoSize = true;
            this.convergenceCountLabel.Location = new System.Drawing.Point(16, 49);
            this.convergenceCountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.convergenceCountLabel.Name = "convergenceCountLabel";
            this.convergenceCountLabel.Size = new System.Drawing.Size(133, 17);
            this.convergenceCountLabel.TabIndex = 7;
            this.convergenceCountLabel.Text = "Convergence Count";
            // 
            // uncoveredCasesLabel
            // 
            this.uncoveredCasesLabel.AutoSize = true;
            this.uncoveredCasesLabel.Location = new System.Drawing.Point(16, 81);
            this.uncoveredCasesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.uncoveredCasesLabel.Name = "uncoveredCasesLabel";
            this.uncoveredCasesLabel.Size = new System.Drawing.Size(149, 17);
            this.uncoveredCasesLabel.TabIndex = 8;
            this.uncoveredCasesLabel.Text = "Max Uncovered Cases";
            // 
            // casesPerRuleLabel
            // 
            this.casesPerRuleLabel.AutoSize = true;
            this.casesPerRuleLabel.Location = new System.Drawing.Point(16, 113);
            this.casesPerRuleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.casesPerRuleLabel.Name = "casesPerRuleLabel";
            this.casesPerRuleLabel.Size = new System.Drawing.Size(132, 17);
            this.casesPerRuleLabel.TabIndex = 9;
            this.casesPerRuleLabel.Text = "Min Cases Per Rule";
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(16, 242);
            this.testButton.Margin = new System.Windows.Forms.Padding(4);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(100, 28);
            this.testButton.TabIndex = 12;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // trainingPathLabel
            // 
            this.trainingPathLabel.AutoSize = true;
            this.trainingPathLabel.Location = new System.Drawing.Point(16, 201);
            this.trainingPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.trainingPathLabel.Name = "trainingPathLabel";
            this.trainingPathLabel.Size = new System.Drawing.Size(0, 17);
            this.trainingPathLabel.TabIndex = 14;
            // 
            // testPathLabel
            // 
            this.testPathLabel.AutoSize = true;
            this.testPathLabel.Location = new System.Drawing.Point(16, 290);
            this.testPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.testPathLabel.Name = "testPathLabel";
            this.testPathLabel.Size = new System.Drawing.Size(0, 17);
            this.testPathLabel.TabIndex = 16;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(184, 326);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 22);
            this.textBox1.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 330);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "Cases With Wrong Class";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(380, 153);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 28);
            this.button3.TabIndex = 19;
            this.button3.Text = "Full Set";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(444, 253);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 17);
            this.label2.TabIndex = 21;
            // 
            // entropy
            // 
            this.entropy.AutoSize = true;
            this.entropy.Checked = true;
            this.entropy.Location = new System.Drawing.Point(8, 37);
            this.entropy.Margin = new System.Windows.Forms.Padding(4);
            this.entropy.Name = "entropy";
            this.entropy.Size = new System.Drawing.Size(138, 21);
            this.entropy.TabIndex = 22;
            this.entropy.TabStop = true;
            this.entropy.Text = "Через энтропию";
            this.entropy.UseVisualStyleBackColor = true;
            // 
            // density
            // 
            this.density.AutoSize = true;
            this.density.Location = new System.Drawing.Point(8, 65);
            this.density.Margin = new System.Windows.Forms.Padding(4);
            this.density.Name = "density";
            this.density.Size = new System.Drawing.Size(142, 21);
            this.density.TabIndex = 23;
            this.density.Text = "Через плотность";
            this.density.UseVisualStyleBackColor = true;
            // 
            // euristicFunctionType
            // 
            this.euristicFunctionType.Controls.Add(this.density);
            this.euristicFunctionType.Controls.Add(this.entropy);
            this.euristicFunctionType.Location = new System.Drawing.Point(380, 10);
            this.euristicFunctionType.Margin = new System.Windows.Forms.Padding(4);
            this.euristicFunctionType.Name = "euristicFunctionType";
            this.euristicFunctionType.Padding = new System.Windows.Forms.Padding(4);
            this.euristicFunctionType.Size = new System.Drawing.Size(175, 94);
            this.euristicFunctionType.TabIndex = 26;
            this.euristicFunctionType.TabStop = false;
            this.euristicFunctionType.Text = "Эвристическая функция";
            // 
            // PheromonesUpdateMethod
            // 
            this.PheromonesUpdateMethod.Controls.Add(this.evaporation);
            this.PheromonesUpdateMethod.Controls.Add(this.normalization);
            this.PheromonesUpdateMethod.Location = new System.Drawing.Point(583, 10);
            this.PheromonesUpdateMethod.Margin = new System.Windows.Forms.Padding(4);
            this.PheromonesUpdateMethod.Name = "PheromonesUpdateMethod";
            this.PheromonesUpdateMethod.Padding = new System.Windows.Forms.Padding(4);
            this.PheromonesUpdateMethod.Size = new System.Drawing.Size(175, 94);
            this.PheromonesUpdateMethod.TabIndex = 27;
            this.PheromonesUpdateMethod.TabStop = false;
            this.PheromonesUpdateMethod.Text = "Пересчет феромона";
            // 
            // evaporation
            // 
            this.evaporation.AutoSize = true;
            this.evaporation.Location = new System.Drawing.Point(8, 65);
            this.evaporation.Margin = new System.Windows.Forms.Padding(4);
            this.evaporation.Name = "evaporation";
            this.evaporation.Size = new System.Drawing.Size(102, 21);
            this.evaporation.TabIndex = 23;
            this.evaporation.Text = "Испарение";
            this.evaporation.UseVisualStyleBackColor = true;
            // 
            // normalization
            // 
            this.normalization.AutoSize = true;
            this.normalization.Checked = true;
            this.normalization.Location = new System.Drawing.Point(8, 37);
            this.normalization.Margin = new System.Windows.Forms.Padding(4);
            this.normalization.Name = "normalization";
            this.normalization.Size = new System.Drawing.Size(127, 21);
            this.normalization.TabIndex = 22;
            this.normalization.TabStop = true;
            this.normalization.Text = "Нормализация";
            this.normalization.UseVisualStyleBackColor = true;
            // 
            // RulesPruningStatus
            // 
            this.RulesPruningStatus.Controls.Add(this.pruningInactive);
            this.RulesPruningStatus.Controls.Add(this.pruningActive);
            this.RulesPruningStatus.Location = new System.Drawing.Point(788, 10);
            this.RulesPruningStatus.Margin = new System.Windows.Forms.Padding(4);
            this.RulesPruningStatus.Name = "RulesPruningStatus";
            this.RulesPruningStatus.Padding = new System.Windows.Forms.Padding(4);
            this.RulesPruningStatus.Size = new System.Drawing.Size(175, 94);
            this.RulesPruningStatus.TabIndex = 28;
            this.RulesPruningStatus.TabStop = false;
            this.RulesPruningStatus.Text = "Упрощение правил";
            // 
            // pruningInactive
            // 
            this.pruningInactive.AutoSize = true;
            this.pruningInactive.Location = new System.Drawing.Point(8, 65);
            this.pruningInactive.Margin = new System.Windows.Forms.Padding(4);
            this.pruningInactive.Name = "pruningInactive";
            this.pruningInactive.Size = new System.Drawing.Size(54, 21);
            this.pruningInactive.TabIndex = 23;
            this.pruningInactive.Text = "Нет";
            this.pruningInactive.UseVisualStyleBackColor = true;
            // 
            // pruningActive
            // 
            this.pruningActive.AutoSize = true;
            this.pruningActive.Checked = true;
            this.pruningActive.Location = new System.Drawing.Point(8, 37);
            this.pruningActive.Margin = new System.Windows.Forms.Padding(4);
            this.pruningActive.Name = "pruningActive";
            this.pruningActive.Size = new System.Drawing.Size(48, 21);
            this.pruningActive.TabIndex = 22;
            this.pruningActive.TabStop = true;
            this.pruningActive.Text = "Да";
            this.pruningActive.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 594);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1167, 155);
            this.dataGridView1.TabIndex = 29;
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(642, 153);
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(153, 56);
            this.trackBar1.TabIndex = 30;
            this.trackBar1.Value = 5;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // trainingCount
            // 
            this.trainingCount.AutoSize = true;
            this.trainingCount.Location = new System.Drawing.Point(145, 159);
            this.trainingCount.Name = "trainingCount";
            this.trainingCount.Size = new System.Drawing.Size(0, 17);
            this.trainingCount.TabIndex = 31;
            // 
            // testingCount
            // 
            this.testingCount.AutoSize = true;
            this.testingCount.Location = new System.Drawing.Point(145, 248);
            this.testingCount.Name = "testingCount";
            this.testingCount.Size = new System.Drawing.Size(0, 17);
            this.testingCount.TabIndex = 32;
            // 
            // trackBarValue
            // 
            this.trackBarValue.AutoSize = true;
            this.trackBarValue.Location = new System.Drawing.Point(802, 163);
            this.trackBarValue.Name = "trackBarValue";
            this.trackBarValue.Size = new System.Drawing.Size(0, 17);
            this.trackBarValue.TabIndex = 33;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1805, 761);
            this.Controls.Add(this.trackBarValue);
            this.Controls.Add(this.testingCount);
            this.Controls.Add(this.trainingCount);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.RulesPruningStatus);
            this.Controls.Add(this.PheromonesUpdateMethod);
            this.Controls.Add(this.euristicFunctionType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.testPathLabel);
            this.Controls.Add(this.trainingPathLabel);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.casesPerRuleLabel);
            this.Controls.Add(this.uncoveredCasesLabel);
            this.Controls.Add(this.convergenceCountLabel);
            this.Controls.Add(this.antCountLabel);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.minNumberPerRule);
            this.Controls.Add(this.maxUncoveredCasesCount);
            this.Controls.Add(this.convergenceStopValue);
            this.Controls.Add(this.antsCount);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Ants Classifier";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.antsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.convergenceStopValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxUncoveredCasesCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minNumberPerRule)).EndInit();
            this.euristicFunctionType.ResumeLayout(false);
            this.euristicFunctionType.PerformLayout();
            this.PheromonesUpdateMethod.ResumeLayout(false);
            this.PheromonesUpdateMethod.PerformLayout();
            this.RulesPruningStatus.ResumeLayout(false);
            this.RulesPruningStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown antsCount;
        private System.Windows.Forms.NumericUpDown convergenceStopValue;
        private System.Windows.Forms.NumericUpDown maxUncoveredCasesCount;
        private System.Windows.Forms.NumericUpDown minNumberPerRule;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label antCountLabel;
        private System.Windows.Forms.Label convergenceCountLabel;
        private System.Windows.Forms.Label uncoveredCasesLabel;
        private System.Windows.Forms.Label casesPerRuleLabel;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.Label trainingPathLabel;
        private System.Windows.Forms.Label testPathLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton entropy;
        private System.Windows.Forms.RadioButton density;
        private System.Windows.Forms.GroupBox euristicFunctionType;
        private System.Windows.Forms.GroupBox PheromonesUpdateMethod;
        private System.Windows.Forms.RadioButton evaporation;
        private System.Windows.Forms.RadioButton normalization;
        private System.Windows.Forms.GroupBox RulesPruningStatus;
        private System.Windows.Forms.RadioButton pruningInactive;
        private System.Windows.Forms.RadioButton pruningActive;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label trainingCount;
        private System.Windows.Forms.Label testingCount;
        private System.Windows.Forms.Label trackBarValue;
    }
}

