using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupShareKitSample.Models
{
    public class TranslationMemory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<LanguageDirection> LanguageDirections { get; set; }
    }
}