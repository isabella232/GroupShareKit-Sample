using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupShareKitSample.Models
{
    public class TermbaseLanguages
    {
        public Language Language { get; set; }
        public List<TermbaseTerms> Terms { get; set; }
        public List<Attributes> Attributes { get; set; }
    }
}