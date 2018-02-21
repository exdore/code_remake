using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MAClassification
{
    partial class Main
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private IContainer components = null;

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
            this.antsCount = new NumericUpDown();
            this.convergenceStopValue = new NumericUpDown();
            this.maxUncoveredCasesCount = new NumericUpDown();
            this.minNumberPerRule = new NumericUpDown();
            this.startButton = new Button();
            this.listBox1 = new ListBox();
            this.antCountLabel = new Label();
            this.convergenceCountLabel = new Label();
            this.uncoveredCasesLabel = new Label();
            this.casesPerRuleLabel = new Label();
            this.testButton = new Button();
            this.trainingPathLabel = new Label();
            this.testPathLabel = new Label();
            this.textBox1 = new TextBox();
            this.label1 = new Label();
            this.button3 = new Button();
            this.label2 = new Label();
            this.entropy = new RadioButton();
            this.density = new RadioButton();
            this.EuristicFunction = new GroupBox();
            this.PheromonesUpdateMethod = new GroupBox();
            this.evaporation = new RadioButton();
            this.normalization = new RadioButton();
            this.RulesPruningStatus = new GroupBox();
            this.pruningInactive = new RadioButton();
            this.pruningActive = new RadioButton();
            this.dataGridView1 = new DataGridView();
            this.trackBar1 = new TrackBar();
            this.trainingCount = new Label();
            this.testingCount = new Label();
            this.trackBarValue = new Label();
            this.TrainingSetDivideMethod = new GroupBox();
            this.crossValidation = new RadioButton();
            this.byClass = new RadioButton();
            ((ISupportInitialize)(this.antsCount)).BeginInit();
            ((ISupportInitialize)(this.convergenceStopValue)).BeginInit();
            ((ISupportInitialize)(this.maxUncoveredCasesCount)).BeginInit();
            ((ISupportInitialize)(this.minNumberPerRule)).BeginInit();
            this.EuristicFunction.SuspendLayout();
            this.PheromonesUpdateMethod.SuspendLayout();
            this.RulesPruningStatus.SuspendLayout();
            ((ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((ISupportInitialize)(this.trackBar1)).BeginInit();
            this.TrainingSetDivideMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // antsCount
            // 
            this.antsCount.ForeColor = SystemColors.WindowText;
            this.antsCount.Location = new Point(185, 15);
            this.antsCount.Margin = new Padding(4);
            this.antsCount.Name = "antsCount";
            this.antsCount.Size = new Size(160, 22);
            this.antsCount.TabIndex = 0;
            this.antsCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // convergenceStopValue
            // 
            this.convergenceStopValue.Location = new Point(185, 47);
            this.convergenceStopValue.Margin = new Padding(4);
            this.convergenceStopValue.Name = "convergenceStopValue";
            this.convergenceStopValue.Size = new Size(160, 22);
            this.convergenceStopValue.TabIndex = 1;
            this.convergenceStopValue.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // maxUncoveredCasesCount
            // 
            this.maxUncoveredCasesCount.Location = new Point(184, 79);
            this.maxUncoveredCasesCount.Margin = new Padding(4);
            this.maxUncoveredCasesCount.Name = "maxUncoveredCasesCount";
            this.maxUncoveredCasesCount.Size = new Size(160, 22);
            this.maxUncoveredCasesCount.TabIndex = 2;
            this.maxUncoveredCasesCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // minNumberPerRule
            // 
            this.minNumberPerRule.Location = new Point(184, 111);
            this.minNumberPerRule.Margin = new Padding(4);
            this.minNumberPerRule.Name = "minNumberPerRule";
            this.minNumberPerRule.Size = new Size(160, 22);
            this.minNumberPerRule.TabIndex = 3;
            this.minNumberPerRule.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // startButton
            // 
            this.startButton.Location = new Point(16, 153);
            this.startButton.Margin = new Padding(4);
            this.startButton.Name = "startButton";
            this.startButton.Size = new Size(100, 28);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Train";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new EventHandler(this.startButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new Point(16, 386);
            this.listBox1.Margin = new Padding(4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new Size(1167, 180);
            this.listBox1.TabIndex = 5;
            // 
            // antCountLabel
            // 
            this.antCountLabel.AutoSize = true;
            this.antCountLabel.Location = new Point(16, 17);
            this.antCountLabel.Margin = new Padding(4, 0, 4, 0);
            this.antCountLabel.Name = "antCountLabel";
            this.antCountLabel.Size = new Size(77, 17);
            this.antCountLabel.TabIndex = 6;
            this.antCountLabel.Text = "Ants Count";
            // 
            // convergenceCountLabel
            // 
            this.convergenceCountLabel.AutoSize = true;
            this.convergenceCountLabel.Location = new Point(16, 49);
            this.convergenceCountLabel.Margin = new Padding(4, 0, 4, 0);
            this.convergenceCountLabel.Name = "convergenceCountLabel";
            this.convergenceCountLabel.Size = new Size(133, 17);
            this.convergenceCountLabel.TabIndex = 7;
            this.convergenceCountLabel.Text = "Convergence Count";
            // 
            // uncoveredCasesLabel
            // 
            this.uncoveredCasesLabel.AutoSize = true;
            this.uncoveredCasesLabel.Location = new Point(16, 81);
            this.uncoveredCasesLabel.Margin = new Padding(4, 0, 4, 0);
            this.uncoveredCasesLabel.Name = "uncoveredCasesLabel";
            this.uncoveredCasesLabel.Size = new Size(149, 17);
            this.uncoveredCasesLabel.TabIndex = 8;
            this.uncoveredCasesLabel.Text = "Max Uncovered Cases";
            // 
            // casesPerRuleLabel
            // 
            this.casesPerRuleLabel.AutoSize = true;
            this.casesPerRuleLabel.Location = new Point(16, 113);
            this.casesPerRuleLabel.Margin = new Padding(4, 0, 4, 0);
            this.casesPerRuleLabel.Name = "casesPerRuleLabel";
            this.casesPerRuleLabel.Size = new Size(132, 17);
            this.casesPerRuleLabel.TabIndex = 9;
            this.casesPerRuleLabel.Text = "Min Cases Per Rule";
            // 
            // testButton
            // 
            this.testButton.Location = new Point(16, 242);
            this.testButton.Margin = new Padding(4);
            this.testButton.Name = "testButton";
            this.testButton.Size = new Size(100, 28);
            this.testButton.TabIndex = 12;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new EventHandler(this.testButton_Click);
            // 
            // trainingPathLabel
            // 
            this.trainingPathLabel.AutoSize = true;
            this.trainingPathLabel.Location = new Point(16, 201);
            this.trainingPathLabel.Margin = new Padding(4, 0, 4, 0);
            this.trainingPathLabel.Name = "trainingPathLabel";
            this.trainingPathLabel.Size = new Size(0, 17);
            this.trainingPathLabel.TabIndex = 14;
            // 
            // testPathLabel
            // 
            this.testPathLabel.AutoSize = true;
            this.testPathLabel.Location = new Point(16, 290);
            this.testPathLabel.Margin = new Padding(4, 0, 4, 0);
            this.testPathLabel.Name = "testPathLabel";
            this.testPathLabel.Size = new Size(0, 17);
            this.testPathLabel.TabIndex = 16;
            // 
            // textBox1
            // 
            this.textBox1.Location = new Point(184, 326);
            this.textBox1.Margin = new Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(132, 22);
            this.textBox1.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 330);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(163, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "Cases With Wrong Class";
            // 
            // button3
            // 
            this.button3.Location = new Point(380, 153);
            this.button3.Margin = new Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new Size(100, 28);
            this.button3.TabIndex = 19;
            this.button3.Text = "Full Set";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(444, 253);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0, 17);
            this.label2.TabIndex = 21;
            // 
            // entropy
            // 
            this.entropy.AutoSize = true;
            this.entropy.Checked = true;
            this.entropy.Location = new Point(8, 37);
            this.entropy.Margin = new Padding(4);
            this.entropy.Name = "entropy";
            this.entropy.Size = new Size(78, 21);
            this.entropy.TabIndex = 22;
            this.entropy.TabStop = true;
            this.entropy.Text = "Entropy";
            this.entropy.UseVisualStyleBackColor = true;
            // 
            // density
            // 
            this.density.AutoSize = true;
            this.density.Location = new Point(8, 65);
            this.density.Margin = new Padding(4);
            this.density.Name = "density";
            this.density.Size = new Size(76, 21);
            this.density.TabIndex = 23;
            this.density.Text = "Density";
            this.density.UseVisualStyleBackColor = true;
            // 
            // EuristicFunction
            // 
            this.EuristicFunction.Controls.Add(this.density);
            this.EuristicFunction.Controls.Add(this.entropy);
            this.EuristicFunction.Location = new Point(380, 10);
            this.EuristicFunction.Margin = new Padding(4);
            this.EuristicFunction.Name = "EuristicFunction";
            this.EuristicFunction.Padding = new Padding(4);
            this.EuristicFunction.Size = new Size(175, 94);
            this.EuristicFunction.TabIndex = 26;
            this.EuristicFunction.TabStop = false;
            this.EuristicFunction.Text = "Euristic Function";
            // 
            // PheromonesUpdateMethod
            // 
            this.PheromonesUpdateMethod.Controls.Add(this.evaporation);
            this.PheromonesUpdateMethod.Controls.Add(this.normalization);
            this.PheromonesUpdateMethod.Location = new Point(583, 10);
            this.PheromonesUpdateMethod.Margin = new Padding(4);
            this.PheromonesUpdateMethod.Name = "PheromonesUpdateMethod";
            this.PheromonesUpdateMethod.Padding = new Padding(4);
            this.PheromonesUpdateMethod.Size = new Size(175, 94);
            this.PheromonesUpdateMethod.TabIndex = 27;
            this.PheromonesUpdateMethod.TabStop = false;
            this.PheromonesUpdateMethod.Text = "Pheromones Recalculation";
            // 
            // evaporation
            // 
            this.evaporation.AutoSize = true;
            this.evaporation.Location = new Point(8, 65);
            this.evaporation.Margin = new Padding(4);
            this.evaporation.Name = "evaporation";
            this.evaporation.Size = new Size(105, 21);
            this.evaporation.TabIndex = 23;
            this.evaporation.Text = "Evaporation";
            this.evaporation.UseVisualStyleBackColor = true;
            // 
            // normalization
            // 
            this.normalization.AutoSize = true;
            this.normalization.Checked = true;
            this.normalization.Location = new Point(8, 37);
            this.normalization.Margin = new Padding(4);
            this.normalization.Name = "normalization";
            this.normalization.Size = new Size(115, 21);
            this.normalization.TabIndex = 22;
            this.normalization.TabStop = true;
            this.normalization.Text = "Normalization";
            this.normalization.UseVisualStyleBackColor = true;
            // 
            // RulesPruningStatus
            // 
            this.RulesPruningStatus.Controls.Add(this.pruningInactive);
            this.RulesPruningStatus.Controls.Add(this.pruningActive);
            this.RulesPruningStatus.Location = new Point(788, 10);
            this.RulesPruningStatus.Margin = new Padding(4);
            this.RulesPruningStatus.Name = "RulesPruningStatus";
            this.RulesPruningStatus.Padding = new Padding(4);
            this.RulesPruningStatus.Size = new Size(175, 94);
            this.RulesPruningStatus.TabIndex = 28;
            this.RulesPruningStatus.TabStop = false;
            this.RulesPruningStatus.Text = "Rule Pruning";
            // 
            // pruningInactive
            // 
            this.pruningInactive.AutoSize = true;
            this.pruningInactive.Location = new Point(8, 65);
            this.pruningInactive.Margin = new Padding(4);
            this.pruningInactive.Name = "pruningInactive";
            this.pruningInactive.Size = new Size(47, 21);
            this.pruningInactive.TabIndex = 23;
            this.pruningInactive.Text = "No";
            this.pruningInactive.UseVisualStyleBackColor = true;
            // 
            // pruningActive
            // 
            this.pruningActive.AutoSize = true;
            this.pruningActive.Checked = true;
            this.pruningActive.Location = new Point(8, 37);
            this.pruningActive.Margin = new Padding(4);
            this.pruningActive.Name = "pruningActive";
            this.pruningActive.Size = new Size(53, 21);
            this.pruningActive.TabIndex = 22;
            this.pruningActive.TabStop = true;
            this.pruningActive.Text = "Yes";
            this.pruningActive.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(16, 594);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new Size(1167, 155);
            this.dataGridView1.TabIndex = 29;
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new Point(642, 153);
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new Size(153, 56);
            this.trackBar1.TabIndex = 30;
            this.trackBar1.Value = 5;
            this.trackBar1.ValueChanged += new EventHandler(this.trackBar1_ValueChanged);
            // 
            // trainingCount
            // 
            this.trainingCount.AutoSize = true;
            this.trainingCount.Location = new Point(145, 159);
            this.trainingCount.Name = "trainingCount";
            this.trainingCount.Size = new Size(0, 17);
            this.trainingCount.TabIndex = 31;
            // 
            // testingCount
            // 
            this.testingCount.AutoSize = true;
            this.testingCount.Location = new Point(145, 248);
            this.testingCount.Name = "testingCount";
            this.testingCount.Size = new Size(0, 17);
            this.testingCount.TabIndex = 32;
            // 
            // trackBarValue
            // 
            this.trackBarValue.AutoSize = true;
            this.trackBarValue.Location = new Point(802, 163);
            this.trackBarValue.Name = "trackBarValue";
            this.trackBarValue.Size = new Size(0, 17);
            this.trackBarValue.TabIndex = 33;
            // 
            // TrainingSetDivideMethod
            // 
            this.TrainingSetDivideMethod.Controls.Add(this.crossValidation);
            this.TrainingSetDivideMethod.Controls.Add(this.byClass);
            this.TrainingSetDivideMethod.Location = new Point(991, 10);
            this.TrainingSetDivideMethod.Margin = new Padding(4);
            this.TrainingSetDivideMethod.Name = "TrainingSetDivideMethod";
            this.TrainingSetDivideMethod.Padding = new Padding(4);
            this.TrainingSetDivideMethod.Size = new Size(175, 94);
            this.TrainingSetDivideMethod.TabIndex = 27;
            this.TrainingSetDivideMethod.TabStop = false;
            this.TrainingSetDivideMethod.Text = "Training Set Divide Method";
            // 
            // crossValidation
            // 
            this.crossValidation.AutoSize = true;
            this.crossValidation.Location = new Point(8, 65);
            this.crossValidation.Margin = new Padding(4);
            this.crossValidation.Name = "crossValidation";
            this.crossValidation.Size = new Size(130, 21);
            this.crossValidation.TabIndex = 23;
            this.crossValidation.Text = "Cross-validation";
            this.crossValidation.UseVisualStyleBackColor = true;
            // 
            // byClass
            // 
            this.byClass.AutoSize = true;
            this.byClass.Checked = true;
            this.byClass.Location = new Point(8, 37);
            this.byClass.Margin = new Padding(4);
            this.byClass.Name = "byClass";
            this.byClass.Size = new Size(83, 21);
            this.byClass.TabIndex = 22;
            this.byClass.TabStop = true;
            this.byClass.Text = "By Class";
            this.byClass.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new Size(1805, 761);
            this.Controls.Add(this.TrainingSetDivideMethod);
            this.Controls.Add(this.trackBarValue);
            this.Controls.Add(this.testingCount);
            this.Controls.Add(this.trainingCount);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.RulesPruningStatus);
            this.Controls.Add(this.PheromonesUpdateMethod);
            this.Controls.Add(this.EuristicFunction);
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
            this.Margin = new Padding(4);
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Ants Classifier";
            this.WindowState = FormWindowState.Maximized;
            ((ISupportInitialize)(this.antsCount)).EndInit();
            ((ISupportInitialize)(this.convergenceStopValue)).EndInit();
            ((ISupportInitialize)(this.maxUncoveredCasesCount)).EndInit();
            ((ISupportInitialize)(this.minNumberPerRule)).EndInit();
            this.EuristicFunction.ResumeLayout(false);
            this.EuristicFunction.PerformLayout();
            this.PheromonesUpdateMethod.ResumeLayout(false);
            this.PheromonesUpdateMethod.PerformLayout();
            this.RulesPruningStatus.ResumeLayout(false);
            this.RulesPruningStatus.PerformLayout();
            ((ISupportInitialize)(this.dataGridView1)).EndInit();
            ((ISupportInitialize)(this.trackBar1)).EndInit();
            this.TrainingSetDivideMethod.ResumeLayout(false);
            this.TrainingSetDivideMethod.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericUpDown antsCount;
        private NumericUpDown convergenceStopValue;
        private NumericUpDown maxUncoveredCasesCount;
        private NumericUpDown minNumberPerRule;
        private Button startButton;
        private ListBox listBox1;
        private Label antCountLabel;
        private Label convergenceCountLabel;
        private Label uncoveredCasesLabel;
        private Label casesPerRuleLabel;
        private Button testButton;
        private Label trainingPathLabel;
        private Label testPathLabel;
        private TextBox textBox1;
        private Label label1;
        private Button button3;
        private Label label2;
        private RadioButton entropy;
        private RadioButton density;
        private GroupBox EuristicFunction;
        private GroupBox PheromonesUpdateMethod;
        private RadioButton evaporation;
        private RadioButton normalization;
        private GroupBox RulesPruningStatus;
        private RadioButton pruningInactive;
        private RadioButton pruningActive;
        private DataGridView dataGridView1;
        private TrackBar trackBar1;
        private Label trainingCount;
        private Label testingCount;
        private Label trackBarValue;
        private GroupBox TrainingSetDivideMethod;
        private RadioButton crossValidation;
        private RadioButton byClass;
    }
}

