using System.Collections.Generic;

namespace MAClassification
{
    struct Rule
    {
        public class Condition
        {
            public string Attribute { get; set; }
            public string Value { get; set; }
        }

        public List<Condition> ConditionsList;

        public string Result { get; set; }

        public double Quality { get; set; }

        public Rule(List<Condition> data) : this()
        {
            ConditionsList = data;
            Result = "";
            Quality = 0;
        }
    }
}
