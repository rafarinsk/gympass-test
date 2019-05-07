using System;

namespace gympass_test.core.Models
{
    public class PilotRaceStatistics
    {
        public int FinishPosition { get; set; }
        public TimeSpan TotalRaceDuration { get; set; }
        public double AverageSpeed { get; set; }
        public TimeSpan ArrivalDifferenceFromWinner { get; set; }
        public LapInfo LastLap { get; set; }
        public LapInfo BestLap { get; set; }
    }
}
