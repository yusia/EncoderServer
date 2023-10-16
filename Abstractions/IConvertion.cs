namespace EncoderServer.Abstractions
{
    public interface IConvertionService
    {
        IAsyncEnumerable<char> ToBase64Async(string text);
        IAsyncEnumerable<char> ToBase64Async(string text, CancellationToken cancellationToken);
        void cancelConvertion();
    }
}
