using EncoderServer.Abstractions;

namespace EncoderServer.Infrastructure
{
    public class CancellationTokenStorage : ITokenStorage
    {
        public Dictionary<string, CancellationTokenSource> ClientTokenSources { get; set; }
        public CancellationTokenStorage()
        {
            ClientTokenSources = new Dictionary<string, CancellationTokenSource>();
        }
    }
}
