using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using LoanCalculator.Core.Installer;
using LoanCalculator.Core.Services.Interfaces;

namespace LoanCalculator.Cli
{
    public class CoreClient
    {
        public void Run(string[] args)
        {
            string filePath;
            int amount;
            if (ValidateAndMapArguments(args, out filePath, out amount))
            {

                var container = new WindsorContainer();
                container.Install(new CoreInstaller());

                var fileReader = container.Resolve<IFileReader>();

                fileReader.ReadCsvAsync(filePath);
            }
            else
            {
                Console.WriteLine("Incorrect usage, to run: LoanCalculator.Cli.exe <path to csv market file> <amount that is multiple of 100>");
            }
        }

        private bool ValidateAndMapArguments(string[] args, out string fileName, out int amount)
        {
            fileName = string.Empty;
            amount = 0;
            if (args.Length >= 2)
            {
                fileName = args[0];
                amount = 0;
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
