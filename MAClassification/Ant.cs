using System;
using System.Collections.Generic;
using System.Linq;

namespace MAClassification
{
    public class Ant
    {
        public double Alpha { get; set; }
        public double Beta { get; set; }

        public Rule RunAnt(List<Case> currentCases, decimal minCasesPerRule, Terms initialTerms, Table data,
            Attributes attributes, List<string> results)
        {
            var currentAntRule = new Rule
            {
                ConditionsList = new List<Condition>(),
                CoveredCases = currentCases
            };
            while (currentAntRule.CoveredCases.Count > minCasesPerRule)
            {
                currentAntRule.AddConditionToRule(initialTerms, data);
                currentAntRule.CheckUsedAttributes(attributes);
                currentAntRule.GetCoveredCases(data);
                currentAntRule.GetRuleResult(results);
                currentAntRule.CalculateRuleQuality(data);
                initialTerms.Update(attributes, currentAntRule);
                var prob = initialTerms.CumulativeProbability(attributes);
                if (Math.Abs(prob - 1) > 1e-6)
                {
                    continue;
                }
                currentAntRule.GetCoveredCases(data);
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
            return currentAntRule;
        }
    }
}
