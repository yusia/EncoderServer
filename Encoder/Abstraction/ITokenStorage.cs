namespace EncoderServer.Abstraction
{
    public interface ITokenStorage
    {
        Dictionary<string, CancellationTokenSource> ClientTokenSources { get; set; }
    }
}
