namespace Encoder.ConversionService.Abstraction
{
    public interface IConvertionService
    {
        IAsyncEnumerable<char> ToBase64Async(string text, CancellationToken cancellationToken);
    }
}
