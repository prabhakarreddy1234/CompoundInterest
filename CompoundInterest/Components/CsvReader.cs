using System;
using System.Collections.Generic;
using System.Linq;
using CompoundInterest.Interfaces;

namespace CompoundInterest.Components
{
    public class CsvReader : FileReader, ICsvReader
    {
        public CsvReader(string fileName) : base(fileName)
        {
        }

        public IList<Amount> Read()
        {
            var list = new List<Amount>();
            foreach (var file in ReadFile().Skip(1))
            {
                var lender = file.Split(',');
                var amt = new Amount
                {
                    Name = lender[0],
                    InterestRate = Convert.ToDouble(lender[1]),
                    Balance = Convert.ToInt32(lender[2])
                };
                list.Add(amt);
            }
            return list;
        }
    }
}