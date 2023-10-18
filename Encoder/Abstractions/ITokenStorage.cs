namespace EncoderServer.Abstractions
{
    public interface ITokenStorage
    {
        Dictionary<string, CancellationTokenSource> ClientTokenSources { get; set; }
    }
}
