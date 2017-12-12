using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ArffSharp;

namespace MAClassification
{
    [Serializable]
    [XmlInclude(typeof(Case))]
    [XmlInclude(typeof(TableTypes))]

    public class Table
    {
        public enum TableTypes
        {
            Full,
            Training,
            Testing
        }

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
                    Result = reader.Attributes.Last().NominalValues[record.Values.Last().NominalValueIndex]
                };
                for (var i = 0; i < record.Values.Length - 1; i++)
                {
                    cs.AttributesValuesList.Add(reader.Attributes[i].NominalValues[record.Values[i].NominalValueIndex]);
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
                attributes.Add(new Attribute
                {
                    AttributeName = Header[i],
                    AttributeValues = Cases.Select(item => item.AttributesValuesList[i]).Distinct().ToList(),
                    IsUsed = false
                });
            return attributes;
        }

        public void Serialize()
        {
            var xmlSerializer = new XmlSerializer(typeof(Table));
            var streamWriter = new StreamWriter(TableType + @"Table.xml");
            xmlSerializer.Serialize(streamWriter, this);
            streamWriter.Close();
        }

        public Table Deserialize()
        {
            var xmlSerializer = new XmlSerializer(typeof(Table));
            var streamReader = new StreamReader(TableType + @"Table.xml");
            var currentTable = (Table) xmlSerializer.Deserialize(streamReader);
            streamReader.Close();
            return currentTable;
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
                var casesWithSetResultCount = apropriateCases.Count(item => item.Result == sample);
                if (casesWithSetResultCount != 0)
                    result -= (double) casesWithSetResultCount / apropriateCasesCount *
                              Math.Log((double) casesWithSetResultCount / apropriateCasesCount, 2);
            }
            return result;
        }

        public static Table ReadData(string path)
        {
            var streamReader = new StreamReader(path, Encoding.GetEncoding(1251));
            var sourceData = new List<Case>();
            var line = streamReader.ReadLine();
            if (line != null)
            {
                var header = line.Split('\t').ToList();
                header.RemoveAt(header.Count - 1);
                var count = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var sourceList = line.Split('\t').ToList();
                    sourceData.Add(new Case
                    {
                        Number = ++count,
                        AttributesValuesList = sourceList.GetRange(0, header.Count),
                        Result = sourceList[header.Count]
                    });
                }
                streamReader.Close();
                return new Table
                {
                    Header = header,
                    Cases = sourceData,
                    TableType = TableTypes.Full
                };
            }
            return null;
        }

        public List<string> GetResultsInfo()
        {
            return Cases.Select(item => item.Result).Distinct().ToList();
        }
    }
}