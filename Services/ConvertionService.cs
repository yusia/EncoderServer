using EncoderServer.Abstractions;
using System.Text;

namespace EncoderServer.Services
{
    public class ConvertionService : IConvertionService
    {
        private CancellationTokenSource cancellationTokenSource;

        public IAsyncEnumerable<char> ToBase64Async(string text)
        {
            cancellationTokenSource = new CancellationTokenSource();
            return ToBase64Async(text, cancellationTokenSource.Token);
        }

        public async IAsyncEnumerable<char> ToBase64Async(string text, CancellationToken cancellationToken)
        {
            var values = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(text));
            foreach (var item in values)
            {
                var randomTime = this.RandomPause();

                await Task.Delay(randomTime, cancellationToken);

                yield return item;
            }
        }

        public void cancelConvertion()
        {
            cancellationTokenSource.Cancel();
        }

        private int RandomPause()
        {
            const int minPause = 1000, maxPause = 5000;
            var rnd = new Random();
            return rnd.Next(minPause, maxPause);
        }
    }
}
