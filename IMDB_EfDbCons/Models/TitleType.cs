using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Models
{
    public class TitleType
    {
        [Key]
        [Name("titleType")]
        public string Type { get; set; }

        public TitleType() { }

        public override string ToString()
        {
            return $"{Type}";
        }
    }

}
