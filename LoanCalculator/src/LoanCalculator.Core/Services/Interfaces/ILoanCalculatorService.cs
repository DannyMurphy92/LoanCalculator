using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanCalculator.Core.Models;

namespace LoanCalculator.Core.Services.Interfaces
{
    public interface ILoanCalculatorService
    {
        CalculateLoanResponse CalculateLoan(double amount, int LoanPeriodMonths, IList<Lender> lenders);
    }
}
