using System;

namespace gympass_test.core.Models
{
    public class LapInfo
    {
        public TimeSpan FinishTime { get; set; }
        public string PilotCode { get; set; }
        public string PilotName { get; set; }
        public int Lap { get; set; }
        public TimeSpan Duration { get; set; }
        public double AverageSpeed { get; set; }
        public string Pilot => $"{PilotCode} - {PilotName}";

        public override string ToString()
        {
            return $"{FinishTime:h\\:mm\\:ss\\.fff} {Pilot} {Lap} {Duration:m\\:ss\\.fff} {AverageSpeed:N3}";
        }
    }
}
