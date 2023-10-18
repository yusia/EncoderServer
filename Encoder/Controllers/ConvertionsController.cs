using EncoderServer.Abstractions;
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
        /// Convertes received text to base64 format
        /// </summary>
        /// <param name="text"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("converted-base64-text")]
        public IAsyncEnumerable<char> GetBase64Value([FromQuery] string text, CancellationToken token = default(CancellationToken))
        {
            var convertedText = convertService.ToBase64Async(text, token);
            return convertedText;
        }

        /// <summary>
        /// Start convertion
        /// </summary>
        /// <param name="text"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("convert")]
        public async Task<IActionResult> Convert([FromQuery] string clientId, [FromQuery] string text)
        {

            var source = new CancellationTokenSource();
            if (!toketStorage.ClientTokenSources.TryAdd(clientId, source))
            {
                toketStorage.ClientTokenSources[clientId] = source;
            }

            startConvertion(text, clientId, source.Token);
            return Ok(new { Message = "please wait converted text" });
        }

        /// <summary>
        /// Cancel convertion
        /// </summary>
        /// <param name="text"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("cancel")]
        public async Task<IActionResult> cancel([FromQuery] string clientId)
        {
            var source = toketStorage.ClientTokenSources[clientId];

            source.Cancel();
            toketStorage.ClientTokenSources.Remove(clientId);

            return Ok(new { Message = "convertion cancelled" });
        }

        private async void startConvertion(string initText, string clientId, CancellationToken token)
        {
            try
            {

                await foreach (var symbol in convertService.ToBase64Async(initText, token))
                {
                    this.hub.Clients.Clients(clientId).SendAsync("converted_letter_received", symbol, token);
                }
            }
            catch (Exception ex)
            {//todo canceled
                this.hub.Clients.Clients(clientId).SendAsync("convertion_finished", token);
                toketStorage.ClientTokenSources.Remove(clientId);
            }
            this.hub.Clients.Clients(clientId).SendAsync("convertion_finished", token);
            toketStorage.ClientTokenSources.Remove(clientId);
        }
    }

}
