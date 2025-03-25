using EFCore.BulkExtensions;
using IMDB_EfDbCons.Insertions;
using IMDB_EfDbCons.Records;
using IMDbLib.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;

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
            // Check if files exist before proceeding
            if (!File.Exists(nameBasicsTsv))
            {
                Console.WriteLine($"Error: File not found - {nameBasicsTsv}");
                return;
            }
            if (!File.Exists(titleBasicsTsv))
            {
                Console.WriteLine($"Error: File not found - {titleBasicsTsv}");
                return;
            }
            if (!File.Exists(titleCrewTsv))
            {
                Console.WriteLine($"Error: File not found - {titleCrewTsv}");
                return;
            }

            // Konfigurer DbContextOptions med EnableRetryOnFailure
            var optionsBuilder = new DbContextOptionsBuilder<IMDb_Context>();
            optionsBuilder.UseSqlServer(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IMDb_DB; Integrated Security=True;",
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null));

            // Tjek om databasen eksisterer og opret den hvis ikke
            Console.WriteLine("Checking database existence...");
            using (var context = new IMDb_Context(optionsBuilder.Options))
            {
                // Forsøg at oprette databasen hvis den ikke findes
                try
                {
                    bool created = context.Database.EnsureCreated();
                    if (created)
                    {
                        Console.WriteLine("Database was created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Database already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating database: {ex.Message}");
                    Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                    return;
                }
            }

            // Konfigurer BulkConfig med timeout og andre indstillinger
            var bulkConfig = new BulkConfig
            {
                BulkCopyTimeout = 0,  // Ingen timeout
                BatchSize = 4000,     // Mindre batch-størrelse
                UseTempDB = false,    // Undgå brug af TempDB
                SqlBulkCopyOptions = Microsoft.Data.SqlClient.SqlBulkCopyOptions.TableLock
            };

            // Opret en ny DbContext med samme options for bulk operationer
            using (var context = new IMDb_Context(optionsBuilder.Options))
            {
                Console.WriteLine("Created context with retry policy...");
                var loader = new CsvLoader();
                Console.WriteLine("Created CSV loader...");

                // Kontroller at databaseforbindelsen virker
                try
                {
                    context.Database.OpenConnection();
                    context.Database.CloseConnection();
                    Console.WriteLine("Database connection test successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database connection test failed: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }
                    return;
                }

                // Load the data into instances of the Record class
                var nameRecords = loader.LoadCsv<NameBasicsRecord>(nameBasicsTsv, 100000);
                Console.WriteLine($"Loaded {nameRecords.Count} records from {nameBasicsTsv} TSV file...");
                var titleRecords = loader.LoadCsv<TitleBasicsRecord>(titleBasicsTsv, 100000);
                Console.WriteLine($"Loaded {titleRecords.Count} records from {titleBasicsTsv} TSV file...");
                var titleCrewRecords = loader.LoadCsv<TitleCrewRecord>(titleCrewTsv, 100000);
                Console.WriteLine($"Loaded {titleCrewRecords.Count} records from {titleCrewTsv} TSV file...");

                var (persons, professions, personalCareers, knownForTitles) = NameBasicsProcessor.ProcessNameBasicsRecords(nameRecords);
                var (movieBases, titleTypes, genres, movieGenres) = TitleBasicsProcessor.ProcessTitleBasicsRecords(titleRecords);
                var (directors, writers) = TitleCrewProcessor.ProcessTitleCrewRecords(titleCrewRecords);

                try
                {
                    // Brug BulkInsert med den konfigurerede bulkConfig
                    Console.WriteLine("Inserting persons...");
                    context.BulkInsert(persons, bulkConfig);
                    Console.WriteLine("Inserting professions...");
                    context.BulkInsert(professions, bulkConfig);
                    Console.WriteLine("Inserting personal careers...");
                    context.BulkInsert(personalCareers, bulkConfig);
                    Console.WriteLine("Inserting known for titles...");
                    context.BulkInsert(knownForTitles, bulkConfig);
                    //movieBase.tsv
                    Console.WriteLine("Inserting movie bases...");
                    context.BulkInsert(movieBases, bulkConfig);
                    Console.WriteLine("Inserting title types...");
                    context.BulkInsert(titleTypes, bulkConfig);
                    Console.WriteLine("Inserting genres...");
                    context.BulkInsert(genres, bulkConfig);
                    Console.WriteLine("Inserting movie genres...");
                    context.BulkInsert(movieGenres, bulkConfig);
                    //titleCrew.tsv
                    Console.WriteLine("Inserting directors...");
                    context.BulkInsert(directors, bulkConfig);
                    Console.WriteLine("Inserting writers...");
                    context.BulkInsert(writers, bulkConfig);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BulkInsert operation failed: {ex.Message}");
                    Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"Total time elapsed: {stopwatch.Elapsed}");
        Console.WriteLine("End of program...");
    }
}