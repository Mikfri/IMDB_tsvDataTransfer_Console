using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Models
{
    public class MovieDirector
    {
        [Key]
        [Name("tconst")]
        public string Tconst { get; set; }
        public MovieBase MovieBase { get; set; }

        [ForeignKey("Person")]
        [Name("directors")]
        public string? Nconst { get; set; }
        public Person? Person { get; set; }
    }    
}
