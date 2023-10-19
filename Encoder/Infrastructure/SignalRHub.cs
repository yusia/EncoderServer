using Encoder.Server.Constants;
using Microsoft.AspNetCore.SignalR;

namespace EncoderServer.Infrastructure
{
    public class SignalRHub : Hub
    {
        /// <summary>
        /// Sends connectionId to current client
        /// </summary>
        /// <param name="message">Chat message model</param>
        public async Task GetConnectionId()
        {
            var connectionId = this.Context.ConnectionId;
            await Clients.Clients(connectionId).SendAsync(ConvertionMessages.ConnectionId, connectionId);
        }
    }
}
