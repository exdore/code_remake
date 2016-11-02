using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAClassification
{
    class Table
    {
        public List<string> Header { get; set; }
        public List<int> AttributesValuesCount { get; set; }
        public List<Case> Cases { get; set; }
        public int CountColumns
        {
            get
            {
                return Cases[0].Attributes.Count - 1;
            }
        }
        public double Probability { get; set; }
        public double Entropy { get; set; }
        public List<double> Gains { get; set; }
        public List<AdditionalCount> AdditionalCounts { get; set; }
        public List<double> CountsByColumns { get; set; }
        public bool[] DisabledColumns { get; set; }
    }
}
