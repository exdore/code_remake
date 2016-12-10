using System;
using System.Collections.Generic;
using System.Linq;

namespace MAClassification
{
    public class Rule 
    {
        public List<Condition> ConditionsList;

        public string Result { get; set; }

        public double Quality { get; set; }

        public List<Case> CoveredCases { get; set; }
    }
}
