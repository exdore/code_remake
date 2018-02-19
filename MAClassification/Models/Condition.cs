using System;

namespace MAClassification
{
    [Serializable]
    public class Condition
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }

        public override string ToString()
        {
            return AttributeName + " = " + AttributeValue;
        }

        public override int GetHashCode()
        {
            return AttributeName.GetHashCode() ^ AttributeValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return GetHashCode() == obj.GetHashCode();
        }
    }
}
