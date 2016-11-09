using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace MAClassification   
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            int antsNumber = 10;
            int numberForConvergence = 3;
    //        const int maxUncoveredCases = 2;
            const int minCasesPerRule = 2;
            InitializeComponent();
            var data = Table.ReadData();
            var attributes = data.GetAttributesInfo();
            var results = data.GetResultsInfo();
            int attributesValuesCount;
            var initialTermsList = new Terms().InitializeTerms(attributes, data, results, out attributesValuesCount);
            foreach (var terms in initialTermsList)
            {
                foreach (var item in terms)
                {
                    item.WeightValue = (double)1 / attributesValuesCount;
                }
            }
            GetEuristicAndProbabilityValues(initialTermsList, attributes);
            Rule currentRule = new Rule();
            List<Rule> currentRules = new List<Rule>();
            Table currentTable = new Table(data);
            Terms currentTerms = new Terms(initialTermsList,attributes);
            while (antsNumber > 0 && numberForConvergence > 0)
            {
                int coveredCasesCount;
                do
                {
                    currentRule.AddConditionToRule(currentTerms, currentTable);
                    currentRule.CheckUsedAttributes(attributes);
                    currentTerms = new Terms(initialTermsList, attributes);
                    GetEuristicAndProbabilityValues(currentTerms, attributes);
                    //recalculate euristic & probability for unused attributes
                    currentRule.GetCoveredCases(currentTable);
                    coveredCasesCount = currentRule.CoveredCases.Count;
                } 
                while (coveredCasesCount > minCasesPerRule);
                currentRules.Add(currentRule);
                //prune
                if (currentRule.ConditionsList == currentRules.Last().ConditionsList)
                    numberForConvergence--;
                //update weights
                //get covered by set of rules - should be bigger than maxUncoveredCases
            }
        }

        private void GetEuristicAndProbabilityValues(Terms initialTermsList, List<Attribute> attributes)
        {
            var sumEntropy = initialTermsList.GetSumForEntopy(attributes);
            foreach (var term in initialTermsList)
            {
                foreach (var item in term)
                {
                    item.EuristicFunctionValue = item.GetEuristicFunctionValue(attributes, sumEntropy);
                }
            }
            var sumEuristic = initialTermsList.GetSumForEurictic();
            foreach (var terms in initialTermsList)
            {
                foreach (var item in terms)
                {
                    item.Probability = item.GetProbability(sumEuristic);
                }
            }
        }
    }
}
