using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanCalculator.Core.Models;

namespace LoanCalculator.Core.Services.Interfaces
{
    public interface IQuoteCalculatorService
    {
        CalculateQuoteResponse CalculateQuote(double amount, int loanPeriodMonths, IList<Lender> lenders);

        double CalculateMonthlyrepayment(double principal, int loanPeriodMonths, double interestRate);
    }
}
