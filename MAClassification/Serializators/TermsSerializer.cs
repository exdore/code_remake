using System.IO;
using System.Xml.Serialization;

namespace MAClassification.Serializators
{

    class TermsSerializer
    {

        public void Serialize(Terms terms)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Terms));
            StreamWriter streamWriter = new StreamWriter(terms.TermType + @"Terms.xml");
            xmlSerializer.Serialize(streamWriter, terms);
            streamWriter.Close();
        }

        public Terms Deserialize(TermTypes termType)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Terms));
            var streamReader = File.Exists(termType + @"Terms.xml")
                ? new StreamReader(termType + @"Terms.xml")
                : new StreamReader(@"BasicTerms.xml");
            Terms currentTerms = (Terms)xmlSerializer.Deserialize(streamReader);
            streamReader.Close();
            return currentTerms;
        }
    }
}
