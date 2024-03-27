using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.Records
{
    /// <summary>
    /// Dette er IKKE en Record klasse.
    /// Record navnet er valgt for at vise at det er en DTO klasse.
    /// Der er brug for en stærk-type klasse fremfor dynamisk, for at den
    /// kunne indeholde data fra NameBasics filen.
    /// 
    /// Record er generelt ideelt til DTO klasser, da det er en immutable klasse.
    /// </summary>
    public class NameBasicsRecord
    {
        public string nconst { get; set; }
        public string primaryName { get; set; }
        public string birthYear { get; set; }
        public string deathYear { get; set; }
        public string primaryProfession { get; set; }
        public string knownForTitles { get; set; }
    }
}
