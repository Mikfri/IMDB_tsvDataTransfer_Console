using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Models
{
    public class Genre
    {
        [Key]
        [Name("genres")]    // Name attribute is used to map the column name in the CSV file to the property name in the class
        public string GenreType { get; set; }

        public Genre() { }

        public override string ToString()
        {
            return $"{GenreType}";
        }
    }
}
