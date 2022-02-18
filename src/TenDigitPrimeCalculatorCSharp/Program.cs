// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The main program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TenDigitPrimeCalculatorCSharp;

/// <summary>
/// The main program.
/// </summary>
public static class Program
{
    /// <summary>
    /// The main method.
    /// </summary>
    public static void Main()
    {
        var tenDigitPrimeCalculator = new TenDigitPrimeCalculator();
        tenDigitPrimeCalculator.CalculatePrimes();
        Console.WriteLine("Please press any key to terminate");
        Console.ReadKey();
    }
}
