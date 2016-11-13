using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;


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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Terms));
            StreamWriter streamWriter = new StreamWriter(@"terms.xml");
            xmlSerializer.Serialize(streamWriter, initialTermsList);
            streamWriter.Close();
            StreamReader streamReader = new StreamReader(@"terms.xml");
            Terms currentTerms = (Terms)xmlSerializer.Deserialize(streamReader);
            while (antsNumber > 0 && numberForConvergence > 0)
            {
                int coveredCasesCount = data.GetCases().Count;
                while (coveredCasesCount > minCasesPerRule)
                {
                    currentRule.AddConditionToRule(currentTerms, currentTable);
                    currentRule.CheckUsedAttributes(attributes);
                    currentTerms = new Terms(initialTermsList, attributes);
                    GetEuristicAndProbabilityValues(currentTerms, attributes);   //recalculate euristic & probability for unused attributes
                 //   var cumulative = currentTerms.CumulativeProbability();
                    currentRule.GetCoveredCases(currentTable);
                    coveredCasesCount = currentRule.CoveredCases.Count;
                    if (coveredCasesCount <= minCasesPerRule)
                    {
                        currentRule.ConditionsList.RemoveAt(currentRule.ConditionsList.Count - 1);
                        currentRule.GetCoveredCases(currentTable);
                        break;
                    }
                } 
                currentRule.GetRuleResult(results);
                currentRule.CalculateRuleQuality(data);
                var tempRule = currentRule.PruneRule(data, results);
                initialTermsList.UpdateWeights(tempRule);
                currentRules.Add(tempRule);
                if (tempRule.ConditionsList == currentRules.Last().ConditionsList)
                    numberForConvergence--;
            }
        }

        private static void GetEuristicAndProbabilityValues(Terms initialTermsList, List<Attribute> attributes)
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
