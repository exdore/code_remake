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

        public List<Condition> Conditions;

        public string Result { get; set; }

        public double Quality { get; set; }

        public Rule(List<Condition> data) : this()
        {
            Conditions = data;
            Result = "";
            Quality = 0;
        }
    }
}
