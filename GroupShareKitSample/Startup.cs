using Akavache;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GroupShareKitSample.Startup))]
namespace GroupShareKitSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            BlobCache.ApplicationName = "GroupShareClient";
            ConfigureAuth(app);
        }
    }
}
