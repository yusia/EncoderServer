using EncoderServer.Abstractions;
using EncoderServer.Services;

namespace Encoder.Tests.Unit
{
    public class ConvertionServiceTests
    {
        IConvertionService service;

        [SetUp]
        public void Setup()
        {
            service = new ConvertionService();
        }


        [Test]
        [TestCase("Hello, World!", "SGVsbG8sIFdvcmxkIQ==")]
        [TestCase("", "")]
        public async Task ToBase64Async_InitText_ReturnsExpected(string initText, string expected)
        {
            var actual = "";

            await foreach (var symbol in service.ToBase64Async(initText, default(CancellationToken)))
            {
                actual += symbol;
            }

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ToBase64Async_ThrowsExceptionWithCancellation()
        {
            var cancelTokenSource = new CancellationTokenSource();
            var enumerator = service.ToBase64Async("init text", cancelTokenSource.Token).GetAsyncEnumerator();
            cancelTokenSource.Cancel();

            Assert.That(async () => await enumerator.MoveNextAsync(), Throws.TypeOf<TaskCanceledException>());
        }

    }
}