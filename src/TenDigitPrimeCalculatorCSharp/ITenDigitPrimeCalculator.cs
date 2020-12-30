// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITenDigitPrimeCalculator.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   An interface to calculate the Google billboard problem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TenDigitPrimeCalculatorCSharp
{
    /// <summary>
    ///     An interface to calculate the Google billboard problem.
    /// </summary>
    public interface ITenDigitPrimeCalculator
    {
        /// <summary>
        ///     Do the calculation.
        /// </summary>
        /// <returns>The solution to the Google billboard problem.</returns>
        // ReSharper disable once UnusedMemberInSuper.Global
        string CalculatePrimes();
    }
}