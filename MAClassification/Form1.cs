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
            Terms initialTerms;
            var discoveredRules = new List<Rule>();
            Initialize(out data, out attributes, out results, out initialTerms);
            data.Serialize();
            File.Delete(@"rules.xml");
            while (data.GetCasesCount() > maxUncoveredCases)
            {
                var currentAnt = 0;
                var currentNumberForConvergence = 0;
                var currentCases = data.GetCases();
                var currentRules = new List<Rule>();
                initialTerms = initialTerms.Deserialize();
                foreach (var initialTerm in initialTerms)
                {
                    foreach (var term in initialTerm)
                    {
                        foreach (var discoveredRule in discoveredRules)
                        {
                            var fl =
                                discoveredRule.ConditionsList.Exists(
                                    item =>
                                        item.AttributeName == term.AttributeName &&
                                        item.AttributeValue == term.AttributeValue);
                            if (fl)
                                term.IsChosen = true;
                        }
                    }
                }
                while (currentAnt < maxAntsNumber && currentNumberForConvergence < maxNumberForConvergence)
                {
                    var currentAntRule = Ant.RunAnt(currentCases, minCasesPerRule, initialTerms, data, attributes, results);
                    if (currentAntRule.ConditionsList.Count > 1)
                    {
                        currentAntRule = currentAntRule.PruneRule(data, results);
                        currentAntRule.GetCoveredCases(data);
                        currentAntRule.GetRuleResult(results);
                        currentAntRule.CalculateRuleQuality(data);
                        initialTerms.UpdateWeights(currentAntRule);
                    }
                    if (currentRules.Count == 0)
                    {
                        currentRules.Add(currentAntRule);
                        currentNumberForConvergence++;
                    }
                    var diff = currentAntRule.ConditionsList.Except(currentRules.Last().ConditionsList).ToList();
                    if (diff.Count != 0)
                    {
                        currentRules.Add(currentAntRule);
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
                    foreach (var initialTerm in initialTerms)
                    {
                        foreach (var term in initialTerm)
                        {
                            term.IsChosen = false;
                        }
                    }
                    initialTerms.Update(attributes, currentAntRule);
                    currentAnt++;
                }
                discoveredRules.Add(currentRules.OrderByDescending(item => item.Quality).First());
                data.Cases = data.Cases.Except(discoveredRules.Last().CoveredCases).ToList();
                discoveredRules.Last().Serialize();
            }
            listBox1.DataSource = discoveredRules;
            uncoveredCount.Text = data.GetCasesCount().ToString();
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
