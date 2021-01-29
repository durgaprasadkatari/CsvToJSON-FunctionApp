using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NHG.Cases.CsvToJson.Tests.Infrastructure
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public string Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = StatusCode,
                Content = new StringContent(Response)
            });
        }
    }
}
