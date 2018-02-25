using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using LoanCalculator.Core.Models;
using LoanCalculator.Core.Services;
using NUnit.Framework;

namespace LoanCalculator.Core.UnitTests.Services
{
    public class QuoteCalculatorServiceTestFixture
    {
        private IFixture fixture;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
        }


        [Test]
        public void CalculateQuote_WhenAmountNotAvailable_ReturnErrorResponse()
        {
            // Arrange
            var amount = 1000;
            var lenders = CreateLenders();

            var subject = fixture.Create<QuoteCalculatorService>();

            // Act
            var result = subject.CalculateQuote(amount, fixture.Create<int>(), lenders);

            // Assert
            Assert.IsFalse(result.LenderAvailable);
        }


        [Test]
        public void CalculateQuote_WhenLendersAreAvailable_SelectsTheLenderWithTheLowestRate()
        {
            // Arrange
            var amount = 10;
            var lenders = CreateLenders();
            var bestValLender = new Lender
            {
                Rate = 0.0000001,
                Available = 100000,
                Name = fixture.Create<string>()
            };
            lenders.Add(bestValLender);

            var subject = fixture.Create<QuoteCalculatorService>();

            // Act
            var result = subject.CalculateQuote(amount, fixture.Create<int>(), lenders);

            // Assert
            Assert.IsTrue(result.LenderAvailable);
            Assert.AreEqual(bestValLender.Rate, result.Rate);
        }

        [Test]
        public void CalculateMonthlyRepayment_WhenInvoked_CorrectlyCalculatesRepaymentAmount()
        {
            // Arrange
            double rate = 0.07;
            double principal = 1000;
            int months = 36;
            var expected = 30.78;

            var subject = fixture.Create<QuoteCalculatorService>();

            // Act
            var result = subject.CalculateMonthlyrepayment(principal, months, rate);

            // Assert
            Assert.AreEqual(expected, Math.Round(result, 2));
        }

        private IList<Lender> CreateLenders()
        {
            return new List<Lender>
            {
                new Lender
                {
                    Name = fixture.Create<string>(),
                    Rate = 0.01,
                    Available = 900
                },
                new Lender
                {
                    Name = fixture.Create<string>(),
                    Rate = 0.007,
                    Available = 600
                },
                new Lender
                {
                    Name = fixture.Create<string>(),
                    Rate = 0.004,
                    Available = 300
                }
            };
        }
    }
}
