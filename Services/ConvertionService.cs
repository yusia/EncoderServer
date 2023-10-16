using EncoderServer.Abstractions;
using System.Text;

namespace EncoderServer.Services
{
    public class ConvertionService : IConvertionService
    {

        public async IAsyncEnumerable<char> ToBase64Async(string text, CancellationToken cancellationToken)
        {
            var values = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(text));
            foreach (var item in values)
            {
                var randomTime = this.RandomPause();

                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(randomTime, cancellationToken);
                yield return item;
            }
        }

        private int RandomPause()
        {
            const int minPause = 1000, maxPause = 5000;
            var rnd = new Random();
            return rnd.Next(minPause, maxPause);
        }
    }
}
