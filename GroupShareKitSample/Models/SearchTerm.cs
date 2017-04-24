using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GroupShareKitSample.Models
{
    public class SearchTerm
    {
        public string SearchedTerm { get; set; }
        public string SourceLanguage   { get; set; }
        public string TargetLanguage { get; set; }
        public  string TermbaseId{ get; set; }
    }
}