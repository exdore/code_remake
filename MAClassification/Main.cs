using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace MAClassification
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            trackBarValue.Text = Convert.ToString(trackBar1.Value / 100.0);
        }

        Solver solver;

        private void startButton_Click(object sender, EventArgs e)
        {
            solver = new Solver
            {
                _dataPath = label2.Text,
                MaxAntsGenerationsNumber = (int)antsCount.Value,
                MaxNumberForConvergence = (int)convergenceStopValue.Value,
                MaxUncoveredCases = (int)maxUncoveredCasesCount.Value,
                MinCasesPerRule = (int)minNumberPerRule.Value,
                _crossValidationCoefficient = trackBar1.Value / 100.0,
                Rules = new List<Rule>(),
                Data = new Table(),
                Agents = new List<Agent>(),
                EuristicFunctionType = euristicFunctionType.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked).Name,
                PheromonesUpdateMethod = PheromonesUpdateMethod.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked).Name,
                RulesPruningStatus = RulesPruningStatus.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked).Name
            };
            var data = solver.InitializeDataTables();
            trainingCount.Text = solver._trainingTable.GetCasesCount().ToString();
            testingCount.Text = solver._testingTable.GetCasesCount().ToString();
            var terms = solver.InitializeTerms();
            terms.Serialize();
            File.Delete(@"rules.xml");
            PopulateDataGrid();
            var _discoveredRules = solver.FindSolution();
            var _chosenAgents = solver.Agents;
            listBox1.HorizontalScrollbar = true;
            listBox1.DataSource = _discoveredRules.OrderByDescending(item => item.Result).ToList();
            XmlSerializer xmlsr = new XmlSerializer(typeof(List<Agent>));
            StreamWriter strwr = new StreamWriter(@"list_ants.xml");
            xmlsr.Serialize(strwr, _chosenAgents);
            strwr.Close();
        }

        private void PopulateDataGrid()
        {
            var data = solver.Data;
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Number", typeof(int)));
            for (int i = 0; i < data.Header.Count; i++)
                dt.Columns.Add(new DataColumn(data.Header[i], typeof(string)));
            dt.Columns.Add(new DataColumn("Class", typeof(string)));
            foreach (var item in data.Cases)
            {
                var row = dt.NewRow();
                row[0] = item.Number;
                for (int i = 1; i <= item.AttributesValuesList.Count; i++)
                    row[i] = item.AttributesValuesList[i - 1];
                row[data.Header.Count + 1] = item.Result;
                dt.Rows.Add(row);    //dependance on attributes count, code in cycle
            }
            dataGridView1.DataSource = dt;
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = solver.Test().ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog { Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*" };
            openFileDialog1.ShowDialog();
            label2.Text = openFileDialog1.FileName;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            trackBarValue.Text = Convert.ToString(trackBar1.Value / 100.0);
        }
    }
}
