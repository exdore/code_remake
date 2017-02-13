using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MAClassification
{
    [Serializable]
    [XmlInclude(typeof(Condition))]
    public class Rule 
    {
        public List<Condition> ConditionsList;

        public string Result { get; set; }

        public double Quality { get; set; }
        [XmlIgnore]
        public List<Case> CoveredCases { get; set; }

        public override string ToString()
        {
            var res = "Conditions: ";
            foreach (var condition in ConditionsList)
            {
                res += condition + " & ";
            }
            if(res.Length > 0) res = res.Remove(res.Length - 2);
            res += " Result: " + Result + " Covered Cases Count: " + CoveredCases.Count;
            return res;
        }

        public void Serialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Rule));
            StreamWriter streamWriter = new StreamWriter(@"rules.xml", true);
            xmlSerializer.Serialize(streamWriter, this);
            streamWriter.Close();
        }

        public void GetRuleResult(List<string> resultsList)
        {
            var result = "";
            int max = 0;
            foreach (var item in resultsList)
            {
                var currentResultCases = CoveredCases.Count(@case => @case.Result == item);
                if (max < currentResultCases)
                {
                    result = item;
                    max = currentResultCases;
                }
            }
            Result = result;
        }

        public void CalculateRuleQuality(Table data)
        {
            int truePositive = CoveredCases.Count(item => item.Result == Result);
            int falsePositive = CoveredCases.Count(item => item.Result != Result);
            var uncoveredData = data.GetCases().Except(CoveredCases).ToList();
            int trueNegative = uncoveredData.Count(item => item.Result == Result);
            int falseNegative = uncoveredData.Count(item => item.Result != Result);
            Quality = (double)truePositive * trueNegative / (truePositive + falseNegative) / (falsePositive + trueNegative);
        }

        public void AddConditionToRule(Terms terms, Table data)
        {
            var probability = new Random().NextDouble();
            foreach (var term in terms)
            {
                if(ConditionsList.Exists(item => item.AttributeName == term[0].AttributeName)) continue;
                foreach (var item in term)
                {
                    if (item.IsChosen) continue;
                    if (probability < item.Probability)
                    {
                        ConditionsList.Add(new Condition
                        {
                            AttributeName = item.AttributeName,
                            AttributeValue = item.AttributeValue
                        });
                        return;
                    }
                    probability -= item.Probability;
                }
            }
        }

        public void GetCoveredCases(Table data)
        {
            var cases = data.GetCases();
            foreach (var condition in ConditionsList)
            {
                int attributeIndex = data.Header.FindIndex(item => item == condition.AttributeName);
                cases = cases.Where(item => item.AttributesValuesList[attributeIndex] == condition.AttributeValue).ToList();
            }
            CoveredCases = cases;
        }

        public void CheckUsedAttributes(List<Attribute> attributes)
        {
            foreach (var condition in ConditionsList)
            {
                var index = attributes.FindIndex(item => item.AttributeName == condition.AttributeName);
                attributes[index].IsUsed = true;
            }
        }

        public Rule Clone()
        {
            return new Rule
            {
                ConditionsList = ConditionsList.ToList(),
                Quality = Quality,
                CoveredCases = CoveredCases.ToList(),
                Result = Result
            };
        }

        public Rule PruneRule(Table data, List<string> resultsList) //решение проблемы оверфиттинга через обобщение правил
        {
            var newRule = Clone();
            if (newRule.ConditionsList.Count == 1) return newRule;
            var rulesList = new List<Rule>();
            for (int i = 0; i < ConditionsList.Count; i++)
            {
                newRule = Clone();
                newRule.ConditionsList.RemoveAt(i);
                newRule.GetCoveredCases(data);
                newRule.GetRuleResult(resultsList);
                newRule.CalculateRuleQuality(data);
                rulesList.Add(newRule);
            }
            var bestRule = rulesList.OrderByDescending(item => item.Quality).First();
            return (bestRule.Quality > Quality) ? bestRule.PruneRule(data, resultsList) : this;
        }
    }
}
