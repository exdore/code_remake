namespace ArffSharp
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class ArffAttribute
    {
        public string Name { get; private set; }
        public List<string> NominalValues { get; set; }
        public List<double> RealValues { get; set; }
        public bool WasRecalculated { get; set; }

        public ArffAttribute(string name, IList<string> nominalValues)
        {
            this.Name = name;
            this.NominalValues = new List<string>(nominalValues);
            this.RealValues = new List<double>();
            WasRecalculated = false;
        }
    }
}
