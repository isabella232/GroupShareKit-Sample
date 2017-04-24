using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace GroupShareKitSample.Models
{
    public class User: IUser
    {
        public Guid UserId { get; set; }
      
        public string Id => UserId.ToString();

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string DisplayName { get; set; }
        public string GsLink { get; set; }
    }
}