using System;
using System.Collections.Generic;
using System.IO;
using gympass_test.core.Models;
using gympass_test.IO.Abstractions;

namespace gympass_test.IO
{
    public class RaceLogReader : IRaceLogReader
    {
        private readonly IRaceLogLineParser _parser;

        public RaceLogReader(IRaceLogLineParser parser)
        {
            this._parser = parser;
        }

        public IEnumerable<LapInfo> Read(TextReader reader, int skipLines = 1)
        {
            if (skipLines < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(_parser), $"{nameof(skipLines)} cannot be negative");
            }
            var raceData = new List<LapInfo>();
            var line = reader.ReadLine();
            while (line != null)
            {
                if (skipLines-- <= 0 && line.Length > 0)
                {
                    var lap = _parser.Parse(line);
                    raceData.Add(lap);
                }
                line = reader.ReadLine();
            }

            return raceData;
        }
    }
}
