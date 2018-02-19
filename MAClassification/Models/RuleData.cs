using System.Collections.Generic;

namespace MAClassification.Models
{
    public class RuleData
    {
        public decimal MinCasesPerRule { get; set; }
        public Terms Terms { get; set; }
        public Table Table { get; set; }
        public Attributes Attributes { get; set; }
        public List<string> Classes { get; set; }
    }
}
