using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using MAClassification.Models;

namespace MAClassification
{
    [Serializable]
    [XmlInclude(typeof(Condition))]
    public class Rule 
    {
        public List<Condition> ConditionsList { get; set; }

        public string Class { get; set; }             //make it array of probabilities to save likehood for every class 

        public double Quality { get; set; }
        private double Precision { get; set; }
        private double Specificity { get; set; }

        [XmlIgnore]
        public List<Case> CoveredCases { get; set; }

        public override string ToString()
        {
            ConditionsList = ConditionsList.OrderBy(item => item.AttributeName).ToList();
            var res = "Conditions: ";
            foreach (var condition in ConditionsList)
            {
                res += condition + " & ";
            }
            if (res.Length > 0) res = res.Remove(res.Length - 2);
            res += " Class: " + Class + " Covered Cases Count: " + CoveredCases.Count;
            return res;
        }

        public void GetRuleResult(List<string> resultsList)
        {
            var result = "";
            int max = 0;
            foreach (var item in resultsList)
            {
                var currentResultCases = CoveredCases.Count(@case => @case.Class == item);
                if (max < currentResultCases)
                {
                    result = item;
                    max = currentResultCases;
                }
            }
            Class = result;
        }

        public void CalculateRuleQuality(Table data)
        {
            int truePositive = CoveredCases.Count(item => item.Class == Class);
            int falsePositive = CoveredCases.Count(item => item.Class != Class);
            var uncoveredData = data.GetCases().Except(CoveredCases).ToList();
            int trueNegative = uncoveredData.Count(item => item.Class == Class);
            int falseNegative = uncoveredData.Count(item => item.Class != Class);
            Precision = double.IsNaN((double)truePositive / (truePositive + falseNegative)) ? 0
                        : (double)truePositive / (truePositive + falseNegative);
            Specificity = double.IsNaN((double)trueNegative / (falsePositive + trueNegative)) ? 0
                        : (double)trueNegative / (falsePositive + trueNegative);
            Quality = Precision * Specificity;
        }

        public void AddConditionToRule(Terms terms)
        {
            var probability = new Random().NextDouble();
            foreach (var term in terms.TermsList)
            {
                if (ConditionsList.Exists(item => item.AttributeName == term[0].AttributeName)) continue;
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

        public bool CheckIfCovers(Case @case, Attributes attributes)
        {
            foreach (var condition in ConditionsList)
            {
                if (condition.AttributeValue != @case.AttributesValuesList[attributes.
                    FindIndex(item => item.AttributeName == condition.AttributeName)]) // Rewrite in Dictionary, make Dictionary(key - rule index?, values - bool[] - Numbers of covered cases)
                    return false;
            }

            return true;
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

        public void CheckUsedAttributes(List<Models.Attribute> attributes)
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
                Precision = Precision,
                Specificity = Specificity,
                Quality = Quality,
                CoveredCases = CoveredCases.ToList(),
                Class = Class
            };
        }

        public Rule PruneRule(Table data, List<string> resultsList) //решение проблемы оверфиттинга через обобщение правил
        {
            var newRule = Clone();
            try
            {
                if (newRule.ConditionsList.Count == 1)
                    return newRule;
                if (newRule.ConditionsList.Count == 0)
                {
                    throw (new Exception(""));
                }
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
                return bestRule.Quality > Quality ? bestRule.PruneRule(data, resultsList) : this;
            }
            catch(Exception)
            {
                throw (new Exception(@"Empty rule!!!"));
                //return newRule;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rule);
        }

        public bool Equals(Rule other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Math.Abs(Quality - other.Quality) < 1e-6 && CoveredCases.SequenceEqual(other.CoveredCases);
        }

        public override int GetHashCode()
        {
            var hashCode = 541764673;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Condition>>.Default.GetHashCode(ConditionsList);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Class);
            hashCode = hashCode * -1521134295 + Quality.GetHashCode();
            hashCode = hashCode * -1521134295 + Precision.GetHashCode();
            hashCode = hashCode * -1521134295 + Specificity.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Case>>.Default.GetHashCode(CoveredCases);
            return hashCode;
        }

        public static bool operator ==(Rule rule1, Rule rule2)
        {
            return EqualityComparer<Rule>.Default.Equals(rule1, rule2);
        }

        public static bool operator !=(Rule rule1, Rule rule2)
        {
            return !(rule1 == rule2);
        }
    }
}
