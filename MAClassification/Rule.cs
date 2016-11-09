using System;
using System.Collections.Generic;
using System.Linq;

namespace MAClassification
{
    struct Rule
    {
        public class Condition
        {
            public string Attribute { get; set; }
            public string Value { get; set; }
        }

        public List<Condition> ConditionsList;

        public string Result { get; set; }

        public double Quality { get; set; }

        public List<Case> CoveredCases { get; set; }

        public Rule(List<Condition> data) : this()
        {
            ConditionsList = data;
            Result = "";
            Quality = 0;
        }

        public void AddConditionToRule(Terms terms, Table data)
        {
            ConditionsList = (ConditionsList ?? new List<Condition>());
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
                        break;
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
    }
}
