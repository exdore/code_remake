using System;
using System.Collections.Generic;
using System.Linq;

namespace MAClassification
{
    public class Divider
    {
        int Step { get; set; }

        public List<Table> Divide(int n, Table data)
        {
            var size = data.GetCasesCount();
            var tables = new List<Table>();
            var count = n * 5;
            if (n != 1)
            {
                data.Cases = data.Cases.OrderBy(a => Guid.NewGuid()).ToList();
                while (count > 0)
                {
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

        public List<Table> DivideByClass(Table data)
        {
            var tables = new List<Table>();
            var results = data.Cases.Select(item => item.Result).Distinct();
            foreach (var result in results)
            {
                tables.Add(new Table
                {
                    Cases = data.Cases.Where(item => item.Result == result).ToList(),
                    Header = data.Header
                });
            }
            return tables;
        }
    }
}
