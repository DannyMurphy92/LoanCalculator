﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LoanCalculator.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new CoreClient().Run(args).Wait();
            
            Console.ReadLine();
        }
    }
}
