namespace LoanCalculator.Core.Models
{
    public class CalculateLoanResponse
    {
        public bool LenderAvailable { get; set; }

        public Lender Lender { get; set; }

        public double MonthlyRepayment { get; set; }
    }
}
