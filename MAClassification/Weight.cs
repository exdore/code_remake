﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAClassification
{
    class Weight
    {
        public int AttributeIndex { get; set; }
        public int AttributeValue { get; set; }
        public bool IsChosen { get; set; }
        public double WeightValue { get; set; }
        public double EuristicFunctionValue { get; set; }
        public double Probability { get; set; }
    }
}
