using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAClassification
{
    class Attribute
    {
        public string AttributeName { get; set; }
        public List<string> AttributeValues { get; set; }
        public bool IsUsed { get; set; }
    }
}
