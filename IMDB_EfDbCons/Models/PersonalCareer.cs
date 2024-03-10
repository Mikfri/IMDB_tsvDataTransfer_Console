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
    public class PersonalCareer
    {
        [ForeignKey("Person")]
        public string Nconst { get; set; }
        public Person Person { get; set; }


        [ForeignKey("Profession")]
        public string? PrimProf { get; set; }
        public Profession Profession { get; set; }


        public PersonalCareer() { }

    }
}
