using System;
using System.Collections.Generic;
using System.Linq;

namespace MAClassification
{
    public class Rule : ICloneable
    {
        public List<Condition> ConditionsList;

        public string Result { get; set; }

        public double Quality { get; set; }

        public List<Case> CoveredCases { get; set; }

        public Rule()
        {

        }

        public Rule(Rule rule)
        {
            ConditionsList = rule.ConditionsList;
            Result = rule.Result;
            Quality = rule.Quality;
        }

        public Rule(List<Condition> conditions, string result, double quality)
        {
            ConditionsList = conditions;
            Result = result;
            Quality = quality;
        }

        public void GetRuleResult(List<string> resultsList)
        {
            var result = "";
            int max = 0;
            foreach (var item in resultsList)
            {
                if (max < CoveredCases.Count(@case => @case.Result == item))
                {
                    result = item;
                    max = CoveredCases.Count(@case => @case.Result == item);
                }
            }
            Result = result;
        }

        public void CalculateRuleQuality(Table data)
        {
            int truePositive = this.CoveredCases.Count(item => item.Result == this.Result);
            int falsePositive = this.CoveredCases.Count(item => item.Result != this.Result);
            var uncoveredData = data.GetCases().Except(this.CoveredCases).ToList();
            int trueNegative = uncoveredData.Count(item => item.Result == this.Result);
            int falseNegative = uncoveredData.Count(item => item.Result != this.Result);
            Quality = (double)truePositive*trueNegative/(truePositive + falseNegative)/(falsePositive + trueNegative);
        }

        public void AddConditionToRule(Terms terms, Table data)
        {
            ConditionsList = ConditionsList ?? new List<Condition>();
            var probability = GetSomeProbability();
            foreach (var term in terms)
            {
                foreach (var item in term)
                {
                    if (probability < item.Probability)
                    {
                        ConditionsList.Add(new Condition
                        {
                            Attribute = item.AttributeName,
                            Value = item.AttributeValue
                        });
                        return;
                    }
                    probability -= item.Probability;
                }
            }
        }

        public double GetSomeProbability()
        {
            return new Random().NextDouble();
        }

        public void GetCoveredCases(Table data)
        {
            var cases = data.GetCases();
            foreach (var condition in ConditionsList)
            {
                int attributeIndex = data.Header.FindIndex(item => item == condition.Attribute);
                cases = cases.Where(item => item.AttributesValuesList[attributeIndex] == condition.Value).ToList();
            }
            CoveredCases = cases;
        }

        public void CheckUsedAttributes(List<Attribute> attributes)
        {
            foreach (var condition in ConditionsList)
            {
                var index = attributes.FindIndex(item => item.AttributeName == condition.Attribute);
                attributes[index].IsUsed = true;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
