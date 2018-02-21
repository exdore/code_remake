using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using ArffSharp;
using MAClassification.Models;
using MAClassification.Serializators;

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
            Records = new List<ArffRecord>();
            ArffReader reader = new ArffReader(label2.Text);
            ArffRecord record;
            while ((record = reader.ReadNextRecord()) != null)
            {
                Records.Add(record);
            }
            reader.BuildIntervals();
            Records = reader.Discretize(Records);
            var t = Table.CreateTable(Records, reader);

            Dictionary<string, EuristicTypes> euristicMapper = new Dictionary<string, EuristicTypes>
            {
                {"entropy", EuristicTypes.Entropy},
                {"density", EuristicTypes.Density}
            };

            Dictionary<string, PheromonesTypes> pheromonesMapper = new Dictionary<string, PheromonesTypes>
            {
                {"evaporation", PheromonesTypes.Evaporation},
                {"normalization", PheromonesTypes.Normalization}
            };

            Dictionary<string, DivideTypes> divideMapper = new Dictionary<string, DivideTypes>
            {
                {"byClass", DivideTypes.ByClass},
                {"crossValidation", DivideTypes.CrossValidation}
            };


            var euristicFunctionType = EuristicFunction.Controls.OfType<RadioButton>()
                .FirstOrDefault(item => item.Checked)?.Name;
            var pheromonesFunctionType = PheromonesUpdateMethod.Controls.OfType<RadioButton>()
                .FirstOrDefault(item => item.Checked)?.Name;
            var pruningStatus = RulesPruningStatus.Controls.OfType<RadioButton>()
                .FirstOrDefault(item => item.Checked)?.Name;
            var divideMethod = TrainingSetDivideMethod.Controls.OfType<RadioButton>()
                .FirstOrDefault(item => item.Checked)?.Name;

            bool isPruned = pruningStatus == "pruningActive";
            CalculationOptions options = new CalculationOptions
            {
                EuristicType = euristicMapper[euristicFunctionType],
                PheromonesType = pheromonesMapper[pheromonesFunctionType],
                DivideType = divideMapper[divideMethod],
                IsPruned = isPruned,
                DataPath = label2.Text,
                MaxAntsGenerationsNumber = (int)antsCount.Value,
                MaxNumberForConvergence = (int)convergenceStopValue.Value,
                MaxUncoveredCases = (int)maxUncoveredCasesCount.Value,
                MinCasesPerRule = (int)minNumberPerRule.Value,
                CrossValidationCoefficient = trackBar1.Value
            };

            _solver = new Solver
            {
                Rules = new List<Rule>(),
                Data = new Table(),
                Agents = new List<Agent>(),
                CalculationOptions = options
            };
            _solver.Data = t;
            _solver.InitializeDataTables();
            var divider = new Divider();
            var tables = divider.MakeTables(_solver.CalculationOptions.DivideType, _solver.Data, _n);
            testingCount.Text = _solver.InitializeDataTables().GetCasesCount().ToString();
            var terms = _solver.InitializeTerms();
            TermsSerializer ts = new TermsSerializer();
            ts.Serialize(terms);
            File.Delete(@"rules.xml");
            PopulateDataGrid();
            _rulesSets = new List<Rule>();
            Parallel.ForEach(tables, table =>
            {
                _rulesSets.AddRange(_solver.FindSolution(table));
            });
            //foreach (Table table in tables)
            //{
            //    _rulesSets.AddRange(_solver.FindSolution(table));
            //}
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
                row[data.Header.Count + 1] = item.Class;
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

        public List<ArffRecord> Records { get; set; }

        private void button3_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog { Filter = @"arff files (*.arff)|*.arff" };
            openFileDialog1.ShowDialog();
            label2.Text = openFileDialog1.FileName;
        }
    }
}
