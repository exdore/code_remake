using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ArffSharp
{
    public class ArffAttribute
    {
        public ArffAttribute(string name, IList<string> nominalValues)
        {
            Name = name;
            NominalValues = new List<string>(nominalValues);
            RealValues = new List<double>();
        }

        public string Name { get; }
        public List<string> NominalValues { get; set; }
        public List<double> RealValues { get; set; }

    }
}