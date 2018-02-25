using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using LoanCalculator.Core.Installer;
using LoanCalculator.Core.Models;
using LoanCalculator.Core.Services.Interfaces;

namespace LoanCalculator.Cli
{
    public class CoreClient
    {
        public async Task Run(string[] args)
        {
            string filePath;
            int amount;
            if (ValidateAndMapArguments(args, out filePath, out amount))
            {
                var container = new WindsorContainer();
                container.Install(new CoreInstaller());

                var lenderFactory = container.Resolve<ILenderFactory>();
                var loanCalculator = container.Resolve<IQuoteCalculatorService>();

                var lenders = await lenderFactory.CreateLendersFromCsvFileAsync(filePath);
                var loanResult = loanCalculator.CalculateQuote(amount, 36, lenders);

                Outputresult(loanResult, amount);
            }
            else
            {
                Console.WriteLine("Incorrect usage, to run: LoanCalculator.Cli.exe <path to csv market file> <amount that is multiple of 100>");
            }
        }

        private void Outputresult(CalculateQuoteResponse quoteResult, int amount)
        {
            if (quoteResult.LenderAvailable)
            {
                Console.WriteLine($"Requested amount £{amount}");
                Console.WriteLine($"Rate {Math.Round((quoteResult.Rate * 100), 1)}%");
                Console.WriteLine($"Monthly repayment: £{Math.Round(quoteResult.MonthlyRepayment, 2)}");
                Console.WriteLine($"Total repayment: £{Math.Round(quoteResult.MonthlyRepayment * 36, 2)}");
            }
            else
            {
                Console.WriteLine($"A quote for £{amount} cannot be provider at this time.");
            }
        }

        private bool ValidateAndMapArguments(string[] args, out string fileName, out int amount)
        {
            fileName = string.Empty;
            amount = 0;
            if (args.Length >= 2)
            {
                fileName = args[0];
                var validNum = int.TryParse(args[1], out amount);

                if (fileName.ToLower().EndsWith(".csv") && validNum && (amount % 100 == 0))
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
