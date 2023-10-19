using Encoder.ConversionService.Abstraction;
using Encoder.Server.Constants;
using EncoderServer.Abstraction;
using EncoderServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EncoderServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertionsController : ControllerBase
    {
        private IConvertionService convertService { get; }
        private readonly IHubContext<SignalRHub> hub;
        private readonly ITokenStorage toketStorage;
        public ConvertionsController(IConvertionService convertService, IHubContext<SignalRHub> hub,
            ITokenStorage toketStorage)
        {
            this.convertService = convertService;
            this.hub = hub;
            this.toketStorage = toketStorage;
        }

        /// <summary>
        /// Start convertion
        /// </summary>
        /// <param name="text"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("convert")]
        public IActionResult Convert([FromQuery] string clientId, [FromQuery] string text)
        {

            var source = new CancellationTokenSource();
            if (!toketStorage.ClientTokenSources.TryAdd(clientId, source))
            {
                toketStorage.ClientTokenSources[clientId] = source;
            }

            _ = startConvertion(text, clientId, source.Token);
            return Ok(new { Message = "please wait converted text" });
        }

        /// <summary>
        /// Cancel convertion
        /// </summary>
        /// <param name="text"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("cancel")]
        public IActionResult cancel([FromQuery] string clientId)
        {
            var source = toketStorage.ClientTokenSources[clientId];

            source.Cancel();
            toketStorage.ClientTokenSources.Remove(clientId);

            return Ok(new { Message = "convertion cancelled" });
        }

        private async Task startConvertion(string initText, string clientId, CancellationToken token)
        {
            try
            {
                await foreach (var symbol in convertService.ToBase64Async(initText, token))
                {
                    await this.hub.Clients.Clients(clientId).SendAsync(ConvertionMessages.ConvertedLetter, symbol, token);
                }
            }
            catch (TaskCanceledException)
            {
                await this.hub.Clients.Clients(clientId).SendAsync(ConvertionMessages.Cancelled, token);
                toketStorage.ClientTokenSources.Remove(clientId);
            }
            await this.hub.Clients.Clients(clientId).SendAsync(ConvertionMessages.Finished, token);
            toketStorage.ClientTokenSources.Remove(clientId);
        }
    }

}
