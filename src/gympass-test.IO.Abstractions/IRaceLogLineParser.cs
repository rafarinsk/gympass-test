using gympass_test.core.Models;

namespace gympass_test.IO.Abstractions
{
    public interface IRaceLogLineParser
    {
        LapInfo Parse(string raceLogLine);
    }

}
