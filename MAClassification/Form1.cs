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
            startButton.Focus();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            var maxAntsNumber = antsCount.Value;
            var maxNumberForConvergence = convergenceStopValue.Value;
            var maxUncoveredCases = maxUncoveredCasesCount.Value;
            var minCasesPerRule = minNumberPerRule.Value;
            Table data;
            Attributes attributes;
            List<string> results;
            Terms terms;
            var discoveredRules = new List<Rule>();
            Initialize(out data, out attributes, out results, out terms);
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
                    while (currentAnt < maxAntsNumber && currentNumberForConvergence < maxNumberForConvergence)
                    {
                        var basicAnt = new Ant
                        {
                            Alpha = (double)new Random().Next(40, 60) / 100,
                            Beta = (double)new Random().Next(40,60) / 100
                        };
                        var greedyAnt = new Ant
                        {
                            Alpha = (double)new Random().Next(80, 100) / 100,
                            Beta = (double)new Random().Next(0, 30) / 100
                        };
                        var socialAnt = new Ant
                        {
                            Alpha = (double)new Random().Next(0, 20) / 100,
                            Beta = (double)new Random().Next(70, 100) / 100
                        };
                        var mergingAnt = new Ant
                        {
                            Alpha = 1,
                            Beta = 1
                        };
                        GetAntResult(basicAnt, currentCases, minCasesPerRule, basicTerms, data, attributes, results, currentRules, ref currentNumberForConvergence, ref currentAnt);
                        GetAntResult(greedyAnt, currentCases, minCasesPerRule, greedyTerms, data, attributes, results, currentRules, ref currentNumberForConvergence, ref currentAnt);
                        GetAntResult(socialAnt, currentCases, minCasesPerRule, socialTerms, data, attributes, results, currentRules, ref currentNumberForConvergence, ref currentAnt);
                        mergingTerms.Merge(socialTerms, basicTerms, greedyTerms);
                        mergingTerms.UpdateWeights(new Rule());
                        mergingTerms.Update(attributes, new Rule(), mergingAnt);
                        GetAntResult(mergingAnt, currentCases, minCasesPerRule, mergingTerms, data, attributes, results, currentRules, ref currentNumberForConvergence, ref currentAnt);
                    }
                    discoveredRules.Add(currentRules.OrderByDescending(item => item.Quality).First());
                    data.Cases = data.Cases.Except(discoveredRules.Last().CoveredCases).ToList();
                    discoveredRules.Last().Serialize();
                }
            }
            listBox1.HorizontalScrollbar = true;
            listBox1.DataSource = discoveredRules;
            uncoveredCount.Text = data.GetCasesCount().ToString();
            var header = data.Header;
            var dnf = new List<string>();
            foreach (var discoveredRule in discoveredRules)
            {
                var dnfRule = "Conditions: ";
                foreach (var condition in discoveredRule.ConditionsList)
                {
                    dnfRule += " x" + (header.IndexOf(condition.AttributeName) + 1) + " = " + condition.AttributeValue + " &";
                }
                dnfRule = dnfRule.Remove(dnfRule.Length - 1) + " Result: " + discoveredRule.Result;
                dnf.Add(dnfRule);
            }
            listBox2.DataSource = dnf;
        }

        private static void GetAntResult(Ant basicAnt, List<Case> currentCases, decimal minCasesPerRule, Terms terms, Table data,
            Attributes attributes, List<string> results, List<Rule> currentRules, ref int currentNumberForConvergence, ref int currentAnt)
        {
            var currentAntRule = basicAnt.RunAnt(currentCases, minCasesPerRule, terms, data, attributes, results);
            if (currentAntRule.ConditionsList.Count > 1)
            {
                currentAntRule = currentAntRule.PruneRule(data, results);
                currentAntRule.GetCoveredCases(data);
                currentAntRule.GetRuleResult(results);
                currentAntRule.CalculateRuleQuality(data);
                terms.UpdateWeights(currentAntRule);
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
            terms.Update(attributes, currentAntRule, basicAnt);
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

        private static void Initialize(out Table data, out Attributes attributes, out List<string> results, out Terms initialTerms)
        {
            data = Table.ReadData();
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
            initialTerms.FullInitialize(attributes);
            initialTerms.Serialize();
        }
    }
}
