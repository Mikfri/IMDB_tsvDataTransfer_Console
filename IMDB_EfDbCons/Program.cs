using IMDB_EfDbCons.DataContext;
using IMDB_EfDbCons.Insertions;
using IMDB_EfDbCons.Models;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        string nameBasicsTsv = @"C:\Users\mikkf\OneDrive\Dokumenter\Visual Studio 2022\TSVs\name.basics.tsv\data.tsv";
        Console.WriteLine("Starting program...");

        try
        {
            using (var context = new IMDb_Context())
            {
                Console.WriteLine("Created context...");

                var loader = new CsvLoader();
                Console.WriteLine("Created CSV loader...");

                // Load the data into instances of the Record class
                var records = loader.LoadCsv<Record>(nameBasicsTsv, 1000);
                Console.WriteLine($"Loaded {records.Count} records from TSV file...");

                // Process each record
                foreach (var record in records)
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
                        var profession = context.Professions.Local.FirstOrDefault(p => p.PrimaryProfession == professionName);

                        if (profession == null)
                        {
                            profession = context.Professions.FirstOrDefault(p => p.PrimaryProfession == professionName);
                        }

                        if (profession == null)
                        {
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

                context.SaveChanges();
                Console.WriteLine("Saved changes to context...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        Console.WriteLine("Ending program...");
    }

}



