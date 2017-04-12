using System.Collections.Generic;
using System.IO;

namespace CompoundInterest.Components
{
    public abstract class FileReader
    {
        protected FileReader(string fileName)
        {
            FileName = fileName;
        }

        protected string FileName { get; }

        protected IEnumerable<string> ReadFile()
        {
            if (!File.Exists(FileName))
                throw new FileNotFoundException();

            foreach (var line in File.ReadAllLines(FileName))
                yield return line;
        }
    }
}