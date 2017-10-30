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
    public abstract class Agent
    {
        public abstract Rule Rule { get; set; }
        private enum AgentTypes { ant }

        public abstract void BuildRule(List<Case> currentCases, decimal minCasesPerRule, Terms initialTerms, Table data,
            Attributes attributes, List<string> results, GroupBox groupBox);

    }
}
