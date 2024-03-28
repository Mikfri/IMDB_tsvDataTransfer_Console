using IMDB_EfDbCons.Records;
using IMDbLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Insertions
{
    public class NameBasicsProcessor
    {
        /// <summary>
        /// Metodenavn: ProcessNameBasicsRecords
        /// Tager imod en liste af NameBasicsRecord og returnerer
        /// 4 lister: Person, Profession, PersonalCareer og KnownForTitle
        /// </summary>
        /// <param name="nameRecords"></param>
        /// <returns></returns>
        public static (List<Person>, HashSet<Profession>, List<PersonalCareer>, List<KnownForTitle>) ProcessNameBasicsRecords(List<NameBasicsRecord> nameRecords)
        {
            var persons = new List<Person>();
            var professions = new Dictionary<string, Profession>();
            var personalCareers = new List<PersonalCareer>();
            var knownForTitles = new List<KnownForTitle>();

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
                    var blockBuster = new KnownForTitle { Nconst = record.nconst, Tconst = tconst };
                    knownForTitles.Add(blockBuster);
                }
            }

            //----------------------- DateTime Converter
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

            return (persons, new HashSet<Profession>(professions.Values), personalCareers, knownForTitles);
        }
    }
}
