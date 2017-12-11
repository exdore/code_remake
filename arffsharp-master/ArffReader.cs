namespace ArffSharp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using ArffSharp.Extensions;
    using LumenWorks.Framework.IO.Csv;

    public class ArffReader : IDisposable
    {
        public const string AttributeDeclaration = "@attribute";
        public const string DataDeclaration = "@data";

        private Stream stream;
        private StreamReader reader;
        private CsvReader csvReader;
        private int attributeCount;

        public ArffReader(string fileName)
            : this(File.OpenRead(fileName))
        {
        }

        public ArffReader(Stream stream)
        {
            this.stream = stream;
            this.reader = new StreamReader(stream);
            this.ReadAttributes();
            this.csvReader = new CsvReader(reader, false, trimmingOptions: ValueTrimmingOptions.All);
        }

        public ReadOnlyCollection<ArffAttribute> Attributes { get; private set; }

        public ArffRecord ReadNextRecord()
        {
            if (!csvReader.ReadNextRecord()) return null;

            ArffRecord record = new ArffRecord();
            record.Values = new ArffValue[this.attributeCount];

            for (int i = 0; i < this.attributeCount; i++)
            {
                if (Attributes[i].NominalValues.Count != 1)
                {
                    var arffVal = record.Values[i] = new ArffValue();
                    var val = csvReader[i].Unescape();
                    if (val == "?")
                    {
                        arffVal.NominalValueIndex = -1;
                    }
                    else
                    {
                        arffVal.NominalValueIndex = this.Attributes[i].NominalValues.IndexOf(val);

                        if (arffVal.NominalValueIndex == -1)
                        {
                            throw new ArffReaderException("Unknown nominal value \"" + val + "\" for attribute \"" +
                                                          this.Attributes[i].Name + "\".");
                        }
                    }
                }
                else
                {
                    var arffVal = record.Values[i] = new ArffValue();
                    var val = double.Parse(csvReader[i].Unescape());
                    var index = this.Attributes[i].RealValues.IndexOf(val);
                    if (index == -1)
                    {
                        Attributes[i].RealValues.Add(val);
                        arffVal.RealValueIndex = Attributes[i].RealValues.Count - 1;
                    }
                }
            }

            return record;
        }

        private void ReadAttributes()
        {
            var attributes = new List<ArffAttribute>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWithI(DataDeclaration))
                {
                    break;
                }

                if (!line.StartsWithI(AttributeDeclaration))
                {
                    continue;
                }

                line = line.Substring(AttributeDeclaration.Length + 1);
                string[] split;
                string name;
                string[] values;
                if (!line.Contains("{"))
                {
                    split = line.Split(' ');
                    name = split[0].Trim().Unescape();
                    values = new string[1];
                    values[0] = split[1];
                }
                else
                {
                    split = line.Split('{', '}');
                    name = split[0].Trim().Unescape();
                    var valueList = split[1];
                    var csv = new CsvReader(new StringReader(valueList), false,
                        trimmingOptions: ValueTrimmingOptions.All);
                    int fieldCount = csv.FieldCount;
                    values = new string[fieldCount];
                    csv.ReadNextRecord();

                    for (int i = 0; i < fieldCount; i++)
                    {
                        values[i] = csv[i].Unescape();
                    }
                }
                attributes.Add(new ArffAttribute(name, values));
            }

            this.Attributes = new ReadOnlyCollection<ArffAttribute>(attributes);
            this.attributeCount = attributes.Count;
        }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Dispose();
            }
        }
    }
}
