using System;
using System.Collections.Generic;
using System.Linq;
using gympass_test.core.Abstractions;
using gympass_test.core.Models;
using gympass_test.IO;
using gympass_test.IO.Abstractions;

namespace gympass_test
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                throw new ArgumentException("Race file name must be informed as the first argument", nameof(args));
            }
            
            IRaceLogLineParser parser = new RaceLogLineParser();
            IRaceLogReader reader = new RaceLogReader(parser);
            IRaceRepositoryFactory repositoryFactory = new RaceLogFileRepositoryFactory(reader);
            IRaceRepository repo = repositoryFactory.CreateRaceRepository(args[0]);

            var stats = repo.GetRaceStatistics();
            var bestLap = repo.GetRaceBestLap();
            PrintResults(stats, bestLap);
        }

        private static void PrintResults(IEnumerable<PilotRaceStatistics> stats, LapInfo bestLap)
        {
            Console.WriteLine("Posição Chegada\tCódigo Piloto\tNome Piloto\tQtde Voltas Completadas\tTempo Total de Prova\tMelhor Volta\tTempo Melhor Volta\tVelocidade Média da Corrida\tDiferença de tempo para o vencedor");
            foreach (var stat in stats.OrderBy(stat => stat.FinishPosition))
            {
                Console.WriteLine($"{stat.FinishPosition}\t{stat.BestLap.PilotCode}\t{stat.BestLap.PilotName}\t{stat.LastLap.Lap}\t{stat.TotalRaceDuration:m\\:ss\\.fff}\t{stat.BestLap.Lap}\t{stat.BestLap.Duration:m\\:ss\\.fff}\t{stat.AverageSpeed:N3}\t{stat.ArrivalDifferenceFromWinner:m\\:ss\\.fff}");
            }
            Console.WriteLine();
            Console.WriteLine($"Melhor volta da corrida:");
            Console.WriteLine("Piloto\tNº Volta\tTempo Volta\tVelocidade média da volta");
            Console.WriteLine($"{bestLap.Pilot}\t{bestLap.Lap}\t{bestLap.Duration:m\\:ss\\.fff}\t{bestLap.AverageSpeed:N3}");
        }
    }
}
