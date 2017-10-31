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
            startButton.Enabled = false;
            testButton.Enabled = false;
            button4.Enabled = false;
        }

        private double _crossValidatonCoefficient;
        private List<Rule> _discoveredRules;
        private Table _fullData;
        private Table _trainingTable;
        private Table _testingTable;
        private List<Agent> _chosenAgents;
        private string _dataPath;

        private void startButton_Click(object sender, EventArgs e)
        {
            _crossValidatonCoefficient = trackBar1.Value / 100.0;
            var maxAntsGenerationsNumber = antsCount.Value;
            var maxNumberForConvergence = convergenceStopValue.Value;
            var maxUncoveredCases = maxUncoveredCasesCount.Value;
            var minCasesPerRule = minNumberPerRule.Value;
            _discoveredRules = new List<Rule>();
            _chosenAgents = new List<Agent>();
            _fullData.Cases = _fullData.Cases.OrderBy(item => Guid.NewGuid()).ToList();
            var count = (int)Math.Floor(_fullData.Cases.Count * _crossValidatonCoefficient);
            _trainingTable = new Table
            {
                Cases = _fullData.Cases.GetRange(0, count).ToList(),
                Header = _fullData.Header,
                TableType = TableTypes.Training
            };
            _trainingTable.Serialize();
            _testingTable = new Table
            {
                Cases = _fullData.Cases.GetRange(count, _fullData.Cases.Count - count).ToList(),
                Header = _fullData.Header,
                TableType = TableTypes.Testing
            };
            _testingTable.Serialize();
            var data = _trainingTable.Deserialize();
            var terms = Initialize(data, out Attributes attributes, out List<string> results);
            terms.Serialize();
            //maxAntsGenerationsNumber = data.Cases.Count / 10;
            //maxNumberForConvergence = (int)Math.Sqrt((double)maxAntsGenerationsNumber);
            //maxUncoveredCases = maxAntsGenerationsNumber / 5;
            //minCasesPerRule = maxUncoveredCases * 2;
            //antsCount.Value = maxAntsGenerationsNumber;
            //convergenceStopValue.Value = maxNumberForConvergence;
            //maxUncoveredCasesCount.Value = maxUncoveredCases;
            //minNumberPerRule.Value = minCasesPerRule;
            File.Delete(@"rules.xml");
            while (data.GetCasesCount() > maxUncoveredCases)
            {
                var currentAnt = 0;
                var currentNumberForConvergence = 0;
                var currentCases = data.GetCases();
                var currentRules = new List<Rule>();
                var socialTerms = terms.Deserialize();
                socialTerms.TermType = TermTypes.Euristic;
                var basicTerms = terms.Deserialize();
                basicTerms.TermType = TermTypes.Basic;
                var greedyTerms = terms.Deserialize();
                greedyTerms.TermType = TermTypes.Greedy;
                if (CheckUsedTerms(terms, data))
                {
                    var antsPopulation = IterateGeneration(maxAntsGenerationsNumber, maxNumberForConvergence, minCasesPerRule,
                        data, attributes, results, ref currentAnt, ref currentNumberForConvergence, currentCases, currentRules,
                        socialTerms, basicTerms, greedyTerms);
                    antsPopulation = antsPopulation.OrderByDescending(item => item.Rule.Quality).ToList();
                    _chosenAgents.Add(antsPopulation.Where(item => (item.Rule.Quality - antsPopulation.First().Rule.Quality)
                        < 1e-5).OrderByDescending(item => new Random().Next()).First());
                    _discoveredRules.Add(_chosenAgents.Last().Rule);
                    data.Cases = data.Cases.Except(_chosenAgents.Last().Rule.CoveredCases).ToList();
                }
            }
            listBox1.HorizontalScrollbar = true;
            listBox1.DataSource = _discoveredRules.OrderByDescending(item => item.Result).ToList();
            XmlSerializer xmlsr = new XmlSerializer(typeof(List<Agent>));
            StreamWriter strwr = new StreamWriter(@"list_ants.xml");
            xmlsr.Serialize(strwr, _chosenAgents);
            strwr.Close();
        }

        private List<Ant> IterateGeneration(decimal maxAntsGenerationsNumber, decimal maxNumberForConvergence, decimal minCasesPerRule,
            Table data, Attributes attributes, List<string> results, ref int currentAnt, ref int currentNumberForConvergence,
            List<Case> currentCases, List<Rule> currentRules, Terms socialTerms, Terms basicTerms, Terms greedyTerms)
        {
            var antsPopulation = new List<Ant>();
            var count = 0;
            while (count < maxAntsGenerationsNumber * 3 && currentNumberForConvergence < maxNumberForConvergence)
            {
                var rnd = new Random();
                var basicAnt = new Ant
                {
                    Alpha = 1,
                    Beta = 1,
                    AntType = AntTypes.Basic,
                    AntNumber = ++count
                };
                var greedyAnt = new Ant
                {
                    Alpha = rnd.Next(200, 500) / 100.0,
                    Beta = rnd.Next(500, 1000) / 100.0,
                    AntType = AntTypes.Greedy,
                    AntNumber = ++count
                };
                var socialAnt = new Ant
                {
                    Alpha = rnd.Next(500, 1000) / 100.0,
                    Beta = rnd.Next(200, 500) / 100.0,
                    AntType = AntTypes.Euristic,
                    AntNumber = ++count
                };
                GetAntResult(basicAnt, currentCases, minCasesPerRule, basicTerms, data, attributes, results, currentRules, groupBox1, groupBox2, groupBox3, ref currentNumberForConvergence, ref currentAnt);
                GetAntResult(greedyAnt, currentCases, minCasesPerRule, greedyTerms, data, attributes, results, currentRules, groupBox1, groupBox2, groupBox3, ref currentNumberForConvergence, ref currentAnt);
                GetAntResult(socialAnt, currentCases, minCasesPerRule, socialTerms, data, attributes, results, currentRules, groupBox1, groupBox2, groupBox3, ref currentNumberForConvergence, ref currentAnt);
                antsPopulation.Add(basicAnt);
                antsPopulation.Add(greedyAnt);
                antsPopulation.Add(socialAnt);
            }
            return antsPopulation;
        }

        private static void GetAntResult(Ant ant, List<Case> currentCases, decimal minCasesPerRule, Terms terms, Table data,
            Attributes attributes, List<string> results, List<Rule> currentRules, GroupBox groupBox1, GroupBox groupBox2, GroupBox groupBox3, ref int currentNumberForConvergence, ref int currentAnt)
        {
            ant.BuildRule(currentCases, minCasesPerRule, terms, data, attributes, results, groupBox2);
            var currentAntRule = ant.Rule;
            var pruningActive = groupBox3.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked);
            if (pruningActive != null && pruningActive.Text == @"Да")
            {
                currentAntRule = currentAntRule.PruneRule(data, results);
                currentAntRule.GetCoveredCases(data);
                currentAntRule.GetRuleResult(results);
                currentAntRule.CalculateRuleQuality(data);
                terms.UpdateWeights(currentAntRule, groupBox2, currentAnt);
            }
            if (currentRules.Count > 0)
            {
                if (currentAntRule.Equals(currentRules.Last()))
                    currentNumberForConvergence++;
                else
                    currentNumberForConvergence = 0;
            }
            currentRules.Add(currentAntRule);
            foreach (var attribute in attributes)
            {
                attribute.IsUsed = false;
            }
            CheckUsedTerms(terms, data);
            terms.Update(attributes, ant, groupBox1, currentCases);
            currentAnt++;
            if (pruningActive != null && pruningActive.Text == @"Да")
                ant.Rule = currentAntRule;
        }

        private static bool CheckUsedTerms(Terms terms, Table data)
        {
            var availableTermsCount = 0;
            foreach (var term in terms.TermsList)
            {
                foreach (var item in term)
                {
                    item.IsChosen = false;
                    var fl = false;
                    var index = data.Header.IndexOf(item.AttributeName);
                    foreach (var dataCase in data.Cases)
                    {
                        if (dataCase.AttributesValuesList[index] == item.AttributeValue)
                        {
                            availableTermsCount++;
                            fl = true;
                            break;
                        }
                    }
                    if (!fl) item.IsChosen = true;
                }
            }
            return availableTermsCount != 0;
        }

        private Terms Initialize(Table data, out Attributes attributes, out List<string> results)
        {
            PopulateDataGrid();
            attributes = data.GetAttributesInfo();
            results = data.GetResultsInfo();
            var initialTerms = new Terms
            {
                TermsList = new List<List<Term>>(),
                TermType = TermTypes.Basic,
                MaxValue = 0.8,
                MinValue = 0.01
            };
            foreach (var attribute in attributes)
            {
                initialTerms.TermsList.Add(new List<Term>());
                foreach (var item in attribute.AttributeValues)
                {
                    initialTerms.TermsList.Last().Add(new Term
                    {
                        AttributeName = attribute.AttributeName,
                        AttributeValue = item,
                        Entropy = data.CalculateGain(attribute.AttributeName, item, results)
                    });
                }
            }
            initialTerms.FullInitialize(attributes, groupBox1, data.Cases);
            return initialTerms;
        }

        private void PopulateDataGrid()
        {
            var data = _fullData;
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Number", typeof(int)));
            for (int i = 0; i < data.Header.Count; i++)
                dt.Columns.Add(new DataColumn("Attribute " + i, typeof(string)));
            dt.Columns.Add(new DataColumn("Class", typeof(string)));
            foreach (var item in data.Cases)
                dt.Rows.Add(item.Number, item.AttributesValuesList[0], item.AttributesValuesList[1],
                    item.AttributesValuesList[2], item.AttributesValuesList[3], item.AttributesValuesList[4], item.AttributesValuesList[5],
                    item.Result);     //dependance on attributes count, code in cycle
            dataGridView1.DataSource = dt;
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            var data = _testingTable.Deserialize();
            data.Cases = data.Cases.OrderBy(item => item.Number).ToList();
            var realResults = new List<string>();
            foreach (var item in data.Cases)
            {
                realResults.Add(item.Result);
                item.Result = "";
            }
            foreach (var discoveredRule in _discoveredRules)
            {
                discoveredRule.GetCoveredCases(data);
                foreach (var discoveredRuleCoveredCase in discoveredRule.CoveredCases)
                {
                    data.Cases.Find(item => item.Number == discoveredRuleCoveredCase.Number).Result =
                        discoveredRule.Result;
                }
                data.Cases = data.Cases.Except(discoveredRule.CoveredCases).ToList();
            }
            data = _testingTable.Deserialize();

            var predictedResults = new List<string>();
            foreach (var item in data.Cases)
            {
                predictedResults.Add(item.Result);
            }
            var count = 0;
            for (var index = 0; index < predictedResults.Count; index++)
            {
                if (predictedResults[index] != realResults[index])
                    count++;
            }
            textBox1.Text = count.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog { Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*" };
            openFileDialog1.ShowDialog();
            label2.Text = openFileDialog1.FileName;
            _dataPath = label2.Text;
            button4.Enabled = true;
        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            _fullData = Table.ReadData(_dataPath);
            testButton.Enabled = true;
            startButton.Enabled = true;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
