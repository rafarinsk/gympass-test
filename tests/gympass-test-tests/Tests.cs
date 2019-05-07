using System;
using System.IO;
using System.Linq;
using gympass_test.core.Abstractions;
using gympass_test.IO;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        const string RaceData = @"Hora                               Piloto             Nº Volta   Tempo Volta       Velocidade média da volta
23:49:08.277      038 – F.MASSA                           1		1:02.852                        44,275
23:49:10.858      033 – R.BARRICHELLO                     1		1:04.352                        43,243
23:49:11.075      002 – K.RAIKKONEN                       1             1:04.108                        43,408
23:49:12.667      023 – M.WEBBER                          1		1:04.414                        43,202
23:49:30.976      015 – F.ALONSO                          1		1:18.456			35,47
23:50:11.447      038 – F.MASSA                           2		1:03.170                        44,053
23:50:14.860      033 – R.BARRICHELLO                     2		1:04.002                        43,48
23:50:15.057      002 – K.RAIKKONEN                       2             1:03.982                        43,493
23:50:17.472      023 – M.WEBBER                          2		1:04.805                        42,941
23:50:37.987      015 – F.ALONSO                          2		1:07.011			41,528
23:51:14.216      038 – F.MASSA                           3		1:02.769                        44,334
23:51:18.576      033 – R.BARRICHELLO		          3		1:03.716                        43,675
23:51:19.044      002 – K.RAIKKONEN                       3		1:03.987                        43,49
23:51:21.759      023 – M.WEBBER                          3		1:04.287                        43,287
23:51:46.691      015 – F.ALONSO                          3		1:08.704			40,504
23:52:01.796      011 – S.VETTEL                          1		3:31.315			13,169
23:52:17.003      038 – F.MASS                            4		1:02.787                        44,321
23:52:22.586      033 – R.BARRICHELLO		          4		1:04.010                        43,474
23:52:22.120      002 – K.RAIKKONEN                       4		1:03.076                        44,118
23:52:25.975      023 – M.WEBBER                          4		1:04.216                        43,335
23:53:06.741      015 – F.ALONSO                          4		1:20.050			34,763
23:53:39.660      011 – S.VETTEL                          2		1:37.864			28,435
23:54:57.757      011 – S.VETTEL                          3		1:18.097			35,633

";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RaceLogParserNormalizeLineTest()
        {
            var normalized = RaceLogLineParser.Normalize("23:51:46.691      015 – F.ALONSO                          3		1:08.704			40,504");
            Assert.AreEqual(normalized, "23:51:46.691\t015 – F.ALONSO\t3\t1:08.704\t40,504");
        }

        [Test]
        public void RaceLogParserCorrectLineTest()
        {
            var parser = new RaceLogLineParser();
            var lap = parser.Parse(@"23:49:12.667      023 – M.WEBBER                          1		1:04.414                        43,202");
            Assert.AreEqual(lap.FinishTime, new TimeSpan(0, 23, 49, 12, 667), "FinishTime different than expected");
            Assert.AreEqual(lap.PilotCode, "023", "PilotCode different than expected");
            Assert.AreEqual(lap.PilotName, "M.WEBBER", "PilotName different than expected");
            Assert.AreEqual(lap.Lap, 1, "Lap different than expected");
            Assert.AreEqual(lap.Duration, new TimeSpan(0, 0, 1, 4, 414), "Duration different than expected");
            Assert.AreEqual(lap.AverageSpeed, 43.202, 0.0001, "AverageSpeed different than expected");
        }

        [Test]
        public void RaceLogParserInvalidLineMissingFieldTest()
        {
            var parser = new RaceLogLineParser();
            Assert.Throws(typeof(FormatException), () => parser.Parse("23:49:12.667	023 – M.WEBBER	3	1:04.414"), "Parser didn't throws FormatException when missing fields");
        }

        [Test]
        public void RaceLogParserInvalidLineInvalidLapTest()
        {
            var parser = new RaceLogLineParser();
            Assert.Throws(typeof(FormatException), () => parser.Parse("23:49:12.667	023 – M.WEBBER	E	1:04.414	43,202"), "Parser didn't throws FormatException with invalid Lap number");
        }

        [Test]
        public void RaceLogParserInvalidLineInvalidFinishTimeTest()
        {
            var parser = new RaceLogLineParser();
            Assert.Throws(typeof(FormatException), () => parser.Parse("23:49.667	023 – M.WEBBER	3	1:04.414	43,202"), "Parser didn't throws FormatException with invalid FinishTime");
        }

        [Test]
        public void RaceLogParserInvalidLineInvalidPilotTest()
        {
            var parser = new RaceLogLineParser();
            Assert.Throws(typeof(FormatException), () => parser.Parse("23:49:12.667	M.WEBBER	3	1:04.414	43,202"), "Parser didn't throws FormatException with inválid Pilot");
        }

        [Test]
        public void RaceLogParserInvalidLineInvalidPilotCodeTest()
        {
            var parser = new RaceLogLineParser();
            Assert.Throws(typeof(FormatException), () => parser.Parse("23:49:12.667	XII - M.WEBBER	3	1:04.414	43,202"), "Parser didn't throws FormatException with inválid PilotCode");
        }

        [Test]
        public void RaceLogParserInvalidLineInvalidAverageSpeedTest()
        {
            var parser = new RaceLogLineParser();
            Assert.Throws(typeof(FormatException), () => parser.Parse("23:49:12.667	023 – M.WEBBER	3	1:04.414	43.202"), "Parser didn't throws FormatException with invalid AverageSpeed");
        }

        [Test]
        public void RaceLogParserInvalidLineInvalidDurationTest()
        {
            var parser = new RaceLogLineParser();
            Assert.Throws(typeof(FormatException), () => parser.Parse("23:49:12.667	023 – M.WEBBER	3	04.414	43,202"), "Parser didn't throws FormatException with invalid Duration");
        }

        [Test]
        public void RaceLogReaderTest()
        {
            var parser = new RaceLogLineParser();
            var reader = new RaceLogReader(parser);
            using (var textReader = new StringReader(RaceData))
            {
                var laps = reader.Read(textReader).ToArray();
                Assert.AreEqual(23, laps.Length);
            }
        }

        [Test]
        public void RaceBestLapTest()
        {
            var parser = new RaceLogLineParser();
            var reader = new RaceLogReader(parser);
            IRaceRepository repo;
            using (var textReader = new StringReader(RaceData))
            {
                var laps = reader.Read(textReader).ToArray();
                repo = new RaceLogFileRepository(laps);
            }
            var bestLap = repo.GetRaceBestLap(); //23:51:14.216	038 – F.MASSA	3	1:02.769	44,334
            Assert.AreEqual(bestLap.Lap, 3);
            Assert.AreEqual(bestLap.Duration, new TimeSpan(0,0,1,2,769));
        }

        [Test]
        public void RaceWinnerTest()
        {
            var parser = new RaceLogLineParser();
            var reader = new RaceLogReader(parser);
            IRaceRepository repo;
            using (var textReader = new StringReader(RaceData))
            {
                var laps = reader.Read(textReader).ToArray();
                repo = new RaceLogFileRepository(laps);
            }
            var winner = repo.GetRaceStatistics().First();
            Assert.AreEqual(winner.FinishPosition, 1);
            Assert.AreEqual(winner.LastLap.Lap, 4);
            Assert.AreEqual(winner.ArrivalDifferenceFromWinner, TimeSpan.Zero);
            Assert.AreEqual(winner.LastLap.PilotCode, "038");
        }

        [Test]
        public void RaceLooserTest()
        {
            var parser = new RaceLogLineParser();
            var reader = new RaceLogReader(parser);
            IRaceRepository repo;
            using (var textReader = new StringReader(RaceData))
            {
                var laps = reader.Read(textReader).ToArray();
                repo = new RaceLogFileRepository(laps);
            }
            var looser = repo.GetRaceStatistics().Last();
            Assert.AreEqual(looser.FinishPosition, 6);
            Assert.AreEqual(looser.LastLap.Lap, 2);
            Assert.AreEqual(looser.ArrivalDifferenceFromWinner, new TimeSpan(0, 0, 1, 22, 657));
            Assert.AreEqual(looser.LastLap.PilotCode, "011");
        }

        [Test]
        public void RaceAverageSpeedTest()
        {
            var parser = new RaceLogLineParser();
            var reader = new RaceLogReader(parser);
            IRaceRepository repo;
            using (var textReader = new StringReader(RaceData))
            {
                var laps = reader.Read(textReader).ToArray();
                repo = new RaceLogFileRepository(laps);
            }
            var looser = repo.GetRaceStatistics().Last();
            Assert.AreEqual(looser.AverageSpeed, 18.001222592737545);
        }

        [Test]
        public void RaceProcessorGetTrackLengthTest()
        {
            var parser = new RaceLogLineParser();
            var reader = new RaceLogReader(parser);
            using (var textReader = new StringReader(RaceData))
            {
                var laps = reader.Read(textReader).ToArray();
                var lengths = laps.Select(RaceProcessor.GetTrackLength);
                Assert.IsTrue(lengths.All(len => len == 0.773));
            }
        }
    }
}