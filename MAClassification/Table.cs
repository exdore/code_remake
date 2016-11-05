using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MAClassification
{
    public class Table
    {
        public List<string> Header { get; set; }
        private List<Case> Cases { get; set; }
        public int CountColumns
        {
            get
            {
                return Cases[0].Attributes.Count - 1;
            }
        }
        private List<double> Gains { get; set; }
        private List<AdditionalCount> AdditionalCounts { get; set; }
        public List<double> CountsByColumns { get; set; }

        public List<Attribute> GetAttributesInfo()
        {
            List<Attribute> attributes = new List<Attribute>();
            for (int i = 0; i < CountColumns; i++)
            {
                attributes.Add(new Attribute
                {
                    AttributeName = Header[i+1],
                    AttributeValues = Cases.Select(item => item.Attributes[i]).Distinct().ToList(),
                    IsUsed = false
                });
            }
            return attributes;
        }

        public static Table ReadData()
        {
            var streamReader = new StreamReader(@"data.txt", System.Text.Encoding.GetEncoding(1251));
            var sourceData = new List<Case>();
            var line = streamReader.ReadLine();
            if (line != null)
            {
                var header = line.Split('\t').ToList();
                while ((line = streamReader.ReadLine()) != null)
                {
                    var sourceList = line.Split('\t').ToList();
                    sourceData.Add(new Case
                    {
                        Number = Convert.ToInt32(sourceList[0]),
                        Attributes = sourceList.GetRange(1,sourceList.Count-1)
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
    }
}
