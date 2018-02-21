using System.Collections.Generic;
using System.Linq;
using MAClassification.Models;

namespace MAClassification
{
    public class Attributes : List<DataAttribute>
    {
        public int GetValuesCount()
        {
            return this.Sum(attribute => attribute.AttributeValues.Count);
        }
    }
}
