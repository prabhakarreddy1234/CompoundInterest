using System;
using CompoundInterest.Components;
using CompoundInterest.Interfaces;
using Microsoft.Practices.Unity;

namespace CompoundInterest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var unity = new UnityContainer();
            unity.RegisterType<IBootstrapper, Bootstrapper>();
            unity.RegisterType<ICsvReader, CsvReader>(new InjectionConstructor(args[0]));

            var bootstrapper = unity.Resolve<IBootstrapper>();
            var output = bootstrapper.CalculateInterest(args);

            Console.WriteLine($"Requested amount: {args[1]}");
            Console.WriteLine($"Rate: {output.CalculatedInterest}");
            Console.WriteLine($"Monthly Repayment: {Math.Round(output.RepayAmount / 36, 2)}");
            Console.WriteLine($"Total Repayment: {Math.Round(output.RepayAmount, 2)}");
            Console.Read();
        }
    }
}