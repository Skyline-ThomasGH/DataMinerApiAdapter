using Microsoft.AspNet.SignalR;

namespace DataMinerApiAdapter.Hubs
{
    public class JobHub : Hub
    {
        // No methods here.
        // This hub exists only so the server can push messages
        // via GlobalHost.ConnectionManager.GetHubContext<JobHub>().
    }
}