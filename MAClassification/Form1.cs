using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace MAClassification   
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            const int maxAntsNumber = 10;
            const int maxNumberForConvergence = 3;
            const int maxUncoveredCases = 2;
            const int minCasesPerRule = 2;
            InitializeComponent();
            Table data;
            Attributes attributes;
            List<string> results;
            Terms initialTerms;
            var discoveredRules = new List<Rule>();
            Initialize(out data, out attributes, out results, out initialTerms);
            data.Serialize();
            while (data.GetCasesCount() > maxUncoveredCases)
            {
                var currentAnt = 0;
                var currentNumberForConvergence = 0;
                var currentCases = data.GetCases();
                var currentRules = new List<Rule>();
                initialTerms = initialTerms.Deserialize();
                while (currentAnt < maxAntsNumber && currentNumberForConvergence < maxNumberForConvergence)
                {
                    var currentAntRule = new Rule
                    {
                        ConditionsList = new List<Condition>(),
                        CoveredCases = currentCases
                    };
                    while (currentAntRule.CoveredCases.Count > minCasesPerRule)
                    {
                        var prob = initialTerms.CumulativeProbability(attributes);
                        currentAntRule.AddConditionToRule(initialTerms, data);
                        currentAntRule.CheckUsedAttributes(attributes);
                        currentAntRule.GetCoveredCases(data);
                        currentAntRule.GetRuleResult(results);
                        currentAntRule.CalculateRuleQuality(data);
                        initialTerms.Update(attributes, currentAntRule);
                        prob = initialTerms.CumulativeProbability(attributes);
                        currentAntRule.GetCoveredCases(data);
                    }
                    attributes.Find(item => item.AttributeName == currentAntRule.ConditionsList.Last().Attribute)
                                .IsUsed = false;
                    currentAntRule.ConditionsList.RemoveAt(currentAntRule.ConditionsList.Count - 1);
                    currentAntRule.GetCoveredCases(data);
                    currentAntRule.GetRuleResult(results);
                    currentAntRule.CalculateRuleQuality(data);
                    currentAntRule = currentAntRule.PruneRule(data, results);
                    currentAntRule.GetCoveredCases(data);
                    currentAntRule.GetRuleResult(results);
                    currentAntRule.CalculateRuleQuality(data);
                    initialTerms.UpdateWeights(currentAntRule);
                    if (currentRules.Count == 0)
                    {
                        currentRules.Add(currentAntRule);
                        currentNumberForConvergence++;
                    }
                    else if (currentAntRule.ConditionsList == currentRules.Last().ConditionsList)
                    {
                        currentRules.Add(currentAntRule);
                        currentNumberForConvergence++;
                    }
                    else
                    {
                        currentRules.Add(currentAntRule);
                        currentNumberForConvergence = 0;
                    }
                    currentAnt++;
                }

            }
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
