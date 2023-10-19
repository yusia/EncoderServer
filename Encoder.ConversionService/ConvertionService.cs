using Encoder.ConversionService.Abstraction;
using Encoder.ConversionService.Settings;
using Microsoft.Extensions.Options;
using System.Text;

namespace Encoder.ConversionService
{
    public class ConvertionService : IConvertionService
    {
        private readonly DelaySettings _delaySettings;
        public ConvertionService(IOptions<DelaySettings> delaySettings)
        {
            _delaySettings = delaySettings.Value;
        }

        /// <summary>
        /// Convertes received text to base64 forma with immulation long running operation
        /// </summary>
        /// <param name="text"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<char> ToBase64Async(string text, CancellationToken cancellationToken)
        {
            var values = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(text));
            foreach (var item in values)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var randomTime = this.RandomPause();
                await Task.Delay(randomTime, cancellationToken);
                yield return item;
            }
        }

        /// <summary>
        /// Generates random time period from 1 to 5 sec
        /// </summary>
        /// <returns></returns>
        private int RandomPause()
        {
            var rnd = new Random();
            var rndTime = rnd.Next(_delaySettings.Min, _delaySettings.Max); ;
            return rndTime;
        }
    }
}
