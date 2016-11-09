﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MAClassification
{
    public class Terms : List<List<Term>>
    {
        public Terms TermsList;

        public Terms()
        {
            
        }

        public Terms(Terms terms)
        {
            this.TermsList = terms.TermsList;
        }

        public Terms InitializeTerms(List<Attribute> attributes, Table data, List<string> results, out int attributesValuesCount)
        {
            TermsList = new Terms();
            attributesValuesCount = GetAllAttributesValuesCount(attributes);
            foreach (Attribute attribute in attributes)
            {
                TermsList.Add(new List<Term>());
                foreach (string attributeValue in attribute.AttributeValues)
                {
                    TermsList.Last().Add(new Term
                    {
                        AttributeName = attribute.AttributeName,
                        AttributeValue = attributeValue,
                        Entropy = data.CalculateGain(attribute.AttributeName, attributeValue, results),
                        IsChosen = false,
                        WeightValue = (double)1 / attributesValuesCount
                    });
                }
            }
            return TermsList;
        }

        public double GetSumForEntopy(List<Attribute> attributes, Terms termsList)
        {
            double result = 0;
            for (int i = 0; i < Count; i++)
            {
                if(attributes[i].IsUsed) continue;
                for (int j = 0; j < termsList[i].Count; j++)
                {
                    result += Math.Log(attributes[i].AttributeValues.Count, 2) - termsList[i][j].Entropy;
                }
            }
            return result;
        }

        public double GetSumForEurictic(Terms termsList)
        {
            double result = 0;
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < termsList[i].Count; j++)
                {
                    result += termsList[i][j].WeightValue*termsList[i][j].EuristicFunctionValue;
                }
            }
            return result;
        }

        private static int GetAllAttributesValuesCount(List<Attribute> attributes)
        {
            var attributesValuesCount = 0;
            foreach (Attribute attribute in attributes)
            {
                for (int j = 0; j < attribute.AttributeValues.Count; j++)
                {
                    attributesValuesCount++;
                }
            }
            return attributesValuesCount;
        }
    }
}