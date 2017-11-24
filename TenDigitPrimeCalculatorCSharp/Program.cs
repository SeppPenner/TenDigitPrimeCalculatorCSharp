using System;

namespace TenDigitPrimeCalculatorCSharp
{
    internal static class Program
    {
        private static void Main()
        {
            var tenDigitPrimeCalculator = new TenDigitPrimeCalculator();
            tenDigitPrimeCalculator.CalculatePrimes();
            Console.WriteLine("Please press any key to terminate");
            Console.ReadKey();
        }
    }
}