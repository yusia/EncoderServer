using EncoderServer.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EncoderServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertionsController : ControllerBase
    {
        private IConvertion ConvertService { get; }
        public ConvertionsController(IConvertion convertService)
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
