using EncoderServer.Abstractions;

namespace EncoderServer.Services
{
    public class ConvertionService : IConvertion
    {
        public string ConvertToBase64(string text)
        {
            var textBytes = System.Text.Encoding.UTF8.GetBytes(text);

            return Convert.ToBase64String(textBytes);
        }

        public async IAsyncEnumerable<char> ToBase64Async(string text)
        {
            var values = ConvertToBase64(text);
            foreach (var item in values)
            {
                await RandomPause();
                yield return item;
            }
        }
        private async Task RandomPause()
        {
            const int minPause = 1000, maxPause = 5000;
            var rnd = new Random();
            int rndTime = rnd.Next(minPause, maxPause);
            await Task.Delay(rndTime);
        }
    }
}
