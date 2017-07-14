using Microsoft.AspNet.SignalR;

namespace CTM.Codes.SignalR
{
    public class ViewDataUpdateHub : Hub
    {
        public void UpdateSearchResult()
        {
            Clients.Caller.updateSearchResult();
        }
    }
}