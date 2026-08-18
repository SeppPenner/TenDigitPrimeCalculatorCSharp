// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataProvider.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to provide the test data used in the tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TenDigitPrimeCalculatorCSharp.Tests;

/// <summary>
/// A class to provide the test data used in the tests.
/// </summary>
public static class TestDataProvider
{
    /// <summary>
    /// The solution to the Google billboard problem, the same value the readme documents as the result.
    /// </summary>
    public const string ExpectedTenDigitPrime = "7427466391";

    /// <summary>
    /// The zero based index of <see cref="ExpectedTenDigitPrime"/> inside <see cref="EulerDigitsUpToTheSolution"/>.
    /// </summary>
    public const int ExpectedStartIndex = 98;

    /// <summary>
    /// The digits of the Euler number behind the decimal point, up to and including the solution. They are written
    /// down here on purpose instead of being read from the calculator, so that the tests check the digits the
    /// calculator uses against an independent copy.
    /// </summary>
    public const string EulerDigitsUpToTheSolution =
            "718281828459045235360287471352662497757247093699959574966967627724076630353547594571382178525166427427466391";
}
