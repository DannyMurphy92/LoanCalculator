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
    public class LoanCalculatorServiceTestFixture
    {
        private IFixture fixture;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());
        }


        [Test]
        public void CalculateLoan_WhenAmountNotAvailable_ReturnErrorResponse()
        {
            // Arrange
            var amount = 1000d;
            var lenders = new List<Lender>
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

            var subject = fixture.Create<LoanCalculatorService>();

            // Act
            var result = subject.CalculateLoan(amount, fixture.Create<int>(), lenders);

            // Assert
            Assert.AreEqual(false, result.LenderAvailable);
        }
    }
}
