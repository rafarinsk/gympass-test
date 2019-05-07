using System.Collections.Generic;
using System.IO;
using gympass_test.core.Models;

namespace gympass_test.IO.Abstractions
{
    public interface IRaceLogReader
    {
        IEnumerable<LapInfo> Read(TextReader reader, int skipLines);
    }
}
