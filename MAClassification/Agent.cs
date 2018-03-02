using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MAClassification.Models;

namespace MAClassification
{ 
    public enum AgentTypes { Ant }
    public enum EuristicTypes { Entropy, Density }
    public enum PheromonesTypes { Normalization, Evaporation }
    public enum DivideTypes { CrossValidation, ByClass }
    [Serializable]
    [XmlInclude(typeof(Ant))]
    [XmlInclude(typeof(Rule))]
    public abstract class Agent
    {
        public abstract Rule Rule { get; set; }

        public AgentTypes AgentType { get; set; }

        public abstract void BuildRule(RuleData data, EuristicTypes euristicType, PheromonesTypes pheromonesType);

        public abstract void GetAntResult(RuleData ruleData, List<Rule> currentRules, CalculationOptions options, ref int currentNumberForConvergence, ref int currentAnt);
    }
}
