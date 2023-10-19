using Encoder.Server.Constants;
using Microsoft.AspNetCore.SignalR;

namespace EncoderServer.Infrastructure
{
    public class SignalRHub : Hub
    {
        public async Task GetConnectionId()
        {
            await Clients.Clients(this.Context.ConnectionId).SendAsync(ConvertionMessages.ConnectionId, this.Context.ConnectionId);
        }
    }
}
