using System;
using System.IO;
using gympass_test.core.Abstractions;
using gympass_test.IO.Abstractions;

namespace gympass_test.IO
{
    public class RaceLogFileRepositoryFactory : IRaceRepositoryFactory
    {
        private readonly IRaceLogReader _reader;

        public RaceLogFileRepositoryFactory(IRaceLogReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public IRaceRepository CreateRaceRepository(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (fileName.Length == 0)
            {
                throw new ArgumentException(nameof(fileName));
            }

            var file = new FileInfo(fileName);
            if (!file.Exists)
            {
                throw new FileNotFoundException("Race file doesn't exists", fileName);
            }

            using (var fileReader = file.OpenText())
            {
                var raceLaps = _reader.Read(fileReader, RaceLogStructure.HasColumnHeaders ? 1 : 0);
                return new RaceLogFileRepository(raceLaps);
            }
        }
    }
}
