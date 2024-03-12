using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Models
{
    public class Director
    {
        [ForeignKey("Person")]
        [Name("nconst")]
        public string Nconst { get; set; }
        public Person Person { get; set; }

        [ForeignKey("MovieBase")]
        [Name("tconst")]
        public string Tconst { get; set; }
        public MovieBase MovieBase { get; set; }

        public Director() { }

        public override string ToString()
        {
            return $"{Nconst} - {Tconst}";
        }
    }
}
