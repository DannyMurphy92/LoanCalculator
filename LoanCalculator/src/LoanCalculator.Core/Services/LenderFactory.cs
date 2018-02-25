using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanCalculator.Core.Models;
using LoanCalculator.Core.Services.Interfaces;

namespace LoanCalculator.Core.Services
{
    public class LenderFactory : ILenderFactory
    {
        private readonly IFileSystem fileSystem;

        public LenderFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public async Task<IList<Lender>> CreateLendersFromCsvFileAsync(string filePath)
        {
            if (!fileSystem.File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            using (var reader = new StreamReader(filePath))
            {
                var result = new List<Lender>();
                string currentLine;
                while ((currentLine = await reader.ReadLineAsync()) != null)
                {
                    var lineProps = currentLine.Split(',');
                    if (lineProps.Length >= 3)
                    {
                        string name = lineProps[0];
                        double rate;
                        double available;
                        var valid = double.TryParse(lineProps[1], out rate);
                        valid &= double.TryParse(lineProps[2], out available);

                        if (valid)
                        {
                            result.Add( new Lender
                            {
                                Available = available,
                                Rate = rate,
                                Name = name
                            });
                        }
                    }
                }

                return result;
            }
        }
    }
}
