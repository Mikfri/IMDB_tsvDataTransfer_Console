using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IMDB_EfDbCons.Models
{
    [Keyless]
    public class BlockBuster
    {
        [ForeignKey("Person")]
        public string Nconst { get; set; }
        public Person Person { get; set; }


        [ForeignKey("MovieBase")]
        public string? Tconst { get; set; }
        public MovieBase? MovieBase { get; set; }

        

        public BlockBuster() { }

        public override string ToString()
        {
            return $"{Nconst}, {Tconst}";
        }
    }
}

///Hvis tabellen PersonalBlockbusters primært fungerer som en sammenkædningstabel (junction table)
///uden at have egne attributter ud over fremmednøglerne, kan det være en god idé at overveje at gøre den keyless.
///Dette er især relevant, hvis dataene i denne tabel primært repræsenterer relationer
///og ikke indeholder signifikante egenskaber, som du vil gemme.

