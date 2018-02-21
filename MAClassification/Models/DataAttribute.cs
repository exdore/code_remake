using System.Collections.Generic;

namespace MAClassification.Models
{
    public class DataAttribute
    {
        public string AttributeName { get; set; }
        public List<string> AttributeValues { get; set; }
        public bool IsUsed { get; set; }
    }
}
