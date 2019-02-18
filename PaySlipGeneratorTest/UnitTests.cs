﻿using PaySlipGenerator;
using PaySlipGenerator.Tax;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace PaySlipGeneratorTest
{
    public class UnitTests
    {
        private readonly ITestOutputHelper output;
        
        public UnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 18200)]
        [InlineData(0.19, 18201)]
        [InlineData(0.19, 37000)]
        [InlineData(0.325, 37001)]
        [InlineData(0.325, 87000)]
        [InlineData(0.37, 87001)]
        [InlineData(0.37, 180000)]
        [InlineData(0.45, 180001)]
        [InlineData(0.45, 99999999)]
        public void GetTaxBand_ValidTaxBand_ReturnsExpectedVariableTax(double expectedVariableTax, int annualIncome)
        {
            Assert.Equal(expectedVariableTax, TaxCalculator.GetTaxBand(annualIncome).VariableTax);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 18200)]
        [InlineData(0, 18201)]
        [InlineData(0, 37000)]
        [InlineData(3572, 37001)]
        [InlineData(3572, 87000)]
        [InlineData(19822, 87001)]
        [InlineData(19822, 180000)]
        [InlineData(54232, 180001)]
        [InlineData(54232, 99999999)]
        public void GetTaxBand_ValidTaxBand_ReturnsExpectedFlatTax(double? expectedFlatTax, int annualIncome)
        {
            Assert.Equal(expectedFlatTax, TaxCalculator.GetTaxBand(annualIncome).FlatTax);
        }

        [Fact]
        public void GetTaxBand_InputIsNegative_FailsWithNegativeNumberException()
        {
            Assert.Throws<NegativeNumberException>(() => TaxCalculator.GetTaxBand(-60050));
        }

        [Fact]
        public void ParseEmployeeLine_IncorrectlyFormattedPeriod_FailsWithFormatException()
        {
            Employee expected = new Employee("David", "Rudd", 60050, 9, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) });
            Assert.Throws<FormatException>(() => IO.ParseEmployeeLine("David,Rudd,60050,9%,01 March . 31 March"));
        }

        [Fact]
        public void ParseEmployeeLine_CorrectlyFormattedLine_ReturnsExpectedEmployee()
        {
            Employee expected = new Employee("David", "Rudd", 60050, 9, new List<PaySlip>() { new PaySlip(new DateTime(2019, 3, 1), new DateTime(2019, 3, 31)) });
            Assert.True(expected.Equals(IO.ParseEmployeeLine("David,Rudd,60050,9%,01 March - 31 March")));
        }

        [Theory]
        [InlineData(5004, 60050)]
        [InlineData(5004, 60045)]
        [InlineData(5003, 60030)]
        [InlineData(5002, 60029)]
        public void GrossIncome_ValidInput_ReturnsExpectedGrossIncome(int expected, int annualIncome)
        {
            Assert.Equal(expected, TaxCalculator.GrossIncome(annualIncome));
        }

        [Fact]
        public void GrossIncome_InputIsNegative_FailsWithNegativeNumberException()
        {
            Assert.Throws<NegativeNumberException>(() => TaxCalculator.GrossIncome(-60050));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 18200)]
        [InlineData(0.19, 18201)]
        [InlineData(298, 37000)]
        [InlineData(298, 37001)]
        [InlineData(922, 60050)]
        [InlineData(1652, 87000)]
        [InlineData(1652, 87001)]
        [InlineData(2669, 120000)]
        [InlineData(4519, 180000)]
        [InlineData(4519, 180001)]
        [InlineData(372769, 9999999)]
        public void IncomeTax_ValidInput_ReturnsExpectedGrossIncome(int expected, int annualIncome)
        {
            Assert.Equal(expected, TaxCalculator.IncomeTax(annualIncome));
        }

        [Fact]
        public void IncomeTax_InputIsNegative_FailsWithNegativeNumberException()
        {
            Assert.Throws<NegativeNumberException>(() => TaxCalculator.IncomeTax(-60050));
        }

        [Theory]
        [InlineData(4082, 60050)]
        [InlineData(7331, 120000)]
        public void NetIncome_ValidInput_ReturnsExpectedNetIncome(int expected, int annualIncome)
        {
            Assert.Equal(expected, TaxCalculator.NetIncome(annualIncome));
        }

        [Fact]
        public void NetIncome_InputIsNegative_FailsWithNegativeNumberException()
        {
            Assert.Throws<NegativeNumberException>(() => TaxCalculator.NetIncome(-60050));
        }

        [Theory]
        [InlineData(450, 60050, 0.09)]
        [InlineData(1000, 120000, 0.1)]
        [InlineData(401, 60084, 0.08)]
        public void Super_ValidInput_ReturnsExpectedSuper(int expected, int annualIncome, double super)
        {
            Assert.Equal(expected, TaxCalculator.Super(annualIncome, super));
        }

        [Fact]
        public void Super_InputIsNegative_FailsWithNegativeNumberException()
        {
            Assert.Throws<NegativeNumberException>(() => TaxCalculator.Super(-60050, 0.09));
        }
    }
}
