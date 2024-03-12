using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Models
{
    public class Writer
    {
        [ForeignKey("Person")]
        public string Nconst { get; set; }
        public Person Person { get; set; }

        [ForeignKey("MovieBase")]
        public string Tconst { get; set; }
        public MovieBase MovieBase { get; set; }

        public Writer() { }

        public override string ToString()
        {
            return $"{Nconst} - {Tconst}";
        }
    }
}
