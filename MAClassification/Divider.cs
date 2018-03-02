using System;
using System.Collections.Generic;
using System.Linq;

namespace MAClassification
{
    public class Divider
    {
        int Step { get; set; }

        public List<Table> MakeTables(DivideTypes divideType, Table data, int n)
        {
            return divideType == DivideTypes.ByClass ? DivideByClass(data) : Divide(n, data);
        }

        private List<Table> Divide(int n, Table data)
        {
            var size = data.GetCasesCount();
            var tables = new List<Table>();
            var count = n * 5;
            if (count % 2 == 0) count++;
            if (n != 1)
            {
                while (count > 0)
                {
                    if (count % n == 0)             //?????
                        data.Cases = data.Cases.OrderBy(a => Guid.NewGuid()).ToList();
                    var k = size / (n - 1);
                    Step = (size - k) / (n - 1);
                    for (int i = 0; i < n; i++)
                    {
                        if (i != n - 1)
                            tables.Add(new Table
                            {
                                Cases = data.Cases.GetRange(i * Step, k),
                                Header = data.Header
                            });
                        else
                            tables.Add(new Table
                            {
                                Cases = data.Cases.GetRange(i * Step, size - i * Step),
                                Header = data.Header
                            });
                    }
                    count -= n;
                }
                return tables;
            }
            tables.Add(data);
            return tables;
        }

        private List<Table> DivideByClass(Table data)
        {
            var tables = new List<Table>();
            var results = data.Cases.Select(item => item.Class).Distinct();
            foreach (var result in results)
            {
                tables.Add(new Table
                {
                    Cases = data.Cases.Where(item => item.Class == result).ToList(),
                    Header = data.Header
                });
            }
            return tables;
        }
    }
}
