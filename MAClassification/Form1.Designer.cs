namespace MAClassification
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.uncoveredCount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.antsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.convergenceStopValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxUncoveredCasesCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minNumberPerRule)).BeginInit();
            this.SuspendLayout();
            // 
            // antsCount
            // 
            this.antsCount.Location = new System.Drawing.Point(139, 12);
            this.antsCount.Name = "antsCount";
            this.antsCount.Size = new System.Drawing.Size(120, 20);
            this.antsCount.TabIndex = 0;
            this.antsCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // convergenceStopValue
            // 
            this.convergenceStopValue.Location = new System.Drawing.Point(139, 38);
            this.convergenceStopValue.Name = "convergenceStopValue";
            this.convergenceStopValue.Size = new System.Drawing.Size(120, 20);
            this.convergenceStopValue.TabIndex = 1;
            this.convergenceStopValue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // maxUncoveredCasesCount
            // 
            this.maxUncoveredCasesCount.Location = new System.Drawing.Point(138, 64);
            this.maxUncoveredCasesCount.Name = "maxUncoveredCasesCount";
            this.maxUncoveredCasesCount.Size = new System.Drawing.Size(120, 20);
            this.maxUncoveredCasesCount.TabIndex = 2;
            this.maxUncoveredCasesCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // minNumberPerRule
            // 
            this.minNumberPerRule.Location = new System.Drawing.Point(138, 90);
            this.minNumberPerRule.Name = "minNumberPerRule";
            this.minNumberPerRule.Size = new System.Drawing.Size(120, 20);
            this.minNumberPerRule.TabIndex = 3;
            this.minNumberPerRule.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(138, 126);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(265, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(457, 277);
            this.listBox1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Ants Count";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Convergence Count";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Max Uncovered Cases";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Min Cases Per Rule";
            // 
            // uncoveredCount
            // 
            this.uncoveredCount.Location = new System.Drawing.Point(138, 177);
            this.uncoveredCount.Name = "uncoveredCount";
            this.uncoveredCount.Size = new System.Drawing.Size(100, 20);
            this.uncoveredCount.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(734, 297);
            this.Controls.Add(this.uncoveredCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.minNumberPerRule);
            this.Controls.Add(this.maxUncoveredCasesCount);
            this.Controls.Add(this.convergenceStopValue);
            this.Controls.Add(this.antsCount);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.antsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.convergenceStopValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxUncoveredCasesCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minNumberPerRule)).EndInit();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox uncoveredCount;

    }
}

