﻿namespace MAClassification
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
            this.antCountLabel = new System.Windows.Forms.Label();
            this.convergenceCountLabel = new System.Windows.Forms.Label();
            this.uncoveredCasesLabel = new System.Windows.Forms.Label();
            this.casesPerRuleLabel = new System.Windows.Forms.Label();
            this.testButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.trainingPathLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.testPathLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
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
            10,
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
            3,
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
            this.startButton.Location = new System.Drawing.Point(12, 124);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Train";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(348, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(374, 134);
            this.listBox1.TabIndex = 5;
            // 
            // antCountLabel
            // 
            this.antCountLabel.AutoSize = true;
            this.antCountLabel.Location = new System.Drawing.Point(12, 14);
            this.antCountLabel.Name = "antCountLabel";
            this.antCountLabel.Size = new System.Drawing.Size(59, 13);
            this.antCountLabel.TabIndex = 6;
            this.antCountLabel.Text = "Ants Count";
            // 
            // convergenceCountLabel
            // 
            this.convergenceCountLabel.AutoSize = true;
            this.convergenceCountLabel.Location = new System.Drawing.Point(12, 40);
            this.convergenceCountLabel.Name = "convergenceCountLabel";
            this.convergenceCountLabel.Size = new System.Drawing.Size(102, 13);
            this.convergenceCountLabel.TabIndex = 7;
            this.convergenceCountLabel.Text = "Convergence Count";
            // 
            // uncoveredCasesLabel
            // 
            this.uncoveredCasesLabel.AutoSize = true;
            this.uncoveredCasesLabel.Location = new System.Drawing.Point(12, 66);
            this.uncoveredCasesLabel.Name = "uncoveredCasesLabel";
            this.uncoveredCasesLabel.Size = new System.Drawing.Size(115, 13);
            this.uncoveredCasesLabel.TabIndex = 8;
            this.uncoveredCasesLabel.Text = "Max Uncovered Cases";
            // 
            // casesPerRuleLabel
            // 
            this.casesPerRuleLabel.AutoSize = true;
            this.casesPerRuleLabel.Location = new System.Drawing.Point(12, 92);
            this.casesPerRuleLabel.Name = "casesPerRuleLabel";
            this.casesPerRuleLabel.Size = new System.Drawing.Size(100, 13);
            this.casesPerRuleLabel.TabIndex = 9;
            this.casesPerRuleLabel.Text = "Min Cases Per Rule";
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(12, 197);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 12;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(138, 124);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Training Set";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // trainingPathLabel
            // 
            this.trainingPathLabel.AutoSize = true;
            this.trainingPathLabel.Location = new System.Drawing.Point(12, 163);
            this.trainingPathLabel.Name = "trainingPathLabel";
            this.trainingPathLabel.Size = new System.Drawing.Size(0, 13);
            this.trainingPathLabel.TabIndex = 14;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(138, 197);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Testing Set";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // testPathLabel
            // 
            this.testPathLabel.AutoSize = true;
            this.testPathLabel.Location = new System.Drawing.Point(12, 236);
            this.testPathLabel.Name = "testPathLabel";
            this.testPathLabel.Size = new System.Drawing.Size(0, 13);
            this.testPathLabel.TabIndex = 16;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(138, 265);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Cases With Wrong Class";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(734, 297);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.testPathLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.trainingPathLabel);
            this.Controls.Add(this.button1);
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
        private System.Windows.Forms.Label antCountLabel;
        private System.Windows.Forms.Label convergenceCountLabel;
        private System.Windows.Forms.Label uncoveredCasesLabel;
        private System.Windows.Forms.Label casesPerRuleLabel;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label trainingPathLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label testPathLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;

    }
}

