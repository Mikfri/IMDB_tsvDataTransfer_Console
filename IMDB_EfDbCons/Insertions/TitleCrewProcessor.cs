using IMDB_EfDbCons.Models;
using IMDB_EfDbCons.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Insertions
{
    public class TitleCrewProcessor
    {
        public static (List<MovieDirector>, List<MovieWriter>) ProcessTitleCrewRecords(List<TitleCrewRecord> titleCrewRecords)
        {
            var directors = new List<MovieDirector>();
            var writers = new List<MovieWriter>();

            foreach (var record in titleCrewRecords)
            {
                if (!string.IsNullOrEmpty(record.directors))
                {
                    var directorIds = record.directors.Split(',');
                    foreach (var directorId in directorIds)
                    {
                        var director = new MovieDirector { Tconst = record.tconst, Nconst = directorId };
                        directors.Add(director);
                    }
                }

                if (!string.IsNullOrEmpty(record.writers))
                {
                    var writerIds = record.writers.Split(',');
                    foreach (var writerId in writerIds)
                    {
                        var writer = new MovieWriter { Tconst = record.tconst, Nconst = writerId };
                        writers.Add(writer);
                    }
                }
            }

            return (directors, writers);
        }
    }
}
