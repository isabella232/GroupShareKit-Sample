using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupShareKitSample.Models
{
    public class TermbaseTerms
    {
        public List<Attributes> Attributes { get; set; }
        public string Text { get; set; }
        public List<Transactions> Transactions { get; set; }
    }
}