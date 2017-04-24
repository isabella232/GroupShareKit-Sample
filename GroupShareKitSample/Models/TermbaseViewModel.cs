using System;
using System.Collections.Generic;
using Sdl.Community.GroupShareKit.Models.Response;


namespace GroupShareKitSample.Models
{
    public class TermbaseViewModel
    {
        public SearchTerm Search { get; set; }
        public List<Termbase> Termbases { get; set; }
        public List<Term> SearchResult { get; set; }
    }
}