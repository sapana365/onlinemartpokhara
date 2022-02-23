[assembly: HostingStartup(typeof(onlinemartpokhara.Areas.Identity.IdentityHostingStartup))]

namespace onlinemartpokhara.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
    

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
