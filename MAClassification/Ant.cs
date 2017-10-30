using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MAClassification
{
    public enum AntTypes { Greedy, Euristic, Basic}
    [Serializable]
    [XmlInclude(typeof(Rule))]
    public class Ant : Agent
    {
        public double Alpha { get; set; }
        public double Beta { get; set; }
        private Rule _rule;
        public override Rule Rule { get { return _rule; } set { _rule = value; } }
        public AntTypes AntType { get; set; }

        public Ant()
        { }

        public override void BuildRule(List<Case> currentCases, decimal minCasesPerRule, Terms initialTerms, Table data,
            Attributes attributes, List<string> results, GroupBox groupBox)
        {
            var currentAntRule = new Rule
            {
                ConditionsList = new List<Condition>(),
                CoveredCases = currentCases
            };
            _rule = currentAntRule;
            initialTerms.CalculateProbabilities(attributes, Alpha, Beta);
            while (currentAntRule.CoveredCases.Count > minCasesPerRule)
            {
                currentAntRule.AddConditionToRule(initialTerms, data);
                currentAntRule.CheckUsedAttributes(attributes);
                currentAntRule.GetCoveredCases(data);
                currentAntRule.GetRuleResult(results);
                currentAntRule.CalculateRuleQuality(data);
                initialTerms.Update(attributes, this, groupBox, currentCases);
            }
            if (currentAntRule.CoveredCases.Count < minCasesPerRule && currentAntRule.ConditionsList.Count > 1)
            {
                attributes.Find(item => item.AttributeName == currentAntRule.ConditionsList.Last().AttributeName)
                    .IsUsed = false;
                currentAntRule.ConditionsList.RemoveAt(currentAntRule.ConditionsList.Count - 1);
                currentAntRule.GetCoveredCases(data);
                currentAntRule.GetRuleResult(results);
                currentAntRule.CalculateRuleQuality(data);
            }
            _rule = currentAntRule;
        }
    }
}
