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
        private IConvertionService _convertService { get; }
        private readonly IHubContext<SignalRHub> _hub;
        private readonly ITokenStorage _toketStorage;
        public ConvertionsController(IConvertionService convertService, IHubContext<SignalRHub> hub,
            ITokenStorage toketStorage)
        {
            this._convertService = convertService;
            this._hub = hub;
            this._toketStorage = toketStorage;
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
            if (!_toketStorage.ClientTokenSources.TryAdd(clientId, source))
            {
                _toketStorage.ClientTokenSources[clientId] = source;
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
            var source = _toketStorage.ClientTokenSources[clientId];

            source.Cancel();
            _toketStorage.ClientTokenSources.Remove(clientId);

            return Ok(new { Message = "convertion cancelled" });
        }

        /// <summary>
        /// Send convertion result via webSocket connection
        /// </summary>
        /// <returns></returns>
        private async Task startConvertion(string initText, string clientId, CancellationToken token)
        {
            try
            {
                await foreach (var symbol in _convertService.ToBase64Async(initText, token))
                {
                    await this._hub.Clients.Clients(clientId).SendAsync(ConvertionMessages.ConvertedLetter, symbol, token);
                }
            }
            catch (TaskCanceledException)
            {
                await this._hub.Clients.Clients(clientId).SendAsync(ConvertionMessages.Cancelled, token);
                _toketStorage.ClientTokenSources.Remove(clientId);
            }
            await this._hub.Clients.Clients(clientId).SendAsync(ConvertionMessages.Finished, token);
            _toketStorage.ClientTokenSources.Remove(clientId);
        }
    }

}
