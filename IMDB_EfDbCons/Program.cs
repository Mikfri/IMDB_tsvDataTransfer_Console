using IMDB_EfDbCons.DataContext;
using IMDB_EfDbCons.Insertions;
using IMDB_EfDbCons.Models;
using IMDB_EfDbCons.Records;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        string nameBasicsTsv = @"C:\Users\mikkf\OneDrive\Dokumenter\Visual Studio 2022\TSVs\name.basics.tsv\data.tsv";
        string titleBasicsTsv = @"C:\Users\mikkf\OneDrive\Dokumenter\Visual Studio 2022\TSVs\title.basics.tsv\data.tsv";
        

        Console.WriteLine("Starting program...");


        try
        {
            using (var context = new IMDb_Context())
            {
                Console.WriteLine("Created context...");

                var loader = new CsvLoader();
                Console.WriteLine("Created CSV loader...");

                // Load the data into instances of the Record class
                var nameRecords = loader.LoadCsv<NameBasicsRecord>(nameBasicsTsv, 1000);
                Console.WriteLine($"Loaded {nameRecords.Count} records from {nameRecords}TSV file...");

                //------------------ NameBasicsRecord ------------------
                foreach (var record in nameRecords)
                {
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
                        // Look for the profession in the local context
                        var profession = context.Professions.Local.FirstOrDefault(p => p.PrimaryProfession == professionName);

                        if (profession == null)
                        {
                            // Look for the profession in the database
                            profession = context.Professions.FirstOrDefault(p => p.PrimaryProfession == professionName);
                        }

                        if (profession == null)
                        {
                            // If the profession is not found, create a new instance
                            profession = new Profession { PrimaryProfession = professionName };
                            context.Professions.Add(profession);
                        }

                        var personalCareer = new PersonalCareer
                        {
                            Person = person,
                            Profession = profession
                        };

                        context.PersonalCareers.Add(personalCareer);
                    }
                }                


                var titleRecords = loader.LoadCsv<TitleBasicsRecord>(titleBasicsTsv, 1000);
                Console.WriteLine($"Loaded {titleRecords.Count} records from {titleRecords}TSV file...");


                //------------------ TitleBasicsRecord ------------------
                foreach (var record in titleRecords)
                {
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
                        // Look for the genre in the local context
                        var genre = context.Genres.Local.FirstOrDefault(g => g.GenreType == genreType);

                        if (genre == null)
                        {
                            // Look for the genre in the database
                            genre = context.Genres.FirstOrDefault(g => g.GenreType == genreType);
                        }

                        if (genre == null)
                        {
                            // If the genre is not found, create a new instance
                            genre = new Genre { GenreType = genreType };
                            context.Genres.Add(genre);
                        }

                        var movieGenre = new MovieGenre
                        {
                            MovieBase = movieBase,
                            Genre = genre
                        };

                        context.MovieGenres.Add(movieGenre);
                    }
                }// foreaches ends here

                context.SaveChanges();
                Console.WriteLine("Saved changes to context...");

            }// using context ends here
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        Console.WriteLine("Ending program...");
    }

}



