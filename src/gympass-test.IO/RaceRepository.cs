using System.Collections.Generic;
using gympass_test.core.Abstractions;
using gympass_test.core.Models;

namespace gympass_test.IO
{
    public class RaceLogFileRepository : IRaceRepository
    {
        private readonly IEnumerable<PilotRaceStatistics> _raceStatistics;
        private readonly LapInfo _bestLap;

        public RaceLogFileRepository(IEnumerable<LapInfo> race)
        {
            _raceStatistics = RaceProcessor.ProcessLaps(race, RaceLogStructure.RaceLaps, out _bestLap);
        }

        public LapInfo GetRaceBestLap()
        {
            return _bestLap;
        }

        public IEnumerable<PilotRaceStatistics> GetRaceStatistics()
        {
            return _raceStatistics;
        }
    }
}
