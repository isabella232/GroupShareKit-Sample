using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using GroupShareKitSample.Models;
using Microsoft.AspNet.Identity;
using Sdl.Community.GroupShareKit;
using Sdl.Community.GroupShareKit.Clients;

namespace GroupShareKitSample.Models
{
    public class UserManagerKit: UserManager<User>
    {
        private static GroupShareClient _gsClient;
  
        public UserManagerKit(IUserStore<User> store) : base(store)
        {
        }

        public  async Task<GroupShareClient> GetGroupShareClient(Account account)
        {
            var uri = new Uri(account.GsLink);
            
            var token = await GroupShareClient.GetRequestToken(account.UserName, account.Password, uri, GroupShareClient.AllScopes);
            _gsClient = await GroupShareClient.AuthenticateClient(token,account.UserName, account.Password, uri, GroupShareClient.AllScopes);
            return _gsClient;
        }

        public override async Task<User> FindAsync(string userName, string password)
        {

            var gsUser = await _gsClient.User.Get(new UserRequest(userName));
            var user = new User
            {
                UserId = gsUser.UniqueId,
                UserName = gsUser.Name,
                DisplayName = gsUser.DisplayName,
                Password = password,
                Token = _gsClient.Credentials.Token
            };
            return user;


        }

        public override Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
        {
            var claims = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role);
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id, "http://www.w3.org/2001/XMLSchema#string"));
            claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
            claims.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Custom Identity", "http://www.w3.org/2001/XMLSchema#string"));
            claims.AddClaim(new Claim("Token",user.Token));
           // claims.AddClaim(new Claim("Guid", user.Id));
            claims.AddClaim(new Claim("Gs", _gsClient.BaseAddress.ToString()));

            return Task.Factory.StartNew(()=>claims);
        }

     
    }

   
}