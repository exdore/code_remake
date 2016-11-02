using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MAClassification   
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            int MinCasesPerRule = 3;            //минимальное покрытие каждым правилом
            int NumberOfAnts = 10;
            InitializeComponent();
            List<Rule> RuleSet = new List<Rule>();
            var data = ReadData();              
            data.CountsByColumns = CalculateCountsByColumns(data);
            data.AdditionalCounts = CalculateAdditionalCounts(data);
            var gains = CalculateGains(data);
            while (NumberOfAnts-- > 0)
            {
                Rule rule = new Rule();
                rule.Conditions = new List<Rule.Condition>();
                                           
                var AttributesCount = data.CountColumns;
                var AttributesValuesCount = 2;                                          //потом разное у каждого атрибута
                //List<Weight> weights = new List<Weight>();
                bool[] used = new bool[AttributesCount];
                double[,] weights = new double[AttributesValuesCount, AttributesCount];   //на листы, вложенные - разной длины
                InitializeWeights(AttributesCount, AttributesValuesCount, used, weights);
                double[,] euristicValues = new double[AttributesValuesCount, AttributesCount];
                double[,] probabilities = new double[AttributesValuesCount, AttributesCount];
                var coveredData = data.Cases;
                int res = data.Cases.Count;
                while (true)
                {
                    euristicValues = new double[AttributesValuesCount, AttributesCount];
                    probabilities = new double[AttributesValuesCount, AttributesCount];
                    CalculateEuristicFunction(AttributesCount, AttributesValuesCount, gains, used, euristicValues);
                    CalculateProbabilities(AttributesCount, AttributesValuesCount, used, weights, euristicValues, probabilities); // проверить чтобы не было херни из-за ебучих ссылок
                    int AttributeIndex;
                    int AttributeValue;
                    GetProbability(AttributesCount, AttributesValuesCount, probabilities, out AttributeIndex, out AttributeValue, used);
                    rule.Conditions.Add(new Rule.Condition
                                       {
                                           Index = AttributeIndex,
                                           Value = AttributeValue
                                       });
                    used[AttributeIndex] = true;
                    coveredData = coveredData.Where(item => item.Attributes[rule.Conditions.Last().Index] == rule.Conditions.Last().Value).ToList();
                    res = coveredData.Count;
                    if (res < MinCasesPerRule)
                    {
                        rule.Conditions.RemoveAt(rule.Conditions.Count - 1);
                        break;
                    }
                }
                coveredData = GetCoveredCases(rule, data.Cases).ToList();
                var uncoveredData = data.Cases.Except(coveredData).ToList();
                CalculateRuleConsequent(ref rule, coveredData);
                rule.Quality = CalculateRuleQuality(rule, coveredData, uncoveredData);
                RuleSet.Add(SelectRule(data, rule));
            }
        }

        private Rule SelectRule(Table data, Rule best)
        {
            while (best.Conditions.Count > 1)
            {
                List<Rule> RulesList = new List<Rule>();
                for (int i = 0; i < best.Conditions.Count; i++)
                {
                    RulesList.Add(PruneRule(best, data, i));
                }
                RulesList = RulesList.OrderByDescending(item => item.Quality).ToList();
                if (best.Quality <= RulesList.First().Quality)
                    best = RulesList.First();
            }
            return best;
        }

        private static void CalculateRuleConsequent(ref Rule rule, List<Case> coveredData)
        {
            int oneCount = coveredData.Where(item => item.Attributes.Last() == 1).Count();
            int zeroCount = coveredData.Where(item => item.Attributes.Last() == 0).Count();
            rule.Result = (oneCount >= zeroCount) ? 1 : 0;
        }

        private static double CalculateRuleQuality(Rule rule, List<Case> coveredData, List<Case> uncoveredData)
        {
            int TruePositive = coveredData.Where(item => item.Attributes.Last() == rule.Result).Count();
            int FalsePositive = coveredData.Where(item => item.Attributes.Last() != rule.Result).Count();
            int TrueNegative = uncoveredData.Where(item => item.Attributes.Last() != rule.Result).Count();
            int FalseNegative = uncoveredData.Where(item => item.Attributes.Last() == rule.Result).Count();
            double RuleQuality = (double)TruePositive / (TruePositive + FalseNegative) * TrueNegative / (FalsePositive + TrueNegative);
            return RuleQuality;
        }

        private Rule PruneRule(Rule rule, Table data, int i)
        {
            var tempData = data.Cases;
            var newRule = new Rule(rule.Conditions.Except(rule.Conditions.Where(item => item == rule.Conditions[i])).ToList()); 
            var coveredData = GetCoveredCases(newRule, tempData);
            var uncoveredData = tempData.Except(coveredData).ToList();
            CalculateRuleConsequent(ref newRule, coveredData);
            newRule.Quality = CalculateRuleQuality(newRule, coveredData, uncoveredData);
            return newRule;
        }

        private static double GetSumForEuristic(List<List<double>> gains, int AttributesValuesCount, bool[] used)
        {
            double res = 0;
            for (int i = 0; i < gains.Count; i++)
            {
                for(int j = 0; j < gains[i].Count; j++)
                    res += ((used[i] == false)? 1: 0) * (Math.Log(AttributesValuesCount, 2) - gains[i][j]);
            }
            return res;
        }

        private static double GetSumForProbability(int AttributesValuesCount, int AttributesCount, double[,] weights, double[,] euristicValues, bool[] used)
        {
            double res = 0;
            for (int i = 0; i < AttributesCount; i++)
            {
                for (int j = 0; j < AttributesValuesCount; j++)
                    res += ((used[i] == false)? 1: 0) * (weights[j, i] * euristicValues[j, i]);
            }
            return res;
        }

        private List<Case> GetCoveredCases(Rule newRule, List<Case> tempData)
        {
            for (int i = 0; i < newRule.Conditions.Count; i++)
                tempData = tempData.Where(item => item.Attributes[newRule.Conditions[i].Index] == newRule.Conditions[i].Value).ToList();
            return tempData;
        }

        private static void GetProbability(int AttributesCount, int AttributesValuesCount, double[,] probabilities, out int AttributeIndex, out int AttributeValue, bool[] used) //костыльные методы 
        {
            List<double> CumulativeProbabilities = new List<double>();
            double res = 0;
            for (int i = 0; i < AttributesCount; i++)
            {
                for (int j = 0; j < AttributesValuesCount; j++)
                    res += probabilities[j, i];
                CumulativeProbabilities.Add(res);
            }
            Random rnd = new Random();
            double CurrProb = rnd.NextDouble();
            AttributeIndex = CumulativeProbabilities.FindIndex(item => item > CurrProb);          //возможно баг на краях
            var tempIndex = AttributeIndex;
            AttributeIndex = CumulativeProbabilities.FindLastIndex(item => item == CumulativeProbabilities[tempIndex]);
            AttributeIndex = (AttributeIndex == 0) ? 1 : AttributeIndex - 1;
            CurrProb -= CumulativeProbabilities[AttributeIndex];
            AttributeIndex = tempIndex;
            if (CurrProb < probabilities[0, AttributeIndex])
                AttributeValue = 1;
            else AttributeValue = 0;
        }

        private static void CalculateProbabilities(int AttributesCount, int AttributesValuesCount, bool[] used, double[,] weights, double[,] euristicValues, double[,] probabilities)
        {
            var sumForProbability = GetSumForProbability(AttributesValuesCount, AttributesCount, weights, euristicValues, used);
            for (int i = 0; i < AttributesCount; i++)
            {
                if (used[i]) 
                    continue;
                for (int j = 0; j < AttributesValuesCount; j++)
                {
                    probabilities[j, i] = weights[j, i] * euristicValues[j, i] / sumForProbability;
                }
            }
        }

        private static void CalculateEuristicFunction(int AttributesCount, int AttributesValuesCount, List<List<double>> gains, bool[] used, double[,] euristicValues)
        {
            var sumGainsForEurictic = GetSumForEuristic(gains, AttributesValuesCount, used);
            for (int i = 0; i < AttributesCount; i++)
            {
                if (used[i]) continue;
                for (int j = 0; j < AttributesValuesCount; j++)
                {
                    euristicValues[j, i] = (Math.Log(AttributesValuesCount, 2) - gains[i][j]) / sumGainsForEurictic;
                }
            }
        }
        private static double GetSumWeigthForAttribute(double[,] weights, double[,] euristicValues, int i, int AttributesValuesCount)
        {
            double res = 0;
            for (int j = 0; j < AttributesValuesCount; j++)
                res += euristicValues[j, i] * weights[j, i];
            return res;
        }

        private static double GetSumGainsForAttribute(List<double> gains)
        {
            return gains.Sum();
        }


        private static void InitializeWeights(int AttributesCount, int AttributesValuesCount, bool[] used, double[,] weights) // data.Cases
        {
            for (int i = 0; i < AttributesCount; i++)
            {
                used[i] = false;
                for (int j = 0; j < AttributesValuesCount; j++)
                {
                    weights[j, i] = 1.0 / (AttributesCount * AttributesValuesCount);
                }
            }
        }

        private static List<double> CalculateCountsByColumns(Table sourceData)
        {
            var counts = new List<double>();
            for (int j = 0; j < sourceData.Cases[0].Attributes.Count; j++)
            {
                int count = sourceData.Cases.Sum(item => item.Attributes[j]);
                counts.Add(count);
            }
            return counts;
        }

        private static List<List<double>> CalculateGains(Table sourceData)
        {
            var gains = new List<List<double>>();
            for (int i = 0; i < sourceData.CountColumns; i++)
            {
                gains.Add(new List<double>());
                var zeroCount = sourceData.Cases.Count - sourceData.CountsByColumns[i];
                var oneAndOne = sourceData.AdditionalCounts[i].OneAndOneCounts / sourceData.CountsByColumns[i];
                var logOneAndOne = (oneAndOne != 0.0) ? Math.Log(oneAndOne, 2) : 0;
                var oneAndZero = sourceData.AdditionalCounts[i].OneAndZeroCounts / sourceData.CountsByColumns[i];
                var logOneAndZero = (oneAndZero != 0.0) ? Math.Log(oneAndZero, 2) : 0;
                var zeroAndOne = sourceData.AdditionalCounts[i].ZeroAndOneCounts / zeroCount;
                var logZeroAndOne = (zeroAndOne != 0.0) ? Math.Log(zeroAndOne, 2) : 0;
                var zeroAndZero = sourceData.AdditionalCounts[i].ZeroAndZeroCounts / zeroCount;
                var logZeroAndZero = (zeroAndZero != 0.0) ? Math.Log(zeroAndZero, 2) : 0;
                double gain1 = (-oneAndOne * logOneAndOne - oneAndZero * logOneAndZero);
                double gain2 = (-zeroAndOne * logZeroAndOne - zeroAndZero * logZeroAndZero);
                gains[i].Add(gain1);
                gains[i].Add(gain2);
            }
            return gains;
        }

        private static List<AdditionalCount> CalculateAdditionalCounts(Table sourceData)
        {
            var additionalCounts = new List<AdditionalCount>();
            for (int j = 0; j < sourceData.CountColumns; j++)
            {
                AdditionalCount additinalCount = new AdditionalCount();
                for (int i = 0; i < sourceData.Cases.Count; i++)
                {
                    if (sourceData.Cases[i].Attributes[j] == 1 && sourceData.Cases[i].Attributes[sourceData.CountColumns] == 1)
                    {
                        additinalCount.OneAndOneCounts++;
                    }
                    if (sourceData.Cases[i].Attributes[j] == 1 && sourceData.Cases[i].Attributes[sourceData.CountColumns] == 0)
                    {
                        additinalCount.OneAndZeroCounts++;
                    }
                    if (sourceData.Cases[i].Attributes[j] == 0 && sourceData.Cases[i].Attributes[sourceData.CountColumns] == 1)
                    {
                        additinalCount.ZeroAndOneCounts++;
                    }
                    if (sourceData.Cases[i].Attributes[j] == 0 && sourceData.Cases[i].Attributes[sourceData.CountColumns] == 0)
                    {
                        additinalCount.ZeroAndZeroCounts++;
                    }
                }
                additionalCounts.Add(additinalCount);
            }
            return additionalCounts;
        }

        private static Table ReadData()
        {
            var streamReader = new StreamReader(@"data.txt", System.Text.Encoding.GetEncoding(1251));
            string line;
            var sourceData = new List<Case>();
            int lineNumber = 0;
            line = streamReader.ReadLine();
            var header = line.Split('\t').ToList();
            while ((line = streamReader.ReadLine()) != null)
            {
                var sourceList = line.Split('\t').Select(item => Convert.ToInt32(item))
                                                 .ToList();
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
    }
}
