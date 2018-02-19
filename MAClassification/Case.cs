using System;
using System.Collections.Generic;

namespace MAClassification
{
    [Serializable]
    public class Case
    {
        public int Number { get; set; }
        public List<string> AttributesValuesList { get; set; }
        public string Class { get; set; }

        private string ValuesToString()
        {
            var temp = "";
            foreach (var item in AttributesValuesList)
                temp += item + ", ";
            return temp;
        }

        public override string ToString()
        {
            return Number + " " + ValuesToString() + Class;
        }
    }
}
