using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

namespace MAClassification
{
    [Serializable]
    [XmlInclude(typeof(Case))]
    public class Table
    {
        public List<string> Header { get; set; }
        public List<Case> Cases { get; set; }

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

        public Table(List<Case> cases, Table data )
        {
            Cases = cases;
            Header = data.Header;
        }
        
        public void Serialize()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Table));
            StreamWriter streamWriter = new StreamWriter(@"table.xml");
            xmlSerializer.Serialize(streamWriter, this);
            streamWriter.Close();
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
                var header = line.Split('\t').ToList();
                header.RemoveAt(0);
                header.RemoveAt(header.Count - 1);
                while ((line = streamReader.ReadLine()) != null)
                {
                    var sourceList = line.Split('\t').ToList();
                    sourceData.Add(new Case
                    {
                        Number = Convert.ToInt32(sourceList[0]),
                        AttributesValuesList = sourceList.GetRange(1, sourceList.Count - 2),
                        Result = sourceList[sourceList.Count - 1]
                    });
                }
                streamReader.Close();
                return new Table
                {
                    Header = header,
                    Cases = sourceData
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
