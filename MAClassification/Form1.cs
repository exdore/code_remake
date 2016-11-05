using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace MAClassification   
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var data = ReadData();
        }

        private static Table ReadData()
        {
            var streamReader = new StreamReader(@"data.txt", System.Text.Encoding.GetEncoding(1251));
            var sourceData = new List<Case>();
            int lineNumber = 0;
            var line = streamReader.ReadLine();
            if (line != null)
            {
                var header = line.Split('\t').ToList();
                while ((line = streamReader.ReadLine()) != null)
                {
                    var sourceList = line.Split('\t').ToList();
                    sourceData.Add(new Case
                    {
                        Number = lineNumber + 1,
                        Attributes = sourceList
                    });
                    lineNumber++;
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
