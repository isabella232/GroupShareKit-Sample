using GroupShareKitSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupShareKitSample.Helper
{
    public static class UserCache
    {
        public static void Insert(User user)
        {
            HttpRuntime.Cache.Insert(user.Id, user, null,
                       DateTime.Now.AddHours(11), TimeSpan.Zero);
        }
        public static void Remove(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }
        public static User Get(string cacheKey)
        {
            return HttpRuntime.Cache[cacheKey] as User;
            
        }
    }
}