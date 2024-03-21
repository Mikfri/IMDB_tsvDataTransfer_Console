using EFCore.BulkExtensions;
using IMDB_EfDbCons.DataContext;
using IMDB_EfDbCons.Insertions;
using IMDB_EfDbCons.Models;
using IMDB_EfDbCons.Records;
using System;
using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {
        string nameBasicsTsv = @"C:\Users\mikkf\OneDrive\Dokumenter\Visual Studio 2022\TSVs\name.basics.tsv\data.tsv";
        string titleBasicsTsv = @"C:\Users\mikkf\OneDrive\Dokumenter\Visual Studio 2022\TSVs\title.basics.tsv\data.tsv";
        string titleCrewTsv = @"C:\Users\mikkf\OneDrive\Dokumenter\Visual Studio 2022\TSVs\title.crew.tsv\data.tsv";

        Console.WriteLine("Starting program...");
        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            using (var context = new IMDb_Context())
            {
                Console.WriteLine("Created context...");
                var loader = new CsvLoader();
                Console.WriteLine("Created CSV loader...");


                // Load the data into instances of the Record class
                var nameRecords = loader.LoadCsv<NameBasicsRecord>(nameBasicsTsv, 50000);
                Console.WriteLine($"Loaded {nameRecords.Count} records from {nameBasicsTsv} TSV file...");
                var titleRecords = loader.LoadCsv<TitleBasicsRecord>(titleBasicsTsv, 50000);
                Console.WriteLine($"Loaded {titleRecords.Count} records from {titleBasicsTsv} TSV file...");
                var titleCrewRecords = loader.LoadCsv<TitleCrewRecord>(titleCrewTsv, 50000);
                Console.WriteLine($"Loaded {titleCrewRecords.Count} records from {titleCrewTsv} TSV file...");


                var (persons, professions, personalCareers, personalBlockbusters) = NameBasicsProcessor.ProcessNameBasicsRecords(nameRecords);
                var (movieBases, titleTypes, genres, movieGenres) = TitleBasicsProcessor.ProcessTitleBasicsRecords(titleRecords);
                var (directors, writers) = TitleCrewProcessor.ProcessTitleCrewRecords(titleCrewRecords);



                // Use BulkInsert to insert the records for each table in bulk
                context.BulkInsert(persons);
                context.BulkInsert(professions);
                context.BulkInsert(personalCareers);
                context.BulkInsert(personalBlockbusters);
                //movieBase.tsv
                context.BulkInsert(movieBases);
                context.BulkInsert(titleTypes);
                context.BulkInsert(genres);
                context.BulkInsert(movieGenres);
                //titleCrew.tsv
                context.BulkInsert(directors);
                context.BulkInsert(writers);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        stopwatch.Stop();
        Console.WriteLine($"Total time elapsed: {stopwatch.Elapsed}");
        Console.WriteLine("End of program...");       
    }

}



