using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YourProject.Startup))]
namespace YourProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
