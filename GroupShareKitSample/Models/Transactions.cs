using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupShareKitSample.Models
{
    public class Transactions
    {
        public int? Id { get; set; }

        public DateTime? DateTime { get; set; }
      
        public string Username { get; set; }
       
        public TransactionsDetails Details { get; set; }
    }
}