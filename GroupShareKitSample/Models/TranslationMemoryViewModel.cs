using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sdl.Community.GroupShareKit.Models.Response.TranslationMemory;

namespace GroupShareKitSample.Models
{
    public class TranslationMemoryViewModel
    {
        public List<TranslationMemory> TranslationMemories { get; set; }
        public List<Filter> SearchResult { get; set; }
        public List<LanguageDirection> LanguageDirections { get; set; }
    }
}