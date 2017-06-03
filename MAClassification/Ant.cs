﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MAClassification
{
    [Serializable]
    [XmlInclude(typeof(Rule))]
    public class Ant
    {
        public double Alpha { get; set; }
        public double Beta { get; set; }
        public string Type { get; set; }
        private Rule _rule;

        public void RunAnt(List<Case> currentCases, decimal minCasesPerRule, Terms initialTerms, Table data,
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
            //if (currentAntRule.CoveredCases.Count < minCasesPerRule && currentAntRule.ConditionsList.Count > 1)
            //{
            //    attributes.Find(item => item.AttributeName == currentAntRule.ConditionsList.Last().AttributeName)
            //        .IsUsed = false;
            //    currentAntRule.ConditionsList.RemoveAt(currentAntRule.ConditionsList.Count - 1);
            //    currentAntRule.GetCoveredCases(data);
            //    currentAntRule.GetRuleResult(results);
            //    currentAntRule.CalculateRuleQuality(data);
            //}
            _rule = currentAntRule;
        }

        public Rule GetRule()
        {
            return _rule;
        }

        public void SetRule(Rule rule)
        {
            _rule = rule.Clone();
        }
    }
}
