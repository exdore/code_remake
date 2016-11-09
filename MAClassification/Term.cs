using System;
using System.Collections.Generic;

namespace MAClassification
{
    public class Term
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public bool IsChosen { get; set; }
        public double WeightValue { get; set; }
        public double EuristicFunctionValue { get; set; }
        public double Probability { get; set; }
        public double Entropy { get; set; }

        public double GetEuristicFunctionValue(List<Attribute> attributes, Term term, double sumEntropy)
        {
            return (Math.Log(attributes.Find(item => item.AttributeName == term.AttributeName)
                .AttributeValues.Count, 2) - term.Entropy) / sumEntropy;
        }

        public double GetProbability(Term term, double sumEuristic)
        {
            return term.EuristicFunctionValue * term.WeightValue / sumEuristic;
        }
    }
}