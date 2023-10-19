using Encoder.ConversionService;
using Encoder.ConversionService.Abstraction;
using Encoder.ConversionService.Settings;
using Microsoft.Extensions.Options;

namespace Encoder.Tests.Unit
{
    public class ConvertionServiceTests
    {
        IConvertionService _service;

        [SetUp]
        public void Setup()
        {
            var settings = Options.Create(new DelaySettings() { Min = 1, Max = 2 });
            _service = new ConvertionService(settings);
        }


        [Test]
        [TestCase("Hello, World!", "SGVsbG8sIFdvcmxkIQ==")]
        [TestCase("", "")]
        public async Task ToBase64Async_InitText_ReturnsExpected(string initText, string expected)
        {
            //Arrange
            var actual = "";
            //Act
            await foreach (var symbol in _service.ToBase64Async(initText, default(CancellationToken)))
            {
                actual += symbol;
            }
            //Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ToBase64Async_ThrowsExceptionWithCancellation()
        {   //Arrange
            var cancelTokenSource = new CancellationTokenSource();
            //Act
            var enumerator = _service.ToBase64Async("init text", cancelTokenSource.Token).GetAsyncEnumerator();
            cancelTokenSource.Cancel();
            //Assert
            Assert.That(async () => await enumerator.MoveNextAsync(), Throws.TypeOf<OperationCanceledException>());
        }

    }
}