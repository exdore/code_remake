using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace MAClassification
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            startButton.Enabled = false;
            testButton.Enabled = false;
            button4.Enabled = false;
        }

        private List<Rule> _discoveredRules;
        private Table _trainingTable;
        private Table _testingTable;

        private void startButton_Click(object sender, EventArgs e)
        {
            //button4_Click(sender,e);
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
            Initialize(trainingPathLabel.Text, out data, out attributes, out results, out terms);
            data.Serialize();
            File.Delete(@"rules.xml");
            while (data.GetCasesCount() > maxUncoveredCases)
            {
                var currentAnt = 0;
                var currentNumberForConvergence = 0;
                var currentCases = data.GetCases();
                var currentRules = new List<Rule>();
                var socialTerms = terms.Deserialize();
                var basicTerms = terms.Deserialize();
                var greedyTerms = terms.Deserialize();
                var mergingTerms = terms.Deserialize();
                if (CheckUsedTerms(terms, data))
                {
                    while (currentAnt < maxAntsGenerationsNumber * 4 && currentNumberForConvergence < maxNumberForConvergence)
                    {
                        var basicAnt = new Ant
                        {
                            Alpha = (double)new Random().Next(40, 60) / 100,
                            Beta = (double)new Random().Next(40,60) / 100
                        };
                        var greedyAnt = new Ant
                        {
                            Alpha = (double)new Random().Next(0, 30) / 100,
                            Beta = (double)new Random().Next(80, 100) / 100
                        };
                        var socialAnt = new Ant
                        {
                            Alpha = (double)new Random().Next(70, 100) / 100,
                            Beta = (double)new Random().Next(0, 20) / 100
                        };
                        var mergingAnt = new Ant
                        {
                            Alpha = 1,
                            Beta = 1
                        };
                        GetAntResult(basicAnt, currentCases, minCasesPerRule, basicTerms, data, attributes, results, currentRules, groupBox1, groupBox2, ref currentNumberForConvergence, ref currentAnt);
                        GetAntResult(greedyAnt, currentCases, minCasesPerRule, greedyTerms, data, attributes, results, currentRules, groupBox1, groupBox2, ref currentNumberForConvergence, ref currentAnt);
                        GetAntResult(socialAnt, currentCases, minCasesPerRule, socialTerms, data, attributes, results, currentRules, groupBox1, groupBox2, ref currentNumberForConvergence, ref currentAnt);
                        mergingTerms.Merge(socialTerms, basicTerms, greedyTerms);
                        mergingTerms.UpdateWeights(new Rule(), groupBox2, currentAnt);
                        mergingTerms.Update(attributes, mergingAnt, groupBox1, currentCases);
                        GetAntResult(mergingAnt, currentCases, minCasesPerRule, mergingTerms, data, attributes, results, currentRules, groupBox1, groupBox2, ref currentNumberForConvergence, ref currentAnt);
                    }
                    _discoveredRules.Add(currentRules.OrderByDescending(item => item.Quality).First());
                    data.Cases = data.Cases.Except(_discoveredRules.Last().CoveredCases).ToList();
                    _discoveredRules.Last().Serialize();
                }
            }
            listBox1.HorizontalScrollbar = true;
            listBox1.DataSource = _discoveredRules;
        }

        private static void GetAntResult(Ant ant, List<Case> currentCases, decimal minCasesPerRule, Terms terms, Table data,
            Attributes attributes, List<string> results, List<Rule> currentRules, GroupBox groupBox1, GroupBox groupBox2, ref int currentNumberForConvergence, ref int currentAnt)
        {
            var currentAntRule = ant.RunAnt(currentCases, minCasesPerRule, terms, data, attributes, results, groupBox1);
            if (currentAntRule.ConditionsList.Count > 1)
            {
                currentAntRule = currentAntRule.PruneRule(data, results);
                currentAntRule.GetCoveredCases(data);
                currentAntRule.GetRuleResult(results);
                currentAntRule.CalculateRuleQuality(data);
                terms.UpdateWeights(currentAntRule, groupBox2, currentAnt);
            }
            if (currentRules.Count == 0)
            {
                currentRules.Add(currentAntRule);
            }
            var equal = currentAntRule.ConditionsList.SequenceEqual(currentRules.Last().ConditionsList);
            if (equal)
            {
                currentNumberForConvergence++;
            }
            else
            {
                currentRules.Add(currentAntRule);
                currentNumberForConvergence = 0;
            }
            foreach (var attribute in attributes)
            {
                attribute.IsUsed = false;
            }
            CheckUsedTerms(terms, data);
            terms.Update(attributes, ant, groupBox1, currentCases);
            currentAnt++;
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
            initialTerms.Serialize();
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

        private void button1_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog {Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*"};
            openFileDialog1.ShowDialog();
            var file = openFileDialog1.FileName;
            trainingPathLabel.Text = file;
            startButton.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog {Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*"};
            openFileDialog1.ShowDialog();
            var file = openFileDialog1.FileName;
            testPathLabel.Text = file;
            testButton.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog { Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*" };
            openFileDialog1.ShowDialog();
            label2.Text = openFileDialog1.FileName;
            button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var data = Table.ReadData(label2.Text);
            data.Cases = data.Cases.OrderBy(item => Guid.NewGuid()).ToList();
            var count = data.Cases.Count / 5;
            _trainingTable = new Table
            {
                Cases = data.Cases.GetRange(0, count).ToList(),
                Header = data.Header
            };
            _testingTable = new Table
            {
                Cases = data.Cases.GetRange(count, data.Cases.Count - count).ToList(),
                Header = data.Header
            };
            testButton.Enabled = true;
            startButton.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = false;
        }
    }
}
