using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MAClassification
{
    [Serializable]
    public enum TermTypes { Basic, Greedy, Euristic }

    [Serializable]
    [XmlInclude(typeof(TermTypes))]
    public class Terms
    {
        public List<List<Term>> TermsList { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public TermTypes TermType { get; set; }
        private double SumEuristic { get; set; }
        private double SumEntropy { get; set; }
       

        public void UpdateWeights(Rule rule, string type, int currentAnt)
        {
            double sumWeight = .0;
            foreach (var items in TermsList)
            {
                foreach (var item in items)
                {
                    item.IsChosen = false;
                    if (rule.ConditionsList != null &&
                        rule.ConditionsList.Exists(condition => condition.AttributeName == item.AttributeName &&
                                                                condition.AttributeValue == item.AttributeValue))
                    {
                        item.IsChosen = true;
                        if (type == "normalization")
                            item.WeightValue += item.WeightValue * rule.Quality;
                        else
                        {
                            var rate = CalculateEvaporationRate(currentAnt);
                            item.WeightValue = item.WeightValue * (1 - rate) + item.WeightValue * rule.Quality;
                        }
       
                    }
                    if (item.WeightValue < MinValue)
                        item.WeightValue = MinValue;
                    if (item.WeightValue > MaxValue)
                        item.WeightValue = MaxValue;
                    sumWeight += item.WeightValue;
                }
            }
            foreach (var items in TermsList)
            {
                foreach (var item in items)
                {
                    if (!item.IsChosen)
                        item.WeightValue /= sumWeight;
                }
            }
        }

        private double CalculateEvaporationRate(int currentAnt)
        {
            return 3.0 / 2 * CalculateFunctionForEvaporation(currentAnt) - 3.0 / 2 * CalculateFunctionForEvaporation(0); // magic from 2016 article
        }

        private double CalculateFunctionForEvaporation(int currentAnt)
        {
            var sigma = 10; // magic from 2016 article
            return 1.0 / sigma / Math.Sqrt(2 * Math.PI) * Math.Exp(-currentAnt ^ 2 / 2 / sigma ^ 2);
        }

       

        public void FullInitialize(Attributes attributes, string type, List<Case> cases)
        {
            InitializeWeights(attributes);
            if (type == "entropy")
                CalculateEuristicFunctionValues(attributes);
            else CalculateEuristicsByDensity(attributes, cases);
        }

        public double CumulativeProbability(Attributes attributes)
        {
            double res = 0;
            for (int i = 0; i < TermsList.Count; i++)
            {
                var terms = TermsList[i];
                if (attributes[i].IsUsed) continue;
                for (int index = 0; index < terms.Count; index++)
                {
                    if (terms[index].IsChosen) continue;
                    res += TermsList[i][index].Probability;
                }
            }
            return res;
        }

        public void Update(Attributes attributes, double a, double b, string type, List<Case> cases)
        {
            CalculateEuristicFunctionValues(attributes);
            if (type == "entropy")
                CalculateEuristicFunctionValues(attributes);
            else CalculateEuristicsByDensity(attributes, cases);
            CalculateProbabilities(attributes, a, b);
        }

        private void InitializeWeights(Attributes attributes)
        {
            var attributesCount = attributes.GetValuesCount();
            foreach (var terms in TermsList)
            {
                foreach (var term in terms)
                {
                    term.WeightValue = 1.0 / attributesCount;
                }
            }
        }

        public void CalculateProbabilities(Attributes attributes, double alpha, double beta)
        {
            GetSumForEurictic(attributes, alpha, beta);
            for (int i = 0; i < TermsList.Count; i++)
            {
                var terms = TermsList[i];
                if (attributes[i].IsUsed) continue;
                foreach (var term in terms)
                {
                    if (term.IsChosen) continue;
                    term.Probability = term.GetProbabilityValue(SumEuristic, alpha, beta);
                }
            }
        }

        private void CalculateEuristicFunctionValues(Attributes attributes)
        {
            GetSumForEntopy(attributes);
            for (int i = 0; i < TermsList.Count; i++)
            {
                var terms = TermsList[i];
                if (attributes[i].IsUsed) continue;
                foreach (var term in terms)
                {
                    if (term.IsChosen) continue;
                    term.EuristicFunctionValue = term.GetEuristicFunctionValue(attributes, SumEntropy);
                }
            }
        }

        private void CalculateEuristicsByDensity(Attributes attributes, List<Case> cases)
        {
            foreach (var terms in TermsList)
            {
                foreach (var term in terms)
                {
                    var attribute = attributes.FindIndex(item => item.AttributeName == term.AttributeName);
                    if (attributes[attribute].IsUsed) continue;
                    var correctCasesCount =                             //для DAG обернуть в if по radioButton и проверять класс записи
                        cases.Count(item => item.AttributesValuesList[attribute] == term.AttributeValue);
                    if (correctCasesCount != 0)
                        term.EuristicFunctionValue = correctCasesCount * 1.0 / cases.Count;
                    else
                        term.EuristicFunctionValue = 0;
                }
            }
        }

        private void GetSumForEntopy(Attributes attributes)
        {
            double result = 0;
            for (int i = 0; i < TermsList.Count; i++)
            {
                if (attributes[i].IsUsed) continue;
                for (int j = 0; j < TermsList[i].Count; j++)
                {
                    if (TermsList[i][j].IsChosen) continue;
                    result += Math.Log(attributes[i].AttributeValues.Count, 2) - TermsList[i][j].Entropy;
                }
            }
            SumEntropy = result;
        }

        private void GetSumForEurictic(Attributes attributes, double alpha, double beta)
        {
            double result = 0;
            for (int i = 0; i < TermsList.Count; i++)
            {
                if (attributes[i].IsUsed) continue;
                for (int j = 0; j < TermsList[i].Count; j++)
                {
                    if (TermsList[i][j].IsChosen) continue;
                    result += Math.Pow(TermsList[i][j].WeightValue, beta) * Math.Pow(TermsList[i][j].EuristicFunctionValue, alpha);
                }
            }
            SumEuristic = result;
        }
    }
}
