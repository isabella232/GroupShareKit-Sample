using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Sdl.Community.GroupShareKit;
using Sdl.Community.GroupShareKit.Clients;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Akavache;
using GroupShareKitSample.Helper;

namespace GroupShareKitSample.Models
{
    public class UserStore : IUserStore<User>,IUserLoginStore<User>, IUserSecurityStampStore<User>, IUserPasswordStore<User>
    {
        private static User _authenticatedUser;
        private GroupShareClient _gsClient;

        public void Dispose()
        {
        }

        public UserStore()
        {
            //BlobCache.ApplicationName = "GroupShareClient";

        }
        //public UserStore(GroupShareClient client)
        //{
            
        //    _gsClient = client;
        //}

        public Task CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            // return Task.Factory.StartNew(() => _authenticatedUser);
            throw new NotImplementedException();
        }

        public Task<User> FindByNameAsync(string userName)
        {

            var user = Task<User>.Factory.StartNew(() => new User
            {
                UserName = userName,
                PasswordHash = "testPassword",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid(),
                Password = "hashPassword"
            });
            HelperMethods.SetAuthenticatedUser(user.Result);
            return user;
        }

        public  async Task<User> FindAsync(string userName, string password,GroupShareClient gsClient)
        {

            var gsUser = await gsClient.User.Get(new UserRequest(userName));
            var user = new User
            {
                UserId = gsUser.UniqueId,
                UserName = gsUser.Name,
                DisplayName = gsUser.DisplayName,
                Password = password,
                Token = gsClient.Credentials.Token
            };
            UserCache.Insert(user);
            return user;

        }

        public User GetAuthenticatedUser()
        {
            return _authenticatedUser;
            
        }

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }
    }
}