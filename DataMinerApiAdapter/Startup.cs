using DataMinerApiAdapter.Services;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DataMinerApiAdapter.Startup))]
namespace DataMinerApiAdapter
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            var _ = DataMinerAdapterService.Instance; // Make sure the subscriptions are set up, should be replaced by DI
        }
    }
}