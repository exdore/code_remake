using System;

namespace MAClassification
{
    [Serializable]
    public class Condition
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}
