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
                await Task.Delay(1000);
                yield return item;
            }
        }
    }
}
