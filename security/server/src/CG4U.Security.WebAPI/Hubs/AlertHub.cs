using System.Threading.Tasks;
using CG4U.Security.Domain.Configuration;
using Microsoft.AspNetCore.SignalR;

namespace CG4U.Security.WebAPI.Hubs
{
    public class AlertHub : Hub
    {
        private IHubContext<AlertHub> _context;

        public AlertHub(IHubContext<AlertHub> context)
        {
            _context = context;
        }

        public async Task SendAlert(Alert alert)
        {
            await _context.Clients.All.SendAsync("ReceiveAlert", alert.TypeCode, alert.Message);
            //await Clients.All.SendAsync("ReceiveAlert", alert.TypeCode, alert.Message);
        }
    }
}
