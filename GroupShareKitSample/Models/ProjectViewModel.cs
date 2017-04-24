using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupShareKitSample.Models
{
    public class ProjectViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Template> Templates { get; set; }
        public List<Organization> Organizations { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
    }
}