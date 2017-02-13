﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MAClassification
{
    public class Terms : List<List<Term>>
    {
        private double SumEuristic { get; set; }
        private double SumEntropy { get; set; }

        public void Merge(Terms socialTerms, Terms basicTerms, Terms greedyTerms)
        {
            for (int i = 0; i < this.Count; i++)
            {
                for (int j = 0; j < this[i].Count; j++)
                {
                    this[i][j].WeightValue =
                        Math.Max(Math.Max(socialTerms[i][j].WeightValue, basicTerms[i][j].WeightValue),
                            greedyTerms[i][j].WeightValue);
                }
            }
        }

        public void UpdateWeights(Rule rule)
        {
            double sumWeight = .0;
            foreach (var items in this)
            {
                foreach (var item in items)
                {
                    item.IsChosen = false;
                    if (rule.ConditionsList != null && rule.ConditionsList.Exists(condition => condition.AttributeName == item.AttributeName &&
                                                                condition.AttributeValue == item.AttributeValue))
                    {
                        item.IsChosen = true;
                        item.WeightValue += item.WeightValue * rule.Quality;
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

        public void FullInitialize(Attributes attributes)
        {
            InitializeWeights(attributes);
            InitializeEuristicFunctionValues(attributes);
            //InitializeProbabilities(attributes);
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

        public void Update(Attributes attributes, Rule rule, Ant ant)
        {
            InitializeEuristicFunctionValues(attributes);
            InitializeProbabilities(attributes, ant.Alpha, ant.Beta);
        }

        private void InitializeWeights(Attributes attributes)
        {
            var attributesCount = attributes.GetValuesCount();
            foreach (var terms in this)
            {
                foreach (var term in terms)
                {
                    term.WeightValue = 1.0/attributesCount;
                }
            }
        }

        public void InitializeProbabilities(Attributes attributes, double alpha, double beta)
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

        private void InitializeEuristicFunctionValues(Attributes attributes)
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
                    if(this[i][j].IsChosen) continue;
                    result += Math.Pow(this[i][j].WeightValue, beta) * Math.Pow(this[i][j].EuristicFunctionValue, alpha);
                }
            }
            SumEuristic = result;
        }
    }
}
