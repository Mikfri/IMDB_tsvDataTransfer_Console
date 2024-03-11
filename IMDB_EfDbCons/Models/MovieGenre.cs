using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Models
{
    [Keyless]
    public class MovieGenre
    {
        [ForeignKey("MovieBase")]
        public string Tconst { get; set; }
        public MovieBase MovieBase { get; set; }

        [ForeignKey("Genre")]
        public string GenreType { get; set; }
        public Genre Genre { get; set; }

        public MovieGenre() { }

        public override string ToString()
        {
            return $"{Tconst} - {GenreType}";
        }
    }
}
