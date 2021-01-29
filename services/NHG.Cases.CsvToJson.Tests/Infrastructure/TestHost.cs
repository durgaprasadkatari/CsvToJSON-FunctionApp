using Microsoft.Extensions.Logging;
using NSubstitute;

namespace NHG.Cases.CsvToJson.Tests.Infrastructure
{
    public class TestHost
    {
        public TestHost()
        {
            FakeHttpMessageHandler = new FakeHttpMessageHandler();
            Logger = Substitute.For<ILogger>();

        }

        public FakeHttpMessageHandler FakeHttpMessageHandler { get; }

        public ILogger Logger { get; }
    }
}
