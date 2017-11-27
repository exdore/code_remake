﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace MAClassification
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            trackBarValue.Text = Convert.ToString(trackBar1.Value);
            _n = trackBar1.Value;
        }

        private List<Rule> _rulesSets;
        private Solver _solver;
        private int _n;

        private void startButton_Click(object sender, EventArgs e)
        {
            _solver = new Solver
            {
                _dataPath = label2.Text,
                MaxAntsGenerationsNumber = (int)antsCount.Value,
                MaxNumberForConvergence = (int)convergenceStopValue.Value,
                MaxUncoveredCases = (int)maxUncoveredCasesCount.Value,
                MinCasesPerRule = (int)minNumberPerRule.Value,
                _crossValidationCoefficient = trackBar1.Value,
                Rules = new List<Rule>(),
                Data = new Table(),
                Agents = new List<Agent>(),
                EuristicFunctionType = euristicFunctionType.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked)?.Name,
                PheromonesUpdateMethod = PheromonesUpdateMethod.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked)?.Name,
                RulesPruningStatus = RulesPruningStatus.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked)?.Name
            };
            _solver.InitializeDataTables();
            var divider = new Divider();
            var tables = divider.Divide(_n, _solver.Data);
            //var tables = divider.DivideByClass(solver.Data);
            testingCount.Text = _solver._testingTable.GetCasesCount().ToString();
            var terms = _solver.InitializeTerms();
            terms.Serialize();
            File.Delete(@"rules.xml");
            PopulateDataGrid();
            _rulesSets = new List<Rule>();
            foreach (Table table in tables)
            {
                _rulesSets.AddRange(_solver.FindSolution(table));
            }
            //_rulesSets = _rulesSets.OrderByDescending(item => item.CoveredCases.Count).ToList();
            listBox1.DataSource = _rulesSets;
            XmlSerializer xmlsr = new XmlSerializer(typeof(List<Agent>));
            StreamWriter strwr = new StreamWriter(@"list_ants.xml");
            xmlsr.Serialize(strwr, _solver.Agents);
            strwr.Close();
        }

        private void PopulateDataGrid()
        {
            var data = _solver.Data;
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Number", typeof(int)));
            foreach (string item in data.Header)
                dt.Columns.Add(new DataColumn(item, typeof(string)));
            dt.Columns.Add(new DataColumn("Class", typeof(string)));
            foreach (var item in data.Cases)
            {
                var row = dt.NewRow();
                row[0] = item.Number;
                for (int i = 1; i <= item.AttributesValuesList.Count; i++)
                    row[i] = item.AttributesValuesList[i - 1];
                row[data.Header.Count + 1] = item.Result;
                dt.Rows.Add(row);    
            }
            dataGridView1.DataSource = dt;
        }

        private void testButton_Click(object sender, EventArgs e)  //not working as intended for now
        {
            textBox1.Text = _solver.Test(_rulesSets).ToString();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            trackBarValue.Text = Convert.ToString(trackBar1.Value);
            _n = trackBar1.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog { Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*" };
            openFileDialog1.ShowDialog();
            label2.Text = openFileDialog1.FileName;
        }
    }
}
