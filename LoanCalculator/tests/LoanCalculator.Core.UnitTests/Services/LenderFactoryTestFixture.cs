using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMoq.Helpers;
using LoanCalculator.Core.Models;
using LoanCalculator.Core.Services;
using Microsoft.Practices.ObjectBuilder2;
using NUnit.Framework;
using Moq;

namespace LoanCalculator.Core.UnitTests.Services
{
    [TestFixture]
    public class LenderFactoryTestFixture
    {
        private IFixture fixture;

        private Mock<IFileSystem> fileSystemMock;

        private IList<Lender> lenders;
        private IList<string> fileLines;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            fixture = new Fixture().Customize(new AutoConfiguredMoqCustomization());

            fileSystemMock = fixture.Freeze<Mock<IFileSystem>>();
        }

        [SetUp]
        public void Setup()
        {
            lenders = new List<Lender>();
            fileLines = new List<string>();

            lenders = fixture.CreateMany<Lender>(10).ToList();
            lenders.ForEach(l => fileLines.Add($"{l.Name},{l.Rate},{l.Available}"));


            fileSystemMock.Setup(f => f.File.Exists(It.IsAny<string>()))
                .Returns(true);

            fileSystemMock.Setup(f => f.File.OpenRead(It.IsAny<string>()))
                .Returns(FileLinesToStream);
        }

        [TearDown]
        public void Teardown()
        {
            fileSystemMock.Reset();
        }

        [Test]
        public async Task CreateLendersFromCsvFile_WhenInvoked_ShouldCheckFileExists()
        {
            // Arrange
            var filePath = fixture.Create<string>();
            var subject = fixture.Create<LenderFactory>();

            // Act
            await subject.CreateLendersFromCsvFileAsync(filePath);

            // Assert
            fileSystemMock.Verify(f => f.File.Exists(filePath), Times.Once);
        }

        [Test]
        public void CreateLendersFromCsvFile_IfFileDoesntExist_ThrowsException()
        {
            // Arrange
            var filePath = fixture.Create<string>();
            fileSystemMock.Setup(f => f.File.Exists(filePath))
                .Returns(false);
            var subject = fixture.Create<LenderFactory>();

            // Act
            AsyncTestDelegate act = () => subject.CreateLendersFromCsvFileAsync(filePath);

            // Assert
            Assert.That(act, Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public async Task CreateLendersFromCsvFile_WhenInvoked_ShouldReturnLenderForEveryValidLine()
        {
            // Arrange
            var subject = fixture.Create<LenderFactory>();

            // Act
            var result = await subject.CreateLendersFromCsvFileAsync(fixture.Create<string>());

            // Assert
            Assert.AreEqual(fileLines.Count, result.Count);
            Assert.True(result.All(r => lenders.Any(l => 
                l.Name == r.Name &&
                l.Rate == r.Rate &&
                l.Available == r.Available
            )));
        }

        [Test]
        public async Task CreateLendersFromCsv_WhenInvokedWithAnInvalidString_SkipsLineAndParsesRestOfFile()
        {
            // Arrange
            fileLines[0] = fixture.Create<string>();
            var subject = fixture.Create<LenderFactory>();

            // Act
            var result = await subject.CreateLendersFromCsvFileAsync(fixture.Create<string>());

            // Assert
            Assert.AreEqual(fileLines.Count - 1, result.Count);
            Assert.True(result.All(r => lenders.Any(l =>
                l.Name == r.Name &&
                l.Rate == r.Rate &&
                l.Available == r.Available
            )));
        }


        [Test]
        public async Task CreateLendersFromCsv_WhenLineHasInvalidRate_SkipsLine()
        {
            // Arrange
            var ln = fileLines[0].Split(',');
            ln[1] = "Not a rate";
            fileLines[0] = string.Join(",", ln);

            var subject = fixture.Create<LenderFactory>();

            // Act
            var result = await subject.CreateLendersFromCsvFileAsync(fixture.Create<string>());

            // Assert
            Assert.AreEqual(fileLines.Count - 1, result.Count);
            Assert.True(result.All(r => lenders.Any(l =>
                l.Name == r.Name &&
                l.Rate == r.Rate &&
                l.Available == r.Available
            )));
        }


        [Test]
        public async Task CreateLendersFromCsv_WhenLineHasInvalidAvailable_SkipsLine()
        {
            // Arrange
            var ln = fileLines[0].Split(',');
            ln[2] = "Not a rate";
            fileLines[0] = string.Join(",", ln);

            var subject = fixture.Create<LenderFactory>();

            // Act
            var result = await subject.CreateLendersFromCsvFileAsync(fixture.Create<string>());

            // Assert
            Assert.AreEqual(fileLines.Count - 1, result.Count);
            Assert.True(result.All(r => lenders.Any(l =>
                l.Name == r.Name &&
                l.Rate == r.Rate &&
                l.Available == r.Available
            )));
        }

        private Stream FileLinesToStream()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
                
            foreach (var line in fileLines)
            {
                writer.WriteLine(line);
            }

            writer.Flush();
            stream.Position = 0;
                
            return stream;
        
                
        }
    }
}
