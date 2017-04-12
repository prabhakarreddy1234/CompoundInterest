using System;
using System.Collections.Generic;
using System.Linq;
using CompoundInterest.Entities;
using CompoundInterest.Interfaces;

namespace CompoundInterest
{
    public class Bootstrapper : IBootstrapper
    {
        private readonly ICsvReader _csvReader;

        public Bootstrapper(ICsvReader csvReader)
        {
            _csvReader = csvReader;
        }

        public Output CalculateInterest(string[] args)
        {
            if (args == null || args.Length != 2 || !args[0].EndsWith("csv") || Convert.ToInt32(args[1]) <= 0)
                throw new Exception("Invalid or missing input parameters.");

            var principle = Convert.ToInt32(args[1]);

            if (principle < 1000 || principle > 15000 || principle % 100 != 0)
                throw new Exception("Loan amount should be between 1000 and 15000 with increments of 100.");

            var lenders = _csvReader.Read();

            if (lenders == null || lenders.Count == 0)
                throw new Exception("Input file is empty");

            var totalAmtAvailable = lenders.Sum(x => x.Balance);
            if (totalAmtAvailable < principle)
                throw new Exception("We cannot lend you the amount you requested at the moment. Please try later.");

            var orderedLenders = lenders.OrderBy(s => s.InterestRate);

            var calculatedInterestRate = CalculatedInterestRate(orderedLenders, principle);

            var repayAmount = CompoundInterest(principle, calculatedInterestRate, 36, 3);

            return new Output {CalculatedInterest = calculatedInterestRate, RepayAmount = repayAmount};
        }


        private static double CalculatedInterestRate(IEnumerable<Amount> groupedData, int principle)
        {
            var sum = 0;
            var calculatedInterestRate = 0.0;
            var counter = 0;

            foreach (var val in groupedData)
            {
                sum += val.Balance;
                calculatedInterestRate += val.InterestRate;
                counter++;
                if (principle <= sum)
                {
                    calculatedInterestRate = calculatedInterestRate / counter;
                    break;
                }
            }
            return Math.Round(calculatedInterestRate * 100, 1);
        }

        /// <summary>
        ///     CompoundInterest.
        /// </summary>
        private static double CompoundInterest(double principal,
            double interestRate,
            int timesPerYear,
            double years)
        {
            // (1 + r/n)
            var body = 1 + interestRate / timesPerYear / 100;

            // nt
            var exponent = timesPerYear * years;

            // P(1 + r/n)^nt
            return principal * Math.Pow(body, exponent);
        }
    }
}