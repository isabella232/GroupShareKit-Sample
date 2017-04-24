using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupShareKitSample.Models
{
    public class Project
    {
        public string ProjectId { get; set; }

        public string Name { get; set; }

        public string OrganizationName { get; set; }

        public string CustomerName { get; set; }

        public string OrganizationId { get; set; }

        public string SourceLanguage { get; set; }

        public string TargetLanguage { get; set; }

        public DateTime? DueDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CompletedAt { get; set; }


        public int Status { get; set; }
    }
}