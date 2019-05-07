using System;
using System.Collections.Generic;
using System.Linq;
using gympass_test.core.Models;

namespace gympass_test.IO
{
    public static class RaceProcessor
    {
        public static IEnumerable<PilotRaceStatistics> ProcessLaps(IEnumerable<LapInfo> laps, int raceLaps, out LapInfo bestLap)
        {
            //Esse código poderia ser bem mais simples, porém, por uma queswtão de desempenho e para conseguir calcular todas as informações com o mínimo de interações, decidi usar uma lógica mais complexa.

            var pilotStats = new Dictionary<string, PilotRaceStatistics>();
            var completed = new HashSet<string>(); //pilotos que completaram a corrida
            var raceFinished = false;
            foreach (var lap in laps.OrderBy(lap => lap.FinishTime))
            {
                if(raceFinished && completed.Contains(lap.PilotCode))
                {
                    //Ignoro voltas extras, feitas após o término da corrida
                    continue;
                }
                if(raceFinished)
                {
                    completed.Add(lap.PilotCode);
                }
                if (!pilotStats.TryGetValue(lap.PilotCode, out var stat))
                {
                    stat = new PilotRaceStatistics();
                    pilotStats.Add(lap.PilotCode, stat);
                }
                UpdatePilotStat(stat, lap);
                if(lap.Lap == raceLaps && !raceFinished)
                {
                    //sinalizo o fim da corrida para considerar apenas a volta em andamento dos pilotos
                    raceFinished = true;
                    completed.Add(lap.PilotCode);
                }
            }

            bestLap = null;
            var result = pilotStats.Values.ToArray();
            ConsolidatePilotStats(result, ref bestLap);
            return pilotStats.Values.ToArray();
        }

        public static double GetTrackLength(LapInfo lap)
        {
            //Considerando a velocidade média em Km/h e usando uma preciso de 3 casas decimais (metros)
            return Math.Round(lap.Duration.TotalHours * lap.AverageSpeed, 3);
        }

        private static void UpdatePilotStat(PilotRaceStatistics stat, LapInfo lap)
        {
            if (stat.BestLap == null)
            {
                stat.BestLap = lap;
            }
            else
            {
                if (stat.BestLap.Duration > lap.Duration)
                {
                    stat.BestLap = lap;
                }
            }

            stat.TotalRaceDuration += lap.Duration;
            stat.LastLap = lap;
        }

        private static void ConsolidatePilotStats(PilotRaceStatistics[] pilotStats, ref LapInfo bestLap)
        {
            if(pilotStats.Length == 0)
            {
                return;
            }
            
            var trackLength = GetTrackLength(pilotStats[0].LastLap);
            var position = 1;
            LapInfo winnerLap = null;
            bestLap = null;
            foreach (var stat in pilotStats.OrderBy(stat => stat.TotalRaceDuration))
            {
                if (position == 1)
                {
                    winnerLap = stat.LastLap;
                }
                stat.FinishPosition = position++;
                stat.ArrivalDifferenceFromWinner = stat.LastLap.FinishTime - winnerLap.FinishTime;
                stat.AverageSpeed = (trackLength * stat.LastLap.Lap) / stat.TotalRaceDuration.TotalHours; //distância total percorrida das voltas válidas * o tempo total das voltas válidas
                if (bestLap == null || bestLap.Duration > stat.BestLap.Duration)
                {
                    //calcula a melhor volta da corrida
                    bestLap = stat.BestLap;
                }
            }
        }
    }
}
