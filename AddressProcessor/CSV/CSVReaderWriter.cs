using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        private const char _separator = '\t';

        [Flags]
        public enum Mode
        {
            Read = 1,
            Write = 2
        };

        public void Open(string fileName, Mode mode)
        {
            switch (mode)
            {
                case Mode.Read:
                    _readerStream = File.OpenText(fileName);
                    break;

                case Mode.Write:
                    _writerStream = new FileInfo(fileName).CreateText();
                    break;

                default:
                    throw new Exception("Unknown file mode for " + fileName);
                    break;
            }
        }

        public void Write(params string[] columns)
        {
            _writerStream.WriteLine(string.Join(_separator.ToString(), columns));
        }

        public bool Read(string column1, string column2)
        {
            return Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
            string line = _readerStream.ReadLine();
            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            string[] columns = line.Split(_separator);
            if (columns.Length < 2)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            column1 = columns[0];
            column2 = columns[1];

            return true;
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}