using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Insertions
{
    public class CsvLoader
    {
        public List<T> LoadCsv<T>(string filePath, int numberOfLines)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "\t" }))
            {
                // Limit the number of lines by using Take(numberOfLines)
                return csv.GetRecords<T>().Take(numberOfLines).ToList();
            }
        }
    }
}
