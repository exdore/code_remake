namespace ArffSharp
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class ArffAttribute
    {
        public string Name { get; private set; }
        public Collection<string> NominalValues { get; private set; }
        public Collection<double> RealValues { get; set; }

        public ArffAttribute(string name, IList<string> nominalValues)
        {
            this.Name = name;
            this.NominalValues = new Collection<string>(nominalValues);
            this.RealValues = new Collection<double>();
        }
    }
}
