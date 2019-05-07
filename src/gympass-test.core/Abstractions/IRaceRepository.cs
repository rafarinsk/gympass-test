using System.Collections.Generic;
using gympass_test.core.Models;

namespace gympass_test.core.Abstractions
{
    public interface IRaceRepository
    {
        IEnumerable<PilotRaceStatistics> GetRaceStatistics();
        LapInfo GetRaceBestLap();
    }
}
