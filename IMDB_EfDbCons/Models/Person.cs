using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Models
{
    public class Person
    {
        [Key]
        [Name("nconst")]
        public string Nconst { get; set; }

        [Name("primaryName")]
        public string PrimaryName { get; set; }

        //[Name("birthYear")]
        [Ignore]
        public DateOnly? BirthYear { get; set; }

        //[Name("deathYear")]
        [Ignore]
        public DateOnly? DeathYear { get; set; }

        public ICollection<BlockBuster> BlockBusters { get; set; } = new List<BlockBuster>();
        public ICollection<PersonalCareer> PersonalCareers { get; set; } = new List<PersonalCareer>();
        public ICollection<Director> Directors { get; set; } = new List<Director>();
        public ICollection<Writer> Writers { get; set; } = new List<Writer>();
                      

        public Person() { }

        public override string ToString()
        {
            return $"{Nconst}, {PrimaryName}, {BirthYear}, {DeathYear}";
        }
    }
}
