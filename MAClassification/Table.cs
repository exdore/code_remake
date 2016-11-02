using System.Collections.Generic;

namespace MAClassification
{
    class Table
    {
        public List<string> Header { get; set; }
        public List<Case> Cases { get; set; }
        public int CountColumns
        {
            get
            {
                return Cases[0].Attributes.Count - 1;
            }
        }
        public List<double> Gains { get; set; }
        public List<AdditionalCount> AdditionalCounts { get; set; }
        public List<double> CountsByColumns { get; set; }
    }
}
