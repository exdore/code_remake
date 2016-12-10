using System.Collections.Generic;
using System.Linq;

namespace MAClassification
{
    public class Attributes : List<Attribute>
    {
        public int GetValuesCount()
        {
            return this.Sum(attribute => attribute.AttributeValues.Count);
        }
    }
}
