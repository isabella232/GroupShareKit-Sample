using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Akavache;
using GroupShareKitSample.Models;
using Microsoft.AspNet.Identity;
using Sdl.Community.GroupShareKit;
using Sdl.Community.GroupShareKit.Http;

namespace GroupShareKitSample.Helper
{
    public static class HelperMethods
    {
        private static User _authenticatedUser;
        public static string GetToken(IPrincipal user )
        {
            return ((ClaimsIdentity)user.Identity).FindFirst("Token").Value;
        }

        public static async Task<GroupShareClient> GetCurrentGsClient(string token, IPrincipal user)
        {
            if (token != string.Empty)
            {
                var userId = user.Identity.GetUserId();
               
                var authenticatedUser = await BlobCache.InMemory.GetObject<User>(userId);

                var credentials = new Credentials(token, authenticatedUser.UserName, authenticatedUser.Password);
                var inMemoryCredentials = new InMemoryCredentialStore(credentials);
                var baseAddress = new Uri(GetGsBaseAdress(user));
                var groupShareClient = new GroupShareClient(inMemoryCredentials, baseAddress);

                return groupShareClient;
            }
            return null;
        }

        public static string GetGsBaseAdress(IPrincipal user)
        {
            return  ((ClaimsIdentity)user.Identity).FindFirst("Gs").Value;
        }

        public static void SetAuthenticatedUser(User user)
        {
            _authenticatedUser = user;
        }

        public static User GetAuthenricatedUser()
        {
            return _authenticatedUser;
        }
    }
}