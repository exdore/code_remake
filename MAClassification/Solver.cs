using System;
using System.Collections.Generic;
using System.Linq;
using MAClassification.Models;
using MAClassification.Serializators;

namespace MAClassification
{
    class Solver
    {
        public Table Data { get; set; }
        public List<Agent> Agents { get; set; }
        public List<Rule> Rules { get; set; }
        public Attributes Attributes { get; set; }
        public List<string> Classes { get; set; }

        private Terms Terms { get; set; }

        public CalculationOptions CalculationOptions { get; set; }

        public Table InitializeDataTables()
        {
            return new Table
            {
                Cases = Data.Cases,
                Header = Data.Header,
                TableType = TableTypes.Testing
            };
        }

        public Terms InitializeTerms()
        {
            Attributes = Data.GetAttributesInfo();
            Classes = Data.GetResultsInfo();
            var initialTerms = new Terms
            {
                TermsList = new List<List<Term>>(),
                TermType = TermTypes.Basic,
                MaxValue = 0.8,
                MinValue = 1e-5
            };
            foreach (var attribute in Attributes)
            {
                initialTerms.TermsList.Add(new List<Term>());
                foreach (var item in attribute.AttributeValues)
                {
                    initialTerms.TermsList.Last().Add(new Term
                    {
                        AttributeName = attribute.AttributeName,
                        AttributeValue = item,
                        Entropy = Data.CalculateGain(attribute.AttributeName, item, Classes)
                    });
                }
            }
            initialTerms.FullInitialize(Attributes, CalculationOptions.EuristicType, Data.Cases);
            Terms = initialTerms;
            return initialTerms;
        }

        public List<Rule> FindSolution(Table data)
        {
            Rules = new List<Rule>();
            Agents = new List<Agent>();
            while (data.GetCasesCount() > CalculationOptions.MaxUncoveredCases)
            {
                TermsSerializer ts = new TermsSerializer();
                var currentAgent = 0;
                var currentNumberForConvergence = 0;
                var currentRules = new List<Rule>();
                var socialTerms = ts.Deserialize(TermTypes.Euristic);
                var basicTerms = ts.Deserialize(TermTypes.Basic);
                var greedyTerms = ts.Deserialize(TermTypes.Greedy);
                var agentsPopulation = IterateGeneration(data, ref currentAgent, ref currentNumberForConvergence, currentRules,
                    socialTerms, basicTerms, greedyTerms);
                agentsPopulation = agentsPopulation.OrderByDescending(item => item.Rule.Quality).ToList();
                var bestAgent = agentsPopulation.First();
                Rules.Add(bestAgent.Rule);
                data.Cases = data.Cases.Except(bestAgent.Rule.CoveredCases).ToList();
            }
            return Rules;
        }

        private List<Agent> IterateGeneration(Table data, ref int currentAnt, ref int currentNumberForConvergence, List<Rule> currentRules, Terms socialTerms, Terms basicTerms, Terms greedyTerms)
        {
            var agentsPopulation = new List<Agent>();
            var count = 0;
            while (count < CalculationOptions.MaxAntsGenerationsNumber * 3 && currentNumberForConvergence < CalculationOptions.MaxNumberForConvergence)
            {
                var rnd = new Random();
                var basicAnt = new Ant
                {
                    AgentType = AgentTypes.Ant,   // rewrite
                    Alpha = 1,
                    Beta = 1,
                    AntType = AntTypes.Basic,
                    AntNumber = ++count
                };
                var greedyAnt = new Ant
                {
                    AgentType = AgentTypes.Ant,
                    Alpha = rnd.Next(200, 500) / 100.0,
                    Beta = rnd.Next(500, 1000) / 100.0,
                    AntType = AntTypes.Greedy,
                    AntNumber = ++count
                };
                var socialAnt = new Ant
                {
                    AgentType = AgentTypes.Ant,
                    Alpha = rnd.Next(500, 1000) / 100.0,
                    Beta = rnd.Next(200, 500) / 100.0,
                    AntType = AntTypes.Euristic,
                    AntNumber = ++count
                };
                RuleData ruleData = new RuleData()
                {
                    Attributes = Attributes,
                    Classes = Classes,
                    MinCasesPerRule = this.CalculationOptions.MinCasesPerRule,
                    Table = data,
                    Terms = basicTerms
                };

                
                basicAnt.GetAntResult(ruleData, currentRules, CalculationOptions, ref currentNumberForConvergence, ref currentAnt);
                ruleData.Terms = greedyTerms;
                greedyAnt.GetAntResult(ruleData, currentRules, CalculationOptions, ref currentNumberForConvergence, ref currentAnt);
                ruleData.Terms = socialTerms;
                socialAnt.GetAntResult(ruleData, currentRules, CalculationOptions, ref currentNumberForConvergence, ref currentAnt);
                agentsPopulation.Add(basicAnt);
                agentsPopulation.Add(greedyAnt);
                agentsPopulation.Add(socialAnt);
            }
            return agentsPopulation;
        }

        public int Test(List<Rule> discoveredRules)                             //move to other class -> should work with List<List<Rule>> from all trees
        {
            //var data = _fullData.Deserialize();
            var data = InitializeDataTables();
            data.Cases = data.Cases.OrderBy(item => item.Number).ToList();
            var realResults = new List<string>();
            foreach (var item in data.Cases)
            {
                realResults.Add(item.Class);
                item.Class = "";
            }
            //foreach (var discoveredRule in discoveredRules)
            //{
            //    discoveredRule.GetCoveredCases(data);
            //    foreach (var discoveredRuleCoveredCase in discoveredRule.CoveredCases)
            //    {
            //        data.Cases.Find(item => item.Number == discoveredRuleCoveredCase.Number).Class =
            //            discoveredRule.Class;
            //    }
            //    data.Cases = data.Cases.Except(discoveredRule.CoveredCases).ToList();
            //}
            var rules = new List<List<Rule>>();
            foreach (var @case in data.Cases)
            {
                var res = new List<Rule>();
                foreach (var rule in discoveredRules)
                {
                    if (rule.CheckIfCovers(@case, Attributes))
                        res.Add(rule);
                }
                if (res.Count != 0)
                    rules.Add(res);
            }
            var predictedResults = new List<string>();
            foreach (var items in rules)
            {
                var groups = items.GroupBy(s => s.Class).ToList();
                groups = groups.OrderByDescending(s => s.Sum(item => item.Quality)).ToList();
                predictedResults.Add(groups.First().Key);
            }
            var count = 0;
            for (var index = 0; index < predictedResults.Count; index++)
            {
                if (predictedResults[index] != realResults[index])
                    count++;
            }
            return count;
        }
    }
}
