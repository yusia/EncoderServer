using Microsoft.AspNetCore.SignalR;

namespace EncoderServer.Infrastructure
{
    public class SignalRHub : Hub
    {
        public async Task join()
        {
            await Clients.Clients(this.Context.ConnectionId).SendAsync("client_joined", this.Context.ConnectionId);
        }
    }
}
