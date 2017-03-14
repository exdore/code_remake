using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MAClassification
{
    public class Terms : List<List<Term>>
    {
        private double SumEuristic { get; set; }
        private double SumEntropy { get; set; }

        public void Merge(Terms socialTerms, Terms basicTerms, Terms greedyTerms)
        {
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < this[i].Count; j++)
                {
                    this[i][j].WeightValue =
                        Math.Max(Math.Max(socialTerms[i][j].WeightValue, basicTerms[i][j].WeightValue),
                            greedyTerms[i][j].WeightValue);
                }
            }
        }

        public void UpdateWeights(Rule rule, GroupBox groupBox, int currentAnt)
        {
            var weightsType = groupBox.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked);
            double sumWeight = .0;
            foreach (var items in this)
            {
                foreach (var item in items)
                {
                    item.IsChosen = false;
                    if (rule.ConditionsList != null &&
                        rule.ConditionsList.Exists(condition => condition.AttributeName == item.AttributeName &&
                                                                condition.AttributeValue == item.AttributeValue))
                    {
                        item.IsChosen = true;
                        if (weightsType != null && weightsType.Name == "normalization")
                            item.WeightValue += item.WeightValue * rule.Quality;
                        else
                        {
                            var rate = CalculateEvaporationRate(currentAnt);
                            item.WeightValue = item.WeightValue * (1 - rate) + item.WeightValue * rule.Quality;
                        }
                    }
                    sumWeight += item.WeightValue;
                }
            }
            foreach (var items in this)
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

        public void Serialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Terms));
            StreamWriter streamWriter = new StreamWriter(@"terms.xml");
            xmlSerializer.Serialize(streamWriter, this);
            streamWriter.Close();
        }

        public Terms Deserialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Terms));
            var streamReader = new StreamReader(@"terms.xml");
            Terms currentTerms = (Terms)xmlSerializer.Deserialize(streamReader);
            streamReader.Close();
            return currentTerms;
        }

        public void FullInitialize(Attributes attributes, GroupBox groupBox, List<Case> cases)
        {
            InitializeWeights(attributes);
            var euristicType = groupBox.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked);
            if (euristicType != null && euristicType.Name == "entropy")
                CalculateEuristicFunctionValues(attributes);
            else CalculateEuristicsByDensity(attributes, cases);
        }

        public double CumulativeProbability(Attributes attributes)
        {
            double res = 0;
            for (int i = 0; i < Count; i++)
            {
                var terms = this[i];
                if (attributes[i].IsUsed) continue;
                for (int index = 0; index < terms.Count; index++)
                {
                    if (terms[index].IsChosen) continue;
                    res += this[i][index].Probability;
                }
            }
            return res;
        }

        public void Update(Attributes attributes, Ant ant, GroupBox groupBox, List<Case> cases)
        {
            CalculateEuristicFunctionValues(attributes);
            var euristicType = groupBox.Controls.OfType<RadioButton>().FirstOrDefault(item => item.Checked);
            if (euristicType != null && euristicType.Name == "entropy")
                CalculateEuristicFunctionValues(attributes);
            else CalculateEuristicsByDensity(attributes, cases);
        }

        private void InitializeWeights(Attributes attributes)
        {
            var attributesCount = attributes.GetValuesCount();
            foreach (var terms in this)
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
            for (int i = 0; i < Count; i++)
            {
                var terms = this[i];
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
            for (int i = 0; i < Count; i++)
            {
                var terms = this[i];
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
            foreach (var terms in this)
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
            for (int i = 0; i < Count; i++)
            {
                if (attributes[i].IsUsed) continue;
                for (int j = 0; j < this[i].Count; j++)
                {
                    if (this[i][j].IsChosen) continue;
                    result += Math.Log(attributes[i].AttributeValues.Count, 2) - this[i][j].Entropy;
                }
            }
            SumEntropy = result;
        }

        private void GetSumForEurictic(Attributes attributes, double alpha, double beta)
        {
            double result = 0;
            for (int i = 0; i < Count; i++)
            {
                if (attributes[i].IsUsed) continue;
                for (int j = 0; j < this[i].Count; j++)
                {
                    if (this[i][j].IsChosen) continue;
                    result += Math.Pow(this[i][j].WeightValue, beta) * Math.Pow(this[i][j].EuristicFunctionValue, alpha);
                }
            }
            SumEuristic = result;
        }
    }
}
