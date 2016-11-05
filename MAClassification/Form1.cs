using System.Collections.Generic;
using System.Windows.Forms;


namespace MAClassification   
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var data = Table.ReadData();
            var attributes = data.GetAttributesInfo();
            var results = data.GetResultsInfo();
            int attributesValuesCount;
            var termsList = InitializeTerms(attributes, data, results, out attributesValuesCount);
        }

        private List<Term> InitializeTerms(List<Attribute> attributes, Table data, List<string> results, out int attributesValuesCount)
        {
            List<Term> termsList = new List<Term>();
            attributesValuesCount = GetAllAttributesValuesCount(attributes);
            foreach (Attribute attribute in attributes)
            {
                foreach (string attributeValue in attribute.AttributeValues)
                {
                    termsList.Add(new Term
                    {
                        AttributeName = attribute.AttributeName,
                        AttributeValue = attributeValue,
                        Entropy = data.CalculateGain(attribute.AttributeName, attributeValue, results),
                        IsChosen = false,
                        WeightValue = (double) 1/attributesValuesCount
                    });
                }
            }
            return termsList;
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
