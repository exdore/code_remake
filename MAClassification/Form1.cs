using System;
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
            int minCasesPerRule = 3;            //минимальное покрытие каждым правилом
            int maxUncoveredCases = 2;
            InitializeComponent();
            var data = ReadData();              
            data.CountsByColumns = CalculateCountsByColumns(data);
            data.AdditionalCounts = CalculateAdditionalCounts(data);
            var gains = CalculateGains(data);
            List<Rule> rulesSet = new List<Rule>();
            while(data.Cases.Count > maxUncoveredCases)
            {
                int numberOfAnts = 10;
                List<Rule> iterationRuleSet = new List<Rule>();
                while (numberOfAnts-- > 0)
                {
                    Rule rule = new Rule();
                    rule.Conditions = new List<Rule.Condition>();                    
                    var attributesCount = data.CountColumns;
                    var attributesValuesCount = 2;                                          //потом разное у каждого атрибута
                    //List<Weight> weights = new List<Weight>();
                    bool[] used = new bool[attributesCount];
                    double[,] weights = new double[attributesValuesCount, attributesCount];   //на листы, вложенные - разной длины
                    InitializeWeights(attributesCount, attributesValuesCount, used, weights);
                    double[,] euristicValues = new double[attributesValuesCount, attributesCount];
                    double[,] probabilities = new double[attributesValuesCount, attributesCount];
                    var coveredData = data.Cases;
                    int res = data.Cases.Count;
                    while (true)
                    {
                        euristicValues = new double[attributesValuesCount, attributesCount];
                        probabilities = new double[attributesValuesCount, attributesCount];
                        CalculateEuristicFunction(attributesCount, attributesValuesCount, gains, used, euristicValues);
                        CalculateProbabilities(attributesCount, attributesValuesCount, used, weights, euristicValues, probabilities); // проверить чтобы не было херни из-за ебучих ссылок
                        int attributeIndex;
                        int attributeValue;
                        GetProbability(attributesCount, attributesValuesCount, probabilities, out attributeIndex, out attributeValue, used);
                        rule.Conditions.Add(new Rule.Condition
                                           {
                                               Index = attributeIndex,
                                               Value = attributeValue
                                           });
                        used[attributeIndex] = true;
                        coveredData = coveredData.Where(item => item.Attributes[rule.Conditions.Last().Index] == rule.Conditions.Last().Value).ToList();
                        res = coveredData.Count;
                        if (res < minCasesPerRule)
                        {
                            rule.Conditions.RemoveAt(rule.Conditions.Count - 1);
                            break;
                        }
                    }
                    coveredData = GetCoveredCases(rule, data.Cases).ToList();
                    var uncoveredData = data.Cases.Except(coveredData).ToList();
                    CalculateRuleConsequent(ref rule, coveredData);
                    rule.Quality = CalculateRuleQuality(rule, coveredData, uncoveredData);
                    iterationRuleSet.Add(SelectRule(data, rule));
                }
                iterationRuleSet = iterationRuleSet.OrderByDescending(item => item.Quality).ToList();
                rulesSet.Add(iterationRuleSet.First());
                data.Cases = data.Cases.Except(GetCoveredCases(rulesSet.Last(), data.Cases)).ToList();
            }
        }

        private Rule SelectRule(Table data, Rule best)
        {
            while (best.Conditions.Count > 1)
            {
                List<Rule> rulesList = new List<Rule>();
                for (int i = 0; i < best.Conditions.Count; i++)
                {
                    rulesList.Add(PruneRule(best, data, i));
                }
                rulesList = rulesList.OrderByDescending(item => item.Quality).ToList();
                if (best.Quality <= rulesList.First().Quality)
                    best = rulesList.First();
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
            int truePositive = coveredData.Where(item => item.Attributes.Last() == rule.Result).Count();
            int falsePositive = coveredData.Where(item => item.Attributes.Last() != rule.Result).Count();
            int trueNegative = uncoveredData.Where(item => item.Attributes.Last() != rule.Result).Count();
            int falseNegative = uncoveredData.Where(item => item.Attributes.Last() == rule.Result).Count();
            double ruleQuality = (double)truePositive / (truePositive + falseNegative) * trueNegative / (falsePositive + trueNegative);
            return ruleQuality;
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

        private static double GetSumForEuristic(List<List<double>> gains, int attributesValuesCount, bool[] used)
        {
            double res = 0;
            for (int i = 0; i < gains.Count; i++)
            {
                for(int j = 0; j < gains[i].Count; j++)
                    res += ((used[i] == false)? 1: 0) * (Math.Log(attributesValuesCount, 2) - gains[i][j]);
            }
            return res;
        }

        private static double GetSumForProbability(int attributesValuesCount, int attributesCount, double[,] weights, double[,] euristicValues, bool[] used)
        {
            double res = 0;
            for (int i = 0; i < attributesCount; i++)
            {
                for (int j = 0; j < attributesValuesCount; j++)
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

        private static void GetProbability(int attributesCount, int attributesValuesCount, double[,] probabilities, out int attributeIndex, out int attributeValue, bool[] used) //костыльные методы 
        {
            List<double> cumulativeProbabilities = new List<double>();
            double res = 0;
            for (int i = 0; i < attributesCount; i++)
            {
                for (int j = 0; j < attributesValuesCount; j++)
                    res += probabilities[j, i];
                cumulativeProbabilities.Add(res);
            }
            Random rnd = new Random();
            double currProb = rnd.NextDouble();
            attributeIndex = cumulativeProbabilities.FindIndex(item => item > currProb);          //возможно баг на краях
            var tempIndex = attributeIndex;
            attributeIndex = cumulativeProbabilities.FindLastIndex(item => item == cumulativeProbabilities[tempIndex]);
            attributeIndex = (attributeIndex == 0) ? 1 : attributeIndex - 1;
            currProb -= cumulativeProbabilities[attributeIndex];
            attributeIndex = tempIndex;
            if (currProb < probabilities[0, attributeIndex])
                attributeValue = 1;
            else attributeValue = 0;
        }

        private static void CalculateProbabilities(int attributesCount, int attributesValuesCount, bool[] used, double[,] weights, double[,] euristicValues, double[,] probabilities)
        {
            var sumForProbability = GetSumForProbability(attributesValuesCount, attributesCount, weights, euristicValues, used);
            for (int i = 0; i < attributesCount; i++)
            {
                if (used[i]) 
                    continue;
                for (int j = 0; j < attributesValuesCount; j++)
                {
                    probabilities[j, i] = weights[j, i] * euristicValues[j, i] / sumForProbability;
                }
            }
        }

        private static void CalculateEuristicFunction(int attributesCount, int attributesValuesCount, List<List<double>> gains, bool[] used, double[,] euristicValues)
        {
            var sumGainsForEurictic = GetSumForEuristic(gains, attributesValuesCount, used);
            for (int i = 0; i < attributesCount; i++)
            {
                if (used[i]) continue;
                for (int j = 0; j < attributesValuesCount; j++)
                {
                    euristicValues[j, i] = (Math.Log(attributesValuesCount, 2) - gains[i][j]) / sumGainsForEurictic;
                }
            }
        }
        private static double GetSumWeigthForAttribute(double[,] weights, double[,] euristicValues, int i, int attributesValuesCount)
        {
            double res = 0;
            for (int j = 0; j < attributesValuesCount; j++)
                res += euristicValues[j, i] * weights[j, i];
            return res;
        }

        private static double GetSumGainsForAttribute(List<double> gains)
        {
            return gains.Sum();
        }


        private static void InitializeWeights(int attributesCount, int attributesValuesCount, bool[] used, double[,] weights) // data.Cases
        {
            for (int i = 0; i < attributesCount; i++)
            {
                used[i] = false;
                for (int j = 0; j < attributesValuesCount; j++)
                {
                    weights[j, i] = 1.0 / (attributesCount * attributesValuesCount);
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
