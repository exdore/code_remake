using System;

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


        public double GetEuristicFunctionValue(Attributes attributes, double sumEntropy)
        {
            if (Math.Abs(Entropy) > 1e-6)
                return (Math.Log(attributes.Find(item => item.AttributeName == AttributeName)
                            .AttributeValues.Count, 2) - Entropy) / sumEntropy;
            return 0;
        }

        public double GetProbabilityValue(double sumEuristic, double alpha, double beta)
        {
            return Math.Pow(EuristicFunctionValue, alpha) * Math.Pow(WeightValue, beta) / sumEuristic;
        }
    }
}