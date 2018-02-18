using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Terms Deserialize(TermTypes TermType)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Terms));
            var streamReader = File.Exists(TermType + @"Terms.xml")
                ? new StreamReader(TermType + @"Terms.xml")
                : new StreamReader(@"BasicTerms.xml");
            Terms currentTerms = (Terms)xmlSerializer.Deserialize(streamReader);
            streamReader.Close();
            return currentTerms;
        }
    }
}
