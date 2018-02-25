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
                return new CalculateLoanResponse();
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
    }
}
