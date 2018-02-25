using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanCalculator.Core.Models
{
    public class CalculateLoanResponse
    {
        public bool LenderAvailable { get; set; }

        public Lender Lender { get; set; }

        public double MonthlyRepayment { get; set; }

        public double TotalRepayment { get; set; }
    }
}
