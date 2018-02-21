using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using MAClassification.Models;

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
        public override Rule Rule
        {
            get => _rule;
            set => _rule = value;
        }
        public AntTypes AntType { get; set; }
        public int AntNumber { get; set; }

        public override void BuildRule(RuleData data, EuristicTypes euristicType, PheromonesTypes pheromonesTypes)
        {
            var currentAntRule = new Rule
            {
                ConditionsList = new List<RuleCondition>(),
                CoveredCases = data.Table.Cases
            };
            _rule = currentAntRule;
            data.Terms.CalculateProbabilities(data.Attributes, Alpha, Beta);
            while (currentAntRule.CoveredCases.Count > data.MinCasesPerRule)
            {
                currentAntRule.AddConditionToRule(data.Terms);
                currentAntRule.CheckUsedAttributes(data.Attributes);
                currentAntRule.GetCoveredCases(data.Table);
                currentAntRule.GetRuleResult(data.Classes);
                currentAntRule.CalculateRuleQuality(data.Table);
                data.Terms.Update(data.Attributes, Alpha, Beta, euristicType, data.Table.Cases);
                data.Terms.UpdateWeights(currentAntRule, pheromonesTypes, AntNumber);
            }
            if (currentAntRule.ConditionsList.Count > 1 && currentAntRule.CoveredCases.Count < data.MinCasesPerRule)
            {
                data.Attributes.Find(item => item.AttributeName == currentAntRule.ConditionsList.Last().AttributeName)
                    .IsUsed = false;
                currentAntRule.ConditionsList.RemoveAt(currentAntRule.ConditionsList.Count - 1);
                currentAntRule.GetCoveredCases(data.Table);
                currentAntRule.GetRuleResult(data.Classes);
                currentAntRule.CalculateRuleQuality(data.Table);
            }
            _rule = currentAntRule;
        }

        public override void GetAntResult(RuleData ruleData, List<Rule> currentRules, CalculationOptions options, ref int currentNumberForConvergence, ref int currentAnt)
        {
            CheckUsedTerms(ruleData.Table, ruleData.Terms);
            BuildRule(ruleData, options.EuristicType, options.PheromonesType);
            var currentAntRule = Rule;
            if (options.IsPruned)
            {
                currentAntRule = currentAntRule.PruneRule(ruleData.Table, ruleData.Classes);
                currentAntRule.GetCoveredCases(ruleData.Table);
                currentAntRule.GetRuleResult(ruleData.Classes);
                currentAntRule.CalculateRuleQuality(ruleData.Table);
                ruleData.Terms.UpdateWeights(currentAntRule, options.PheromonesType, currentAnt);
            }
            if (currentRules.Count > 0)
            {
                if (currentAntRule.Equals(currentRules.Last()))
                    currentNumberForConvergence++;
                else
                    currentNumberForConvergence = 0;
            }
            currentRules.Add(currentAntRule);
            foreach (var attribute in ruleData.Attributes)
            {
                attribute.IsUsed = false;
            }
            ruleData.Terms.Update(ruleData.Attributes, Alpha, Beta, options.EuristicType, ruleData.Table.Cases);
            currentAnt++;
            if (options.IsPruned)
            {
                Rule = currentAntRule;
            }
        }

        private void CheckUsedTerms(Table data, Terms terms)
        {
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
                            fl = true;
                            break;
                        }
                    }
                    if (!fl) item.IsChosen = true;
                }
            }
        }
    }
}
