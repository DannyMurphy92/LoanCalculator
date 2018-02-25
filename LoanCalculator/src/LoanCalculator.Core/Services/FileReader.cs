using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanCalculator.Core.Services.Interfaces;

namespace LoanCalculator.Core.Services
{
    public class FileReader : IFileReader
    {
        private readonly IFileSystem fileSystem;

        public FileReader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void ReadCsvAsync(string filePath)
        {
            if (fileSystem.File.Exists(filePath))
            {
                return;
                //using(var reader  = new CsvReader)
            }

            throw new FileNotFoundException();
        }
    }
}
