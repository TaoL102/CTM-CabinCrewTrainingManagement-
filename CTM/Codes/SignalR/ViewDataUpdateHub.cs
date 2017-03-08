using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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