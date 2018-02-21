using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ArffSharp;

namespace MAClassification
{
    [Serializable]
    
    public enum TableTypes
    {
        Full,
        Training,
        Testing
    }

    [XmlInclude(typeof(Case))]
    [XmlInclude(typeof(TableTypes))]
    public class Table
    {
        

        public List<string> Header { get; set; }
        public List<Case> Cases { get; set; }
        public TableTypes TableType { get; set; }

        public static Table CreateTable(List<ArffRecord> records, ArffReader reader)
        {
            Table t = new Table
            {
                Header = new List<string>(),
                Cases = new List<Case>()
            };
            for (var i = 0; i < reader.Attributes.Count - 1; i++)
            {
                var attribute = reader.Attributes[i];
                t.Header.Add(attribute.Name);
            }
            var count = 1;
            foreach (var record in records)
            {
                var cs = new Case
                {
                    Number = count,
                    AttributesValuesList = new List<string>(),
                    Class = reader.Attributes.Last().NominalValues[record.Values.Last().NominalValueIndex]
                };
                for (var i = 0; i < record.Values.Length - 1; i++)
                {
                    if (record.Values[i].NominalValueIndex != -1)
                        cs.AttributesValuesList.Add(reader.Attributes[i]
                            .NominalValues[record.Values[i].NominalValueIndex]);
                    else
                        cs.AttributesValuesList.Add(reader.Attributes[i]
                            .NominalValues[0]);
                }
                count++;
                t.Cases.Add(cs);
            }
            return t;
        }

        public Attributes GetAttributesInfo()
        {
            var attributes = new Attributes();
            for (var i = 0; i < Header.Count; i++)
                attributes.Add(new Models.Attribute
                {
                    AttributeName = Header[i],
                    AttributeValues = Cases.Select(item => item.AttributesValuesList[i]).Distinct().ToList(),
                    IsUsed = false
                });
            return attributes;
        }

        

        public int GetCasesCount()
        {
            return Cases.Count;
        }

        public List<Case> GetCases()
        {
            return Cases;
        }

        public double CalculateGain(string attributeName, string attributeValue, List<string> resultsList)
        {
            double result = 0;
            var attributeIndex = Header.IndexOf(attributeName);
            var apropriateCases = Cases.Where(item => item.AttributesValuesList[attributeIndex] == attributeValue)
                .ToList();
            var apropriateCasesCount = apropriateCases.Count;
            foreach (var sample in resultsList)
            {
                var casesWithSetResultCount = apropriateCases.Count(item => item.Class == sample);
                if (casesWithSetResultCount != 0)
                    result -= (double) casesWithSetResultCount / apropriateCasesCount *
                              Math.Log((double) casesWithSetResultCount / apropriateCasesCount, 2);
            }
            return result;
        }

        public List<string> GetResultsInfo()
        {
            return Cases.Select(item => item.Class).Distinct().ToList();
        }
    }
}