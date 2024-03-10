using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Models
{
    public class Profession
    {
        [Key]
        [Name("primaryProfession")]
        public string? PrimaryProfession { get; set; }
        
        public Profession() { }

        public override string ToString()
        {
            return $"{PrimaryProfession}";
        }
    }
}
