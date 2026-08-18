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
        var tenDigitPrime = tenDigitPrimeCalculator.CalculatePrimes();
        Console.WriteLine(string.IsNullOrEmpty(tenDigitPrime)
            ? "No ten digit prime was found in the digits of the Euler number"
            : "The solution to the Google billboard problem is: '" + tenDigitPrime + "'");
        // Waiting for a key only works on a real console. Without this check the program crashes at the very end
        // when its input is redirected, for example when it is started from a script.
        if (Console.IsInputRedirected)
        {
            return;
        }

        Console.WriteLine("Please press any key to terminate");
        Console.ReadKey();
    }
}
