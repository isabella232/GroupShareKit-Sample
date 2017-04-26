using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupShareKitSample.Models
{
    public class Attributes
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Delete { get; set; }
        public List<Types> Value { get; set; }
    }
    public class Types
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}