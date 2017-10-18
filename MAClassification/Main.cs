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
        public enum Types { basic, greedy, euristic };

        public Main()
        {
            InitializeComponent();
            startButton.Enabled = false;
            testButton.Enabled = false;
            button4.Enabled = false;
        }

        private List<Rule> _discoveredRules;
        private Table _trainingTable;
        private Table _testingTable;
        private List<Ant> _chosenAnts;

        private void startButton_Click(object sender, EventArgs e)
        {
            _discoveredRules = new List<Rule>();
            var maxAntsGenerationsNumber = antsCount.Value;
            var maxNumberForConvergence = convergenceStopValue.Value;
            var maxUncoveredCases = maxUncoveredCasesCount.Value;
            var minCasesPerRule = minNumberPerRule.Value;
            Table data;
            Attributes attributes;
            List<string> results;
            Terms terms;
            _discoveredRules = new List<Rule>();
            _chosenAnts = new List<Ant>();
            Initialize(trainingPathLabel.Text, out data, out attributes, out results, out terms);
            data.Serialize();
            terms.Serialize("");
            File.Delete(@"rules.xml");
            while (data.GetCasesCount() > maxUncoveredCases)
            {
                var currentAnt = 0;
                var currentNumberForConvergence = 0;
                var currentCases = data.GetCases();
                var currentRules = new List<Rule>();
                var socialTerms = terms.Deserialize();
                socialTerms.MaxValue = 0.8;
                socialTerms.MinValue = 0.001;
                var basicTerms = terms.Deserialize();
                basicTerms.MaxValue = 0.8;
                basicTerms.MinValue = 0.001;
                var greedyTerms = terms.Deserialize();
                greedyTerms.MaxValue = 0.8;
                greedyTerms.MinValue = 0.001;
                if (CheckUsedTerms(terms, data))
                {
                    var antsPopulation = IterateGeneration(maxAntsGenerationsNumber, maxNumberForConvergence, minCasesPerRule,
                        data, attributes, results, ref currentAnt, ref currentNumberForConvergence, currentCases, currentRules, 
                        socialTerms, basicTerms, greedyTerms);
                    antsPopulation = antsPopulation.OrderByDescending(item => item.Rule.Quality).ToList();
                    _chosenAnts.Add(antsPopulation.Where(item => (item.Rule.Quality - antsPopulation.First().Rule.Quality)
                        < 1e-5).OrderByDescending(item => new Random().Next()).First());
                    _discoveredRules.Add(_chosenAnts.Last().Rule);
                    data.Cases = data.Cases.Except(_discoveredRules.Last().CoveredCases).ToList();
                }
            }
            listBox1.HorizontalScrollbar = true;
            listBox1.DataSource = _discoveredRules.OrderByDescending(item => item.Result).ToList();
            XmlSerializer xmlsr = new XmlSerializer(typeof(List<Ant>));
            StreamWriter strwr = new StreamWriter(@"list_ants.xml");
            xmlsr.Serialize(strwr, _chosenAnts);
            strwr.Close();
        }

        private List<Ant> IterateGeneration(decimal maxAntsGenerationsNumber, decimal maxNumberForConvergence, decimal minCasesPerRule, 
            Table data, Attributes attributes, List<string> results, ref int currentAnt, ref int currentNumberForConvergence, 
            List<Case> currentCases, List<Rule> currentRules, Terms socialTerms, Terms basicTerms, Terms greedyTerms)
        {
            var antsPopulation = new List<Ant>();
            while (antsPopulation.Count < maxAntsGenerationsNumber * 3 && currentNumberForConvergence < maxNumberForConvergence)
            {
                var rnd = new Random();
                var basicAnt = new Ant
                {
                    Alpha = 1,
                    Beta = 1,
                    Type = Types.basic.ToString()
                };
                var greedyAnt = new Ant
                {
                    Alpha = rnd.Next(200, 500) / 100.0,
                    Beta = rnd.Next(500, 1000) / 100.0,
                    Type = Types.greedy.ToString()
                };
                var socialAnt = new Ant
                {
                    Alpha = rnd.Next(500, 1000) / 100.0,
                    Beta = rnd.Next(200, 500) / 100.0,
                    Type = Types.euristic.ToString()
                };
                GetAntResult(basicAnt, currentCases, minCasesPerRule, basicTerms, data, attributes, results, currentRules, groupBox1, groupBox2, groupBox3, ref currentNumberForConvergence, ref currentAnt);
                GetAntResult(greedyAnt, currentCases, minCasesPerRule, greedyTerms, data, attributes, results, currentRules, groupBox1, groupBox2, groupBox3, ref currentNumberForConvergence, ref currentAnt);
                GetAntResult(socialAnt, currentCases, minCasesPerRule, socialTerms, data, attributes, results, currentRules, groupBox1, groupBox2, groupBox3, ref currentNumberForConvergence, ref currentAnt);
                antsPopulation.Add(basicAnt);
                antsPopulation.Add(greedyAnt);
                antsPopulation.Add(socialAnt);
                greedyTerms.Serialize("greedy");
                socialTerms.Serialize("social");
                basicTerms.Serialize("basic");
            }
            return antsPopulation;
        }

        private static void GetAntResult(Ant ant, List<Case> currentCases, decimal minCasesPerRule, Terms terms, Table data,
            Attributes attributes, List<string> results, List<Rule> currentRules, GroupBox groupBox1, GroupBox groupBox2, GroupBox groupBox3, ref int currentNumberForConvergence, ref int currentAnt)
        {
            ant.RunAnt(currentCases, minCasesPerRule, terms, data, attributes, results, groupBox2);
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
            var equal = false;
            if (currentRules.Count > 0)
                equal = currentAntRule.ConditionsList.SequenceEqual(currentRules.Last().ConditionsList);
            if (equal)
            {
                currentNumberForConvergence++;
            }
            else
            {
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
            foreach (var term in terms)
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

        private void Initialize(string path, out Table data, out Attributes attributes, out List<string> results, out Terms initialTerms)
        {
            data = _trainingTable ?? Table.ReadData(path);
            PopulateDataGrid();
            attributes = data.GetAttributesInfo();
            results = data.GetResultsInfo();
            initialTerms = new Terms();
            foreach (var attribute in attributes)
            {
                initialTerms.Add(new List<Term>());
                foreach (var item in attribute.AttributeValues)
                {
                    initialTerms.Last().Add(new Term
                    {
                        AttributeName = attribute.AttributeName,
                        AttributeValue = item,
                        Entropy = data.CalculateGain(attribute.AttributeName, item, results)
                    });
                }
            }
            initialTerms.FullInitialize(attributes, groupBox1, data.Cases);
            initialTerms.Serialize("initial");
        }

        private void PopulateDataGrid()
        {
            var data = _testingTable;
            data.Cases.AddRange(_trainingTable.Cases);
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
            var data = _testingTable ?? Table.ReadData(testPathLabel.Text);
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
            button4.Enabled = true;
        }

        private Table _fullData; 

        private void button4_Click(object sender, EventArgs e)
        {
            _fullData = Table.ReadData(label2.Text);
            _fullData.Cases = _fullData.Cases.OrderBy(item => Guid.NewGuid()).ToList();
            var count = (int)Math.Floor(_fullData.Cases.Count * 0.8);
            _trainingTable = new Table
            {
                Cases = _fullData.Cases.GetRange(0, count).ToList(),
                Header = _fullData.Header
            };
            _testingTable = new Table
            {
                Cases = _fullData.Cases.GetRange(count, _fullData.Cases.Count - count).ToList(),
                Header = _fullData.Header
            };
            testButton.Enabled = true;
            startButton.Enabled = true;
        }
    }
}
