using Xunit;

namespace Seq.Api.Tests
{
    public class SeqConnectionTests
    {
        [Fact]
        public void WhenConstructedTheHandlerConfigurationCallbackIsCalled()
        {
            var callCount = 0;

            using var _ = new SeqConnection("https://test.example.com", handler => { 
                Assert.NotNull(handler);
                ++callCount;
            }, null);

            Assert.Equal(1, callCount);
        }
    }
}
