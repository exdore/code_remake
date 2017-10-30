using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MAClassification
{
    [Serializable]
    public enum TableTypes { Full, Training, Testing }

    [Serializable]
    [XmlInclude(typeof(Case))]
    [XmlInclude(typeof(TableTypes))]
    public class Table
    {
        public List<string> Header { get; set; }
        public List<Case> Cases { get; set; }
        public TableTypes TableType { get; set; }

        public Attributes GetAttributesInfo()
        {
            Attributes attributes = new Attributes();
            for (int i = 0; i < Header.Count; i++)
            {
                attributes.Add(new Attribute
                {
                    AttributeName = Header[i],
                    AttributeValues = Cases.Select(item => item.AttributesValuesList[i]).Distinct().ToList(),
                    IsUsed = false
                });
            }
            return attributes;
        }

        public Table()
        {

        }
        
        public void Serialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Table));
            StreamWriter streamWriter = new StreamWriter(TableType.ToString() + @"table.xml");
            xmlSerializer.Serialize(streamWriter, this);
            streamWriter.Close();
        }

        public Table Deserialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Table));
            var streamReader = new StreamReader(TableType + @"terms.xml");
            Table currentTable = (Table)xmlSerializer.Deserialize(streamReader);
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
            var apropriateCases = Cases.Where(item => item.AttributesValuesList[attributeIndex] == attributeValue).ToList();
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
            var streamReader = new StreamReader(path, System.Text.Encoding.GetEncoding(1251));
            var sourceData = new List<Case>();
            var line = streamReader.ReadLine();
            if (line != null)
            {
                var header = line.Split(' ').ToList();
                header.RemoveAt(header.Count - 1);
                var count = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var sourceList = line.Split(' ').ToList();
                    sourceData.Add(new Case
                    {
                        Number = ++count,
                        AttributesValuesList = sourceList.GetRange(1, sourceList.Count - 3),
                        Result = sourceList[sourceList.Count - 2]
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
