using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanCalculator.Core.Services.Interfaces
{
    public interface IFileReader
    {
        void ReadCsvAsync(string filePath);
    }
}
