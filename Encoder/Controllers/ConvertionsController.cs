using EncoderServer.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EncoderServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertionsController : ControllerBase
    {
        private IConvertionService convertService { get; }
        public ConvertionsController(IConvertionService convertService)
        {
            this.convertService = convertService;
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
    }

}
