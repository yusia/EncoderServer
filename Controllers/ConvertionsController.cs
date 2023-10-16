using EncoderServer.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EncoderServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertionsController : ControllerBase
    {
        private IConvertionService ConvertService { get; }
        public ConvertionsController(IConvertionService convertService)
        {
            this.ConvertService = convertService;
        }

        [HttpGet("base")]
        public IAsyncEnumerable<char> GetBase64Value([FromQuery] string text)
        {
            return ConvertService.ToBase64Async(text);
        }

        [HttpGet("cancel")]
        public void Cancel()
        {
            ConvertService.cancelConvertion();
        }
    }

}
