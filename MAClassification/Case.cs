using System;
using System.Collections.Generic;

namespace MAClassification
{
    [Serializable]
    public class Case
    {
        public int Number { get; set; }
        public List<string> AttributesValuesList { get; set; }
        public string Result { get; set; }
    }
}
