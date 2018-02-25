using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using LoanCalculator.Core.Services;
using NUnit.Framework;
using Moq;

namespace LoanCalculator.Core.UnitTests.Services
{
    [TestFixture]
    public class FileReaderTestFixture
    {
        private IFixture fixture;

        private Mock<IFileSystem> fileSystemMock;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());

            fileSystemMock = fixture.Freeze<Mock<IFileSystem>>();
        }

        [TearDown]
        public void Teardown()
        {
            fileSystemMock.Reset();
        }

        [Test]
        public void ReadCsv_WhenInvoked_ShouldCheckFileExists()
        {
            // Arrange
            var filePath = fixture.Create<string>();
            fileSystemMock.Setup(f => f.File.Exists(filePath))
                .Returns(true);
            var subject = fixture.Create<FileReader>();

            // Act
            subject.ReadCsvAsync(filePath);

            // Assert
            fileSystemMock.Verify(f => f.File.Exists(filePath), Times.Once);
        }


        [Test]
        public void ReadCsv_IfFileDoesntExist_ThrowsException()
        {
            // Arrange
            var filePath = fixture.Create<string>();
            fileSystemMock.Setup(f => f.File.Exists(filePath))
                .Returns(false);
            var subject = fixture.Create<FileReader>();

            // Act
            TestDelegate act = () => subject.ReadCsvAsync(filePath);

            // Assert
            Assert.That(act, Throws.TypeOf<FileNotFoundException>());
        }
    }
}
