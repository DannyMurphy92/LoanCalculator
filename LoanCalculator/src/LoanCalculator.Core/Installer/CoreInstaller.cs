using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LoanCalculator.Core.Services;
using LoanCalculator.Core.Services.Interfaces;

namespace LoanCalculator.Core.Installer
{
    public class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IFileSystem>()
                    .ImplementedBy<FileSystem>());
            container.Register(
                Component.For<ILenderFactory>()
                    .ImplementedBy<LenderFactory>());
        }
    }
}
