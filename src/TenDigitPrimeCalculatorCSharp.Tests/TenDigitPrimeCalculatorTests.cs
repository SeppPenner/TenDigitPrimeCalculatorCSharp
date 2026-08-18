// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TenDigitPrimeCalculatorTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="TenDigitPrimeCalculator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TenDigitPrimeCalculatorCSharp.Tests;

/// <summary>
/// A class to test the <see cref="TenDigitPrimeCalculator"/> class.
/// </summary>
[TestClass]
public class TenDigitPrimeCalculatorTests
{
    /// <summary>
    /// The calculator under test.
    /// </summary>
    private readonly ITenDigitPrimeCalculator calculator = new TenDigitPrimeCalculator();

    /// <summary>
    /// Checks whether the calculation returns the value the readme documents as the solution to the Google
    /// billboard problem.
    /// </summary>
    [TestMethod]
    public void CalculatePrimesReturnsTheGoogleBillboardPrime()
    {
        var tenDigitPrime = this.calculator.CalculatePrimes();

        Assert.AreEqual(TestDataProvider.ExpectedTenDigitPrime, tenDigitPrime);
    }

    /// <summary>
    /// Checks whether the returned value consists of exactly ten digits. The task asks for a ten digit prime, and a
    /// window of the digits starting with a zero would silently be a nine digit number.
    /// </summary>
    [TestMethod]
    public void CalculatePrimesReturnsATenDigitNumber()
    {
        var tenDigitPrime = this.calculator.CalculatePrimes();

        Assert.AreEqual(10, tenDigitPrime.Length);
        Assert.IsTrue(tenDigitPrime.All(char.IsAsciiDigit), "The result holds characters that are no digits.");
        Assert.AreNotEqual('0', tenDigitPrime[0], "The result starts with a zero, so it is no ten digit number.");
    }

    /// <summary>
    /// Checks whether the returned value really is a prime number, using a primality check that is written
    /// independently of the one inside the calculator.
    /// </summary>
    [TestMethod]
    public void CalculatePrimesReturnsAPrimeNumber()
    {
        var tenDigitPrime = this.calculator.CalculatePrimes();

        Assert.IsTrue(IsPrimeReference(long.Parse(tenDigitPrime)), "The result is no prime number.");
    }

    /// <summary>
    /// Checks whether the returned value is the first ten digit prime of the digits of the Euler number. Every
    /// earlier window of ten digits has to be composite, otherwise the calculator skipped a solution. The digits
    /// come from <see cref="TestDataProvider.EulerDigitsUpToTheSolution"/>, so this also checks that the calculator
    /// works on the digits of the Euler number and not on some other sequence.
    /// </summary>
    [TestMethod]
    public void CalculatePrimesReturnsTheFirstTenDigitPrimeOfTheEulerDigits()
    {
        var digits = TestDataProvider.EulerDigitsUpToTheSolution;

        for (var start = 0; start < TestDataProvider.ExpectedStartIndex; start++)
        {
            var window = long.Parse(digits.Substring(start, 10));
            Assert.IsFalse(IsPrimeReference(window), $"The window at index {start} is prime and was skipped.");
        }

        var tenDigitPrime = this.calculator.CalculatePrimes();

        Assert.AreEqual(digits.Substring(TestDataProvider.ExpectedStartIndex, 10), tenDigitPrime);
    }

    /// <summary>
    /// Checks whether two calls return the same value. The calculator keeps no state between calls, and a caller
    /// may use one instance more than once.
    /// </summary>
    [TestMethod]
    public void CalculatePrimesReturnsTheSameResultOnEveryCall()
    {
        var firstResult = this.calculator.CalculatePrimes();
        var secondResult = this.calculator.CalculatePrimes();

        Assert.AreEqual(firstResult, secondResult);
    }

    /// <summary>
    /// Checks whether a fresh instance returns the same value, so that the result does not depend on the instance
    /// the demo program happens to create.
    /// </summary>
    [TestMethod]
    public void CalculatePrimesReturnsTheSameResultForEveryInstance()
    {
        var tenDigitPrime = new TenDigitPrimeCalculator().CalculatePrimes();

        Assert.AreEqual(this.calculator.CalculatePrimes(), tenDigitPrime);
    }

    /// <summary>
    /// Checks whether the console output names the found prime and the index it was found at. The output is the only
    /// thing a user of the demo program sees.
    /// </summary>
    [TestMethod]
    public void CalculatePrimesWritesTheResultToTheConsole()
    {
        var originalOutput = Console.Out;
        using var writer = new StringWriter();

        try
        {
            Console.SetOut(writer);
            this.calculator.CalculatePrimes();
        }
        finally
        {
            Console.SetOut(originalOutput);
        }

        var output = writer.ToString();

        StringAssert.Contains(output, $"'{TestDataProvider.ExpectedTenDigitPrime}' is a prime number");
        StringAssert.Contains(output, $"Start is: '{TestDataProvider.ExpectedStartIndex}'");
    }

    /// <summary>
    /// Checks whether the test data itself matches the Euler number, which is 2.718281828459045235... The other
    /// tests compare the calculator against these digits, so a typo in them has to fail here and not somewhere
    /// else.
    /// </summary>
    [TestMethod]
    public void TheTestDigitsAreTheDigitsOfTheEulerNumber()
    {
        StringAssert.StartsWith(TestDataProvider.EulerDigitsUpToTheSolution, "718281828459045235");
        Assert.AreEqual(TestDataProvider.ExpectedStartIndex + 10, TestDataProvider.EulerDigitsUpToTheSolution.Length);
        Assert.IsTrue(TestDataProvider.EulerDigitsUpToTheSolution.All(char.IsAsciiDigit));
    }

    /// <summary>
    /// Checks whether the value is a prime number or not. This is a second implementation, written differently from
    /// the one in the calculator on purpose, so that the tests do not repeat the mistakes of the code they check. It
    /// skips the multiples of two and three by stepping in steps of six.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns><c>true</c> if the value is prime, <c>false</c> else.</returns>
    private static bool IsPrimeReference(long value)
    {
        if (value < 2)
        {
            return false;
        }

        if (value % 2 == 0)
        {
            return value == 2;
        }

        if (value % 3 == 0)
        {
            return value == 3;
        }

        var divisor = 5L;

        while (divisor * divisor <= value)
        {
            if (value % divisor == 0 || value % (divisor + 2) == 0)
            {
                return false;
            }

            divisor += 6;
        }

        return true;
    }
}
