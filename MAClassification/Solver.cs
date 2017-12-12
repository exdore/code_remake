using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArffSharp;

namespace MAClassification
{
    class Solver
    {
        public Table Data { get => _fullData; set => _fullData = value; }
        public List<Agent> Agents { get => _chosenAgents; set => _chosenAgents = value; }
        public List<Rule> Rules { get => _discoveredRules; set => _discoveredRules = value; }
        public Attributes Attributes { get => _attributes; set => _attributes = value; }
        public List<string> Results { get => _results; set => _results = value; }

        public int MaxAntsGenerationsNumber { get; set; }
        public int MaxNumberForConvergence { get; set; }
        public int MaxUncoveredCases { get; set; }
        public int MinCasesPerRule { get; set; }

        public string EuristicFunctionType { get; set; }
        public string PheromonesUpdateMethod { get; set; }
        public string RulesPruningStatus { get; set; }

        public double _crossValidationCoefficient { get; set; }
        private List<Rule> _discoveredRules { get; set; }
        private Table _fullData { get; set; }
        public Table _trainingTable { get; private set; }
        public Table _testingTable { get; private set; }
        public string _dataPath { get; set; }
        private Attributes _attributes { get; set; }
        private List<string> _results { get; set; }
        private Terms _terms { get; set; }
        private List<Agent> _chosenAgents { get; set; }

        public Table GetFullData()
        {
            return _fullData;
        }

        public void InitializeDataTables()
        {
            _testingTable = new Table
            {
                Cases = _fullData.Cases,
                Header = _fullData.Header,
                TableType = Table.TableTypes.Testing
            };
            _testingTable.Serialize();
        }

        public Terms InitializeTerms()
        {
            _attributes = _fullData.GetAttributesInfo();
            _results = _fullData.GetResultsInfo();
            var initialTerms = new Terms
            {
                TermsList = new List<List<Term>>(),
                TermType = TermTypes.Basic,
                MaxValue = 0.8,
                MinValue = 0.01
            };
            foreach (var attribute in _attributes)
            {
                initialTerms.TermsList.Add(new List<Term>());
                foreach (var item in attribute.AttributeValues)
                {
                    initialTerms.TermsList.Last().Add(new Term
                    {
                        AttributeName = attribute.AttributeName,
                        AttributeValue = item,
                        Entropy = _fullData.CalculateGain(attribute.AttributeName, item, _results)
                    });
                }
            }
            initialTerms.FullInitialize(_attributes, EuristicFunctionType, _fullData.Cases);
            _terms = initialTerms;
            return initialTerms;
        }

        public List<Rule> FindSolution(Table data)
        {
            _discoveredRules = new List<Rule>();
            _chosenAgents = new List<Agent>();
            while (data.GetCasesCount() > MaxUncoveredCases)
            {
                var currentAgent = 0;
                var currentNumberForConvergence = 0;
                var currentRules = new List<Rule>();
                var socialTerms = _terms.Deserialize();
                socialTerms.TermType = TermTypes.Euristic;
                var basicTerms = _terms.Deserialize();
                basicTerms.TermType = TermTypes.Basic;
                var greedyTerms = _terms.Deserialize();
                greedyTerms.TermType = TermTypes.Greedy;
                var agentsPopulation = IterateGeneration(data, Attributes, Results, ref currentAgent, ref currentNumberForConvergence, currentRules,
                    socialTerms, basicTerms, greedyTerms);
                agentsPopulation = agentsPopulation.OrderByDescending(item => item.Rule.Quality).ToList();
                var bestAgent = agentsPopulation.First();
                _chosenAgents.Add(agentsPopulation.Where(item => (item.Rule.Quality - bestAgent.Rule.Quality) < 1e-5)
                    .OrderByDescending(item => Guid.NewGuid()).First());
                _discoveredRules.Add(_chosenAgents.Last().Rule);
                data.Cases = data.Cases.Except(_chosenAgents.Last().Rule.CoveredCases).ToList();
            }
            return _discoveredRules;
        }

        private List<Agent> IterateGeneration(Table data, Attributes attributes, List<string> results, ref int currentAnt, ref int currentNumberForConvergence, List<Rule> currentRules, Terms socialTerms, Terms basicTerms, Terms greedyTerms)
        {
            var agentsPopulation = new List<Agent>();
            var count = 0;
            while (count < MaxAntsGenerationsNumber * 3 && currentNumberForConvergence < MaxNumberForConvergence)
            {
                var rnd = new Random();
                var basicAnt = new Ant
                {
                    AgentType = Agent.AgentTypes.ant,   // rewrite
                    Alpha = 1,
                    Beta = 1,
                    AntType = AntTypes.Basic,
                    AntNumber = ++count
                };
                var greedyAnt = new Ant
                {
                    AgentType = Agent.AgentTypes.ant,
                    Alpha = rnd.Next(200, 500) / 100.0,
                    Beta = rnd.Next(500, 1000) / 100.0,
                    AntType = AntTypes.Greedy,
                    AntNumber = ++count
                };
                var socialAnt = new Ant
                {
                    AgentType = Agent.AgentTypes.ant,
                    Alpha = rnd.Next(500, 1000) / 100.0,
                    Beta = rnd.Next(200, 500) / 100.0,
                    AntType = AntTypes.Euristic,
                    AntNumber = ++count
                };
                basicAnt.GetAntResult(MinCasesPerRule, basicTerms, data, attributes, results, currentRules, EuristicFunctionType, PheromonesUpdateMethod, RulesPruningStatus, ref currentNumberForConvergence, ref currentAnt);
                greedyAnt.GetAntResult(MinCasesPerRule, greedyTerms, data, attributes, results, currentRules, EuristicFunctionType, PheromonesUpdateMethod, RulesPruningStatus, ref currentNumberForConvergence, ref currentAnt);
                socialAnt.GetAntResult(MinCasesPerRule, socialTerms, data, attributes, results, currentRules, EuristicFunctionType, PheromonesUpdateMethod, RulesPruningStatus, ref currentNumberForConvergence, ref currentAnt);
                agentsPopulation.Add(basicAnt);
                agentsPopulation.Add(greedyAnt);
                agentsPopulation.Add(socialAnt);
            }
            return agentsPopulation;
        }

        public int Test(List<Rule> discoveredRules)                             //move to other class -> should work with List<List<Rule>> from all trees
        {
            //var data = _fullData.Deserialize();
            var data = _testingTable;
            data.Cases = data.Cases.OrderBy(item => item.Number).ToList();
            var realResults = new List<string>();
            foreach (var item in data.Cases)
            {
                realResults.Add(item.Result);
                item.Result = "";
            }
            //foreach (var discoveredRule in discoveredRules)
            //{
            //    discoveredRule.GetCoveredCases(data);
            //    foreach (var discoveredRuleCoveredCase in discoveredRule.CoveredCases)
            //    {
            //        data.Cases.Find(item => item.Number == discoveredRuleCoveredCase.Number).Result =
            //            discoveredRule.Result;
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
                var groups = items.GroupBy(s => s.Result).ToList();
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
