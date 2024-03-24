using IMDB_EfDbCons.Records;
using IMDbLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Insertions
{
    public class TitleBasicsProcessor
    {
        public static (List<MovieBase>, List<TitleType>, List<Genre>, List<MovieGenre>) ProcessTitleBasicsRecords(List<TitleBasicsRecord> titleRecords)
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

            return (movieBases, new List<TitleType>(titleTypes.Values), new List<Genre>(genres.Values), movieGenres);
        }
    }
}
