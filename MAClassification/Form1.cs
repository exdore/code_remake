using System.Collections.Generic;
using System.Windows.Forms;


namespace MAClassification   
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            int antsNumber = 10;
            int numberForConvergence = 3;
            const int maxUncoveredCases = 2;
            const int minCasesPerRule = 2;
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
            foreach (var terms in termsList)
            {
                foreach (var item in terms)
                {
                    item.Probability = item.GetProbability(item, sumEuristic);
                }
            }
            Rule currentRule = new Rule();
            while (antsNumber > 0 && numberForConvergence > 0)
            {
                List<Rule> currentRules = new List<Rule>();
                int coveredCasesCount;
                do
                {
                    currentRule.AddConditionToRule(termsList, data);
                    currentRule.GetCoveredCases(data);
                    coveredCasesCount = currentRule.CoveredCases.Count;
                } while (coveredCasesCount > minCasesPerRule);
                //prune
                //update weights
                currentRules.Add(currentRule);
                //get covered by set of rules - should be bigger than maxUncoveredCases
            }
        }
    }
}
