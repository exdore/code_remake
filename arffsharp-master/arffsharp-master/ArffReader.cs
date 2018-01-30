using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ArffSharp.Extensions;
using LumenWorks.Framework.IO.Csv;

namespace ArffSharp
{
    public class ArffReader : IDisposable
    {
        public const string AttributeDeclaration = "@attribute";
        public const string DataDeclaration = "@data";
        private int attributeCount;
        private readonly CsvReader csvReader;
        private readonly StreamReader reader;

        private readonly Stream stream;

        public ArffReader(string fileName)
            : this(File.OpenRead(fileName))
        {
        }

        public ArffReader(Stream stream)
        {
            this.stream = stream;
            reader = new StreamReader(stream);
            ReadAttributes();
            csvReader = new CsvReader(reader, false, trimmingOptions: ValueTrimmingOptions.All);
        }

        public List<ArffAttribute> Attributes { get; set; }

        public void Dispose()
        {
            stream?.Dispose();
        }

        public void BuildIntervals()
        {
            foreach (var attribute in Attributes)
            {
                if (attribute.NominalValues.Count == 1)
                {
                    var min = attribute.RealValues.Min();
                    var max = attribute.RealValues.Max();
                    int intervals = 5;                           //magic const
                    double lenght = (max - min) / intervals;
                    attribute.NominalValues.RemoveAt(0);
                    var right = min + lenght;
                    for (int i = 0; i < intervals; i++)
                    {
                        attribute.NominalValues.Add(i != intervals - 1
                            ? $"X < {right}"
                            : $"X \u2264 {right}");
                        right += lenght;
                    }
                }
            }
        }

        public List<double> GetBorders(int i)
        {
            List<double> borders = new List<double>();
            foreach (var nominalValue in Attributes[i].NominalValues)
            {
                var res = new Regex("\\d+\\D?\\d*").Match(nominalValue).ToString();
                borders.Add(Convert.ToDouble(res, NumberFormatInfo.CurrentInfo));
            }
            return borders;
        }

        public List<ArffRecord> Discretize(List<ArffRecord> records)
        {
            for (var i = 0; i < Attributes.Count; i++)
            {
                var values = Attributes[i].RealValues;
                if (values.Count > 0)
                {
                    var borders = GetBorders(i);
                    foreach (var item in records)
                    {
                        var value = values[item.Values[i].RealValueIndex];
                        var index = borders.FindIndex(x => x >= value);
                        item.Values[i].NominalValueIndex = index;
                    }
                }
            }
            return records;
        }

        public ArffRecord ReadNextRecord()
        {
            if (!csvReader.ReadNextRecord()) return null;

            var record = new ArffRecord
            {
                Values = new ArffValue[attributeCount]
            };

            for (var i = 0; i < attributeCount; i++)
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
                        arffVal.NominalValueIndex = Attributes[i].NominalValues.IndexOf(val);

                        if (arffVal.NominalValueIndex == -1)
                            throw new ArffReaderException("Unknown nominal value \"" + val + "\" for attribute \"" +
                                                          Attributes[i].Name + "\".");
                    }
                }
                else
                {
                    var arffVal = record.Values[i] = new ArffValue();
                    var val = 0.0;
                    if (csvReader[i].Unescape() != "?")
                    {
                        val = double.Parse(csvReader[i].Unescape(), NumberStyles.Float, CultureInfo.InvariantCulture);
                        var index = Attributes[i].RealValues.IndexOf(val);
                        if (index == -1)
                        {
                            Attributes[i].RealValues.Add(val);
                            arffVal.RealValueIndex = Attributes[i].RealValues.Count - 1;
                        }
                    }
                    else
                    {
                        val = Double.NaN;
                        var index = Attributes[i].RealValues.IndexOf(val);
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
                    break;

                if (!line.StartsWithI(AttributeDeclaration))
                    continue;

                line = line.Substring(AttributeDeclaration.Length + 1);
                string[] split;
                string name;
                string[] values;
                if (!line.Contains("{"))
                {
                    split = line.Split(new char[] {' ', '\t'});
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
                    var fieldCount = csv.FieldCount;
                    values = new string[fieldCount];
                    csv.ReadNextRecord();

                    for (var i = 0; i < fieldCount; i++)
                        values[i] = csv[i].Unescape();
                }
                attributes.Add(new ArffAttribute(name, values));
            }

            Attributes = new List<ArffAttribute>(attributes);
            attributeCount = attributes.Count;
        }
    }
}