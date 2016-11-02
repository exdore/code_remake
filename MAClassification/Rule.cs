using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAClassification
{
    struct Rule
    {
        public class Condition
        {
            public int Index { get; set; }
            public int Value { get; set; }
        }

        public List<Condition> Conditions;

        public int Result { get; set; }

        public double Quality { get; set; }

        public Rule(List<Condition> data) : this()
        {
            Conditions = data;
            Result = 0;
            Quality = 0;
        }
    }
}
