using EncoderServer.Abstractions;
using System.Text;

namespace EncoderServer.Services
{
    public class ConvertionService : IConvertion
    {
        private CancellationTokenSource cancellationTokenSource;

        public IAsyncEnumerable<char> ToBase64Async(string text)
        {
            cancellationTokenSource = new CancellationTokenSource();
            return ToBase64Async(text, cancellationTokenSource.Token);
        }

        public async IAsyncEnumerable<char> ToBase64Async(string text, CancellationToken cancellationToken)
        {
            var values = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
            foreach (var item in values)
            {
                await RandomPause(cancellationToken);
                yield return item;
            }
            cancelConvertion();
        }

        public void cancelConvertion()
        {
            cancellationTokenSource.Cancel();
        }

        private async Task RandomPause(CancellationToken cancellationToken)
        {
            const int minPause = 1000, maxPause = 5000;
            var rnd = new Random();
            int rndTime = rnd.Next(minPause, maxPause);
            await Task.Delay(rndTime, cancellationToken);
        }
    }
}
