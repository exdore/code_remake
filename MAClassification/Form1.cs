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
            var termsList = new Terms().InitializeTerms(attributes, data, results, out attributesValuesCount);
            var sumEntropy = termsList.GetSumForEntopy(attributes, termsList);
            foreach (var term in termsList)
            {
                foreach (var item in term)
                {
                    item.EuristicFunctionValue = item.GetEuristicFunctionValue(attributes, item, sumEntropy);
                }
            }
            var sumEuristic = termsList.GetSumForEurictic(termsList);
            foreach (var term in termsList)
            {
                foreach (var item in term)
                {
                    item.Probability = item.GetProbability(item, sumEuristic);
                }
            }
        }
    }
}
