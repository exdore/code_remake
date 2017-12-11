using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MAClassification
{
    [Serializable]
    [XmlInclude(typeof(Ant))]
    [XmlInclude(typeof(Rule))]
    public abstract class Agent
    {
        public abstract Rule Rule { get; set; }

        public enum AgentTypes { ant }

        public AgentTypes AgentType { get; set; }

        protected Agent()
        {
        }

        public abstract void BuildRule(decimal minCasesPerRule, Terms initialTerms, Table data,
            Attributes attributes, List<string> results, string type);

        public abstract void GetAntResult(decimal minCasesPerRule, Terms terms, Table data,
            Attributes attributes, List<string> results, List<Rule> currentRules, string euristicType, string pheromonesType, string pruningType, ref int currentNumberForConvergence, ref int currentAnt);
    }
}
