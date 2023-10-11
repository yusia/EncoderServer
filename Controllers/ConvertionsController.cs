using Microsoft.AspNetCore.Mvc;

namespace EncoderServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertionsController : ControllerBase
    {
        [HttpGet("base")]
        public async IAsyncEnumerable<char> GetBase64Value([FromQuery] string text)
        {
            var textBytes = System.Text.Encoding.UTF8.GetBytes(text);

            var values = Convert.ToBase64String(textBytes);
            foreach (var item in values)
            {
                await Task.Delay(1000);
                yield return item;
            }
        }
    }

}
