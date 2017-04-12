using System;
using System.Collections.Generic;
using System.IO;
using CompoundInterest.Interfaces;
using Moq;
using NUnit.Framework;

namespace CompoundInterest.Tests
{
    [TestFixture]
    public class BootstrapperTests
    {
        private Mock<ICsvReader> _csvReaderMock;

        private IBootstrapper _bootstrapper;
        private const string TestInputFile = "market.csv";

        [SetUp]
        public void SetUp()
        {
            //create infrastructure for test run
            _csvReaderMock = new Mock<ICsvReader>();
            _bootstrapper = new Bootstrapper(_csvReaderMock.Object);
        }


        [TestCase(1000, 7.6, 1255.78)]
        [TestCase(1500, 7.6, 1883.68)]
        [TestCase(2200, 8.1, 2804.39)]
        public void Should_return_data_returned_from_file(int principle, double interestRate, double totalRepayment)
        {
            //Arrange
            var fakeData = new List<Amount>
            {
                new Amount {Balance = 400, InterestRate = 0.071},
                new Amount {Balance = 500, InterestRate = 0.075},
                new Amount {Balance = 600, InterestRate = 0.083},
                new Amount {Balance = 700, InterestRate = 0.094}
            };
            _csvReaderMock.Setup(c => c.Read()).Returns(fakeData);

            //Act
            var result = _bootstrapper.CalculateInterest(new[] {TestInputFile, principle.ToString()});


            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(interestRate, result.CalculatedInterest);
            Assert.AreEqual(totalRepayment, Math.Round(result.RepayAmount, 2));
            Assert.AreEqual(Math.Round(totalRepayment / 12, 2), Math.Round(result.RepayAmount / 12, 2));
        }

        [Test]
        public void Should_throw_error_when_borrowedamount_is_more_than_available()
        {
            var fakeData = new List<Amount>
            {
                new Amount {Balance = 400, InterestRate = 0.71},
                new Amount {Balance = 500, InterestRate = 0.75},
                new Amount {Balance = 600, InterestRate = 0.83},
                new Amount {Balance = 700, InterestRate = 0.94}
            };
            _csvReaderMock.Setup(c => c.Read()).Returns(fakeData);
            Assert.Throws<Exception>(() => _bootstrapper.CalculateInterest(new[] {TestInputFile, "3000"}));
        }

        [Test]
        public void Should_throw_error_when_file_is_empty()
        {
            _csvReaderMock.Setup(c => c.Read()).Returns(new List<Amount>());
            Assert.Throws<Exception>(() => _bootstrapper.CalculateInterest(new[] {TestInputFile, "1000"}));
        }

        [Test]
        public void Should_throw_exception_when_Invalid_Filename_passed()
        {
            Assert.Throws<Exception>(() => _bootstrapper.CalculateInterest(new[] {"market.txt", "1000"}));
        }

        [Test]
        public void Should_throw_exception_when_Invalid_Principle_passed()
        {
            Assert.Throws<Exception>(() => _bootstrapper.CalculateInterest(new[] {TestInputFile, "100"}));
        }

        [Test]
        public void Should_throw_exception_when_No_Args_Passed()
        {
            Assert.Throws<Exception>(() => _bootstrapper.CalculateInterest(null));
        }

        [Test]
        public void Should_throw_filenotfound_exception_using_CSVReader()
        {
            Assert.Throws<FileNotFoundException>(() => _bootstrapper.CalculateInterest(new[] {TestInputFile, "1000"}));
        }
    }
}