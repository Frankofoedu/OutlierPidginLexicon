using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outliers.Models
{
    class Word
    {
        public int Id { get; set; }
        public string dWord { get; set; }
        public string Synonyms { get; set; }
        public string Definitions { get; set; }
        public string Examples { get; set; }

    }

    class VaderClassification
    {
        public string Word { get; set; }
        public int? Value { get; set; }
    }
}