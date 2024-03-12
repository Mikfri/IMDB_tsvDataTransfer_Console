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
        Stopwatch sw = Stopwatch.StartNew();

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

                // Process NameBasicsRecords and create Persons, Professions, PersonalCareers, and PersonalBlockbusters
                var (persons, professions, personalCareers, personalBlockbusters) = ProcessNameBasicsRecords(nameRecords);

                // Process TitleBasicsRecords and create MovieBases, TitleTypes, Genres, and MovieGenres
                var (movieBases, titleTypes, genres, movieGenres) = ProcessTitleBasicsRecords(titleRecords);

                // Use BulkInsert to insert the records for each table in bulk
                context.BulkInsert(persons);
                context.BulkInsert(professions);
                context.BulkInsert(personalCareers);
                context.BulkInsert(personalBlockbusters);
                context.BulkInsert(movieBases);
                context.BulkInsert(titleTypes);
                context.BulkInsert(genres);
                context.BulkInsert(movieGenres);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        sw.Stop();
        Console.WriteLine($"Total time elapsed: {sw.Elapsed}");

        Console.WriteLine("Ending program...");

        static DateOnly? TryParseDate(string dateValue)
        {
            if (dateValue.Equals("\\N", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            // Hvis datoen er i formatet "yyyy-mm-dd"
            if (DateTime.TryParseExact(dateValue, "yyyy", null, System.Globalization.DateTimeStyles.None, out var dateTime))
            {
                return new DateOnly(dateTime.Year, 1, 1);
            }

            Console.WriteLine($"Fejl ved konvertering af dato: {dateValue}");

            return null;
        }

        static (List<Person>, HashSet<Profession>, List<PersonalCareer>, List<BlockBuster>) ProcessNameBasicsRecords(List<NameBasicsRecord> nameRecords)
        {
            var persons = new List<Person>();
            var professions = new Dictionary<string, Profession>();
            var personalCareers = new List<PersonalCareer>();
            var personalBlockbusters = new List<BlockBuster>();

            foreach (var record in nameRecords)
            {
                var person = new Person
                {
                    Nconst = record.nconst,
                    PrimaryName = record.primaryName,
                    BirthYear = TryParseDate(record.birthYear),
                    DeathYear = TryParseDate(record.deathYear)
                };
                persons.Add(person);

                if (!string.IsNullOrEmpty(record.primaryProfession))
                {
                    var professionTypes = record.primaryProfession.Split(',');
                    foreach (var professionType in professionTypes)
                    {
                        if (!professions.ContainsKey(professionType))
                        {
                            var profession = new Profession { PrimaryProfession = professionType };
                            professions.Add(professionType, profession);
                        }

                        var personalCareer = new PersonalCareer { Nconst = record.nconst, PrimProf = professionType };
                        personalCareers.Add(personalCareer);
                    }
                }

                var tconsts = record.knownForTitles.Split(',');
                foreach (var tconst in tconsts)
                {
                    var blockBuster = new BlockBuster { Nconst = record.nconst, Tconst = tconst };
                    personalBlockbusters.Add(blockBuster);
                }
            }

            return (persons, new HashSet<Profession>(professions.Values), personalCareers, personalBlockbusters);
        }


        static (List<MovieBase>, List<TitleType>, List<Genre>, List<MovieGenre>) ProcessTitleBasicsRecords(List<TitleBasicsRecord> titleRecords)
        {
            var movieBases = new List<MovieBase>();
            var titleTypes = new Dictionary<string, TitleType>();
            var genres = new Dictionary<string, Genre>();
            var movieGenres = new List<MovieGenre>();

            foreach (var record in titleRecords)
            {
                var movieBase = new MovieBase
                {
                    Tconst = record.tconst,
                    TitleType = new TitleType { Type = record.titleType },
                    PrimaryTitle = record.primaryTitle,
                    OriginalTitle = record.originalTitle,
                    IsAdult = record.isAdult,
                    StartYear = TryParseDate(record.startYear),
                    EndYear = TryParseDate(record.endYear),
                    RuntimeMins = int.TryParse(record.runtimeMinutes, out var runtime) ? runtime : (int?)null
                };
                movieBases.Add(movieBase);

                if (!titleTypes.ContainsKey(record.titleType))
                {
                    var titleType = new TitleType { Type = record.titleType };
                    titleTypes.Add(record.titleType, titleType);
                }

                var genreTypes = record.genres.Split(',');
                foreach (var genreType in genreTypes)
                {
                    if (!genres.ContainsKey(genreType))
                    {
                        var genre = new Genre { GenreType = genreType };
                        genres.Add(genreType, genre);
                    }

                    var movieGenre = new MovieGenre { Tconst = record.tconst, GenreType = genreType };
                    movieGenres.Add(movieGenre);
                }
            }

            return (movieBases, new List<TitleType>(titleTypes.Values), new List<Genre>(genres.Values), movieGenres);
        }
    }




}



