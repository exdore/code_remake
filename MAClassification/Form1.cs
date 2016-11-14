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
            int maxAntsNumber = 10;
            int maxNumberForConvergence = 3;
            const int maxUncoveredCases = 2;
            const int minCasesPerRule = 2;
            InitializeComponent();
            var data = Table.ReadData();
            var attributes = data.GetAttributesInfo();
            var results = data.GetResultsInfo();
            int attributesValuesCount;
            var discoveredRules = new List<Rule>();
            var initialTermsList = new Terms().InitializeTerms(attributes, data, results, out attributesValuesCount);
            foreach (var terms in initialTermsList)
            {
                foreach (var item in terms)
                {
                    item.WeightValue = (double)1 / attributesValuesCount;
                }
            }
            GetEuristicAndProbabilityValues(initialTermsList, attributes);
            
            List<Rule> currentRules = new List<Rule>();
            Table currentTable = new Table(data);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Terms));
            StreamWriter streamWriter = new StreamWriter(@"terms.xml");
            xmlSerializer.Serialize(streamWriter, initialTermsList);
            streamWriter.Close();
            while (currentTable.GetCases().Count > maxUncoveredCases)
            {
                var currentAntsNumber = maxAntsNumber;
                var currentNumberForConvergence = maxNumberForConvergence;
                var streamReader = new StreamReader(@"terms.xml");
                Terms currentTerms = (Terms) xmlSerializer.Deserialize(streamReader);
                while (currentAntsNumber > 0 && currentNumberForConvergence > 0)
                {
                    Rule currentRule = new Rule();
                    int coveredCasesCount = currentTable.GetCases().Count;
                    while (coveredCasesCount > minCasesPerRule)
                    {
                        currentRule.AddConditionToRule(currentTerms, currentTable);
                        currentRule.CheckUsedAttributes(attributes);
                        currentTerms = new Terms(initialTermsList, attributes);
                        GetEuristicAndProbabilityValues(currentTerms, attributes);
                            //recalculate euristic & probability for unused attributes
                        //   var cumulative = currentTerms.CumulativeProbability();
                        currentRule.GetCoveredCases(currentTable);
                        coveredCasesCount = currentRule.CoveredCases.Count;
                        if (coveredCasesCount <= minCasesPerRule)
                        {
                            currentRule.ConditionsList.RemoveAt(currentRule.ConditionsList.Count - 1);
                            attributes[currentRule.ConditionsList.Count - 1].IsUsed = false;
                            currentRule.GetCoveredCases(currentTable);
                            break;
                        }
                    }
                    currentRule.GetRuleResult(results);
                    currentRule.CalculateRuleQuality(currentTable);
                    var tempRule = currentRule.PruneRule(currentTable, results);
                    initialTermsList.UpdateWeights(tempRule);
                    currentRules.Add(tempRule);
                    if (tempRule.ConditionsList == currentRules.Last().ConditionsList)
                        currentNumberForConvergence--;
                    else currentNumberForConvergence = maxNumberForConvergence;
                    currentAntsNumber--;
                    foreach (var attribute in attributes)
                    {
                        attribute.IsUsed = false;
                    }
                }
                var bestRule = currentRules.OrderByDescending(item => item.Quality).First();
                discoveredRules.Add(bestRule);
                currentTable.Cases = currentTable.Cases.Except(bestRule.CoveredCases).ToList();
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
