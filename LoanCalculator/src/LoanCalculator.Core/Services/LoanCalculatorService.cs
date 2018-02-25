using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanCalculator.Core.Models;
using LoanCalculator.Core.Services.Interfaces;

namespace LoanCalculator.Core.Services
{
    public class LoanCalculatorService : ILoanCalculatorService
    {
        public CalculateLoanResponse CalculateLoan(double amount, int LoanPeriodMonths, IList<Lender> lenders)
        {
            
            var lender = GetLowestRateLender(amount, lenders);
            if (lender != null)
            {
                return new CalculateLoanResponse
                {
                    LenderAvailable = true,
                    Lender = lender
                };
            }

            return new CalculateLoanResponse
            {
                LenderAvailable = false
            };
        }

        private Lender GetLowestRateLender(double amount, IList<Lender> lenders)
        {
            return lenders
                .Where(l => l.Available >= amount)
                .OrderBy(l => l.Rate)
                .FirstOrDefault();
        }

        //Based off http://www.financeformulas.net/Loan_Payment_Formula.html
        public double CalculateMonthlyrepayment(double principal, int loanPeriodMonths, double interestRate)
        {
            var monthlyRate = Math.Pow((1 + interestRate), (1d/12d)) - 1;

            var upperFraction = principal * monthlyRate;

            var lowerFraction = (1 - Math.Pow((1 + monthlyRate), -loanPeriodMonths));

            return upperFraction / lowerFraction;
        }
    }
}
