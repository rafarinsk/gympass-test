using System;
using System.Globalization;
using gympass_test.core.Models;
using gympass_test.IO.Abstractions;

namespace gympass_test.IO
{
    public class RaceLogLineParser : IRaceLogLineParser
    {
        public LapInfo Parse(string raceLogLine)
        {
            raceLogLine = Normalize(raceLogLine);
            var fields = raceLogLine.Split(RaceLogStructure.FieldSeparator);
            if(fields.Length != RaceLogStructure.FieldCount)
            {
                throw new FormatException("Invalid line");
            }
            try
            {
                var lap = new LapInfo();
                var culture = CultureInfo.GetCultureInfo(RaceLogStructure.Culture);
                for (int i = 0; i < RaceLogStructure.FieldCount; i++)
                {
                    switch(i)
                    {
                        case RaceLogStructure.FinishTimeIndex:
                            lap.FinishTime = TimeSpan.ParseExact(fields[RaceLogStructure.FinishTimeIndex], RaceLogStructure.FinishTimeFormat, culture);
                            break;
                        case RaceLogStructure.Pilot:
                            var pilot = fields[RaceLogStructure.Pilot];
                            var dash = pilot.IndexOf('–');
                            if(dash == -1)
                            {
                                throw new FormatException("Invalid pilot code and name in race log");
                            }
                            var code = pilot.Substring(0, dash).Trim();
                            if (int.TryParse(code, out var _))
                            {
                                lap.PilotCode = code;
                            }
                            else
                            {
                                throw new FormatException("Invalid pilot code in race log");
                            }
                            lap.PilotName = pilot.Substring(dash + 1).Trim();
                            break;
                        case RaceLogStructure.Lap:
                            lap.Lap = int.Parse(fields[RaceLogStructure.Lap]);
                            break;
                        case RaceLogStructure.Duration:
                            lap.Duration = TimeSpan.ParseExact(fields[RaceLogStructure.Duration], RaceLogStructure.DurationFormat, culture);
                            break;
                        case RaceLogStructure.AverageSpeed:
                            if (double.TryParse(fields[RaceLogStructure.AverageSpeed], NumberStyles.AllowDecimalPoint, culture, out var avg))
                            {
                                lap.AverageSpeed = avg;
                            }
                            else
                            {
                                throw new FormatException("Invalid average speed in race log");
                            }
                            break;
                    }
                }
                return lap;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FormatException("Invalid line", ex);
            }
        }

        public static string Normalize(string line)
        {
            line = line.Replace("\t", " ");
            var noSpaces = line.Split(new[] { " – " }, StringSplitOptions.None);
            if (noSpaces.Length != 2)
            {
                throw new FormatException("Invalid line");
            }
            var leftFields = noSpaces[0].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var rightFields = noSpaces[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var normalizedLine = $"{string.Join("\t",leftFields)} – {string.Join("\t", rightFields)}";
            return normalizedLine;
        }
    }

}
