namespace EncoderServer.Abstractions
{
    public interface IConvertion
    {
        IAsyncEnumerable<char> ToBase64Async(string text);
        void cancelConvertion();
    }
}
