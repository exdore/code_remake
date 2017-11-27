using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MAClassification
{
    public enum AntTypes { Greedy, Euristic, Basic }
    [Serializable]
    [XmlInclude(typeof(Rule))]
    [XmlInclude(typeof(AntTypes))]
    public class Ant : Agent
    {
        public double Alpha { get; set; }
        public double Beta { get; set; }
        private Rule _rule;
        public override Rule Rule { get => _rule;
            set => _rule = value;
        }
        public AntTypes AntType { get; set; }
        public int AntNumber { get; set; }
        public Ant()
        { }

        public override void BuildRule(decimal minCasesPerRule, Terms initialTerms, Table data, Attributes attributes,
            List<string> results, string type)
        {
            var currentAntRule = new Rule
            {
                ConditionsList = new List<Condition>(),
                CoveredCases = data.Cases
            };
            _rule = currentAntRule;
            initialTerms.CalculateProbabilities(attributes, Alpha, Beta);
            while (currentAntRule.CoveredCases.Count > minCasesPerRule)
            {
                currentAntRule.AddConditionToRule(initialTerms);
                currentAntRule.CheckUsedAttributes(attributes);
                currentAntRule.GetCoveredCases(data);
                currentAntRule.GetRuleResult(results);
                currentAntRule.CalculateRuleQuality(data);
                initialTerms.Update(attributes, Alpha, Beta, type, data.Cases);
                initialTerms.UpdateWeights(currentAntRule, type, AntNumber);
            }
            if (currentAntRule.ConditionsList.Count > 1 && currentAntRule.CoveredCases.Count < minCasesPerRule)
            {
                attributes.Find(item => item.AttributeName == currentAntRule.ConditionsList.Last().AttributeName)
                    .IsUsed = false;
                currentAntRule.ConditionsList.RemoveAt(currentAntRule.ConditionsList.Count - 1);
                currentAntRule.GetCoveredCases(data);
                currentAntRule.GetRuleResult(results);
                currentAntRule.CalculateRuleQuality(data);
            }
            if(currentAntRule.CoveredCases.Count == 0)
            {
                int k = 5;
            }
            _rule = currentAntRule;
        }

        public override void GetAntResult(decimal minCasesPerRule, Terms terms, Table data,
            Attributes attributes, List<string> results, List<Rule> currentRules, string euristicType, string pheromonesType, string pruningType, ref int currentNumberForConvergence, ref int currentAnt)
        {
            {
                CheckUsedTerms(data, terms);
                BuildRule(minCasesPerRule, terms, data, attributes, results, pheromonesType);
                var currentAntRule = Rule;
                if (pruningType == @"pruningActive")
                {
                    currentAntRule = currentAntRule.PruneRule(data, results);
                    currentAntRule.GetCoveredCases(data);
                    currentAntRule.GetRuleResult(results);
                    currentAntRule.CalculateRuleQuality(data);
                    terms.UpdateWeights(currentAntRule, pheromonesType, currentAnt);
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
                terms.Update(attributes, Alpha, Beta, euristicType, data.Cases);
                currentAnt++;
                if (pruningType == @"pruningActive")
                {
                    Rule = currentAntRule;
                }
            }
        }

        private bool CheckUsedTerms(Table data, Terms terms)
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
    }
}
