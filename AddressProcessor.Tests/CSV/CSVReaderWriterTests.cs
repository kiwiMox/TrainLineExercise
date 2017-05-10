using NUnit.Framework;
using AddressProcessing.CSV;
using System.IO;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private CSVReaderWriter _sut;

        private const string _contactsCSVPath = "..\\debug\\test_data\\contacts.csv";
        private const string _outputTxtPath = "..\\debug\\test_data\\output.txt";

        private const string _testName = "P. Sherman";
        private const string _testAddress = "42 Wallaby Way|Sydney";

        private const string _expectedLineOneName = "Shelby Macias";
        private const string _expectedLineOneAddress = "3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England";
        private const string _expectedLineTwoName = "Porter Coffey";
        private const string _expectedLineTwoAddress = "Ap #827-9064 Sapien. Rd.|Palo Alto|Fl.|HM0G 0YR|Scotland";

        [SetUp]
        public void SetUp()
        {
            _sut = new CSVReaderWriter();            
        }

        [TearDown]
        public void TearDown()
        {
            _sut.Close();
        }

        [Test]
        public void Open_With_Read_Mode_Should_Not_Throw_Exception()
        {
            // Arrange
            // Act
            // Assert
            Assert.DoesNotThrow(() => _sut.Open(_contactsCSVPath, CSVReaderWriter.Mode.Read));            
        }

        [Test]
        public void Open_With_Write_Mode_Should_Not_Throw_Exception()
        {
            // Arrange
            // Act
            // Assert
            Assert.DoesNotThrow(() => _sut.Open(_outputTxtPath, CSVReaderWriter.Mode.Write));
        }

        [Test]
        public void Write_should_create_file()
        {
            // Arrange
            _sut.Open(_outputTxtPath, CSVReaderWriter.Mode.Write);

            // Act
            _sut.Write(new[] { _testName, _testAddress });

            // Assert
            Assert.IsTrue(new FileInfo(_outputTxtPath).Exists);
        }

        [Test]
        public void Read_single_line_should_return_true_and_not_alter_values()
        {
            // Arrange
            var name = _testName;
            var address = _testAddress;
            _sut.Open(_contactsCSVPath, CSVReaderWriter.Mode.Read);

            // Act
            var result = _sut.Read(name, address);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(_testName, name);
            Assert.AreEqual(_testAddress, address);
        }

        [Test]
        public void Read_single_line_should_return_expected_details()
        {
            // Arrange
            var name = _testName;
            var address = _testAddress;
            _sut.Open(_contactsCSVPath, CSVReaderWriter.Mode.Read);

            // Act
            var result = _sut.Read(out name, out address);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(_expectedLineOneName, name);
            Assert.AreEqual(_expectedLineOneAddress, address);
        }

        [Test]
        public void Read_multiple_lines_should_return_expected_details()
        {
            // Arrange
            var name = _testName;
            var address = _testAddress;
            _sut.Open(_contactsCSVPath, CSVReaderWriter.Mode.Read);

            // Act
            var result = _sut.Read(out name, out address);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(_expectedLineOneName, name);
            Assert.AreEqual(_expectedLineOneAddress, address);

            // Act
            result = _sut.Read(out name, out address);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(_expectedLineTwoName, name);
            Assert.AreEqual(_expectedLineTwoAddress, address);
        }

        [Test]
        public void Write_then_read_from_same_file_should_return_written_values()
        {
            // Arrange
            var name = _testName;
            var address = _testAddress;
            _sut.Open(_outputTxtPath, CSVReaderWriter.Mode.Write);            

            // Act
            _sut.Write(new[] { name, address });            

            // Assert
            Assert.IsTrue(new FileInfo(_outputTxtPath).Exists);


            // Arrange
            _sut.Close();
            _sut.Open(_outputTxtPath, CSVReaderWriter.Mode.Read);

            // Act
            var result = _sut.Read(out name, out address);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(_testName, name);
            Assert.AreEqual(_testAddress, address);
        }

    }
}