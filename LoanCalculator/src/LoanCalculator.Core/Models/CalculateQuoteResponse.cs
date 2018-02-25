namespace LoanCalculator.Core.Models
{
    public class CalculateQuoteResponse
    {
        public bool LenderAvailable { get; set; }

        public double Rate { get; set; }

        public double MonthlyRepayment { get; set; }
    }
}
