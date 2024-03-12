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
        

        Console.WriteLine("Starting program...");
        Stopwatch sw = Stopwatch.StartNew();

        try
        {
            using (var context = new IMDb_Context())
            {
                Console.WriteLine("Created context...");

                var loader = new CsvLoader();
                Console.WriteLine("Created CSV loader...");

                // Load the data into instances of the Record class
                var nameRecords = loader.LoadCsv<NameBasicsRecord>(nameBasicsTsv, 10000);
                Console.WriteLine($"Loaded {nameRecords.Count} records from {nameBasicsTsv} TSV file...");

                var titleRecords = loader.LoadCsv<TitleBasicsRecord>(titleBasicsTsv, 10000);
                Console.WriteLine($"Loaded {titleRecords.Count} records from {titleBasicsTsv} TSV file...");

                // Create caches for professions, movies, and genres
                var professionsCache = new Dictionary<string, Profession>();
                var moviesCache = new Dictionary<string, MovieBase>();
                var genresCache = new Dictionary<string, Genre>();

                int nameRecordNumber = 0;
                int titleRecordNumber = 0;
                int batchSize = 500; // Adjust this value based on your performance measurements

                //------------------ NameBasicsRecord ------------------
                foreach (var record in nameRecords)
                {
                    nameRecordNumber++;
                    Console.WriteLine($"Processing nameRecord {nameRecordNumber}...");

                    var person = new Person
                    {
                        Nconst = record.nconst,
                        PrimaryName = record.primaryName,
                        BirthYear = int.TryParse(record.birthYear, out int birthYear) ? new DateOnly(birthYear, 1, 1) : DateOnly.MinValue,
                        DeathYear = int.TryParse(record.deathYear, out int deathYear) ? new DateOnly(deathYear, 1, 1) : DateOnly.MinValue
                    };

                    var professions = record.primaryProfession.Split(',');

                    foreach (var professionName in professions)
                    {
                        // Look for the profession in the cache
                        if (!professionsCache.TryGetValue(professionName, out var profession))
                        {
                            // If the profession is not found in the cache, create a new instance
                            profession = new Profession { PrimaryProfession = professionName };
                            context.Professions.Add(profession);

                            // Add the new profession to the cache
                            professionsCache[professionName] = profession;
                        }

                        var personalCareer = new PersonalCareer
                        {
                            Person = person,
                            Profession = profession
                        };

                        context.PersonalCareers.Add(personalCareer);
                    }

                    var knownForTitles = record.knownForTitles.Split(',');

                    foreach (var tconst in knownForTitles)
                    {
                        // Look for the movie in the cache
                        if (moviesCache.TryGetValue(tconst, out var movie))
                        {
                            var blockBuster = new BlockBuster
                            {
                                Person = person,
                                MovieBase = movie
                            };

                            context.PersonalBlockbusters.Add(blockBuster);
                        }
                    }

                    // Save changes every batchSize records
                    if (nameRecordNumber % batchSize == 0)
                    {
                        context.SaveChanges();
                        Console.WriteLine($"Saved changes for {nameRecordNumber} nameRecords...");
                    }
                }


                //------------------ TitleBasicsRecord ------------------
                foreach (var record in titleRecords)
                {
                    titleRecordNumber++;
                    Console.WriteLine($"Processing titleRecord {titleRecordNumber}...");

                    var movieBase = new MovieBase
                    {
                        Tconst = record.tconst,
                        PrimaryTitle = record.primaryTitle,
                        OriginalTitle = record.originalTitle,
                        IsAdult = record.isAdult,
                        StartYear = int.TryParse(record.startYear, out int startYear) ? new DateOnly(startYear, 1, 1) : DateOnly.MinValue,
                        EndYear = int.TryParse(record.endYear, out int endYear) ? new DateOnly(endYear, 1, 1) : DateOnly.MinValue,
                        RuntimeMins = int.TryParse(record.runtimeMinutes, out int runtimeMins) ? runtimeMins : 0
                    };

                    var titleType = context.TitleTypes.Local.FirstOrDefault(tt => tt.Type == record.titleType);
                    if (titleType == null)
                    {
                        titleType = context.TitleTypes.FirstOrDefault(tt => tt.Type == record.titleType);
                    }

                    if (titleType == null)
                    {
                        titleType = new TitleType { Type = record.titleType };
                        context.TitleTypes.Add(titleType);
                    }

                    movieBase.TitleType = titleType;

                    var genres = record.genres.Split(',');

                    foreach (var genreType in genres)
                    {
                        // Look for the genre in the cache
                        if (!genresCache.TryGetValue(genreType, out var genre))
                        {
                            // If the genre is not found in the cache, create a new instance
                            genre = new Genre { GenreType = genreType };
                            context.Genres.Add(genre);

                            // Add the new genre to the cache
                            genresCache[genreType] = genre;
                        }

                        var movieGenre = new MovieGenre
                        {
                            MovieBase = movieBase,
                            Genre = genre
                        };

                        context.MovieGenres.Add(movieGenre);
                    }

                    // Save changes every batchSize records
                    if (titleRecordNumber % batchSize == 0)
                    {
                        context.SaveChanges();
                        Console.WriteLine($"Saved changes for {titleRecordNumber} titleRecords...");
                    }
                }
                // foreaches ends here
            }// using context ends here
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        sw.Stop();
        Console.WriteLine($"Total time elapsed: {sw.Elapsed}");

        Console.WriteLine("Ending program...");
    }

}



