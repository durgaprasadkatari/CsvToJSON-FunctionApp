using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NHG.Cases.CsvToJson.Models;
using NHG.Cases.CsvToJson.Tests.Infrastructure;
using Xunit;

namespace NHG.Cases.CsvToJson.Tests
{
    public class CsvToJsonTest
    {
        private readonly ILogger _logger;
        private readonly HttpTriggerCsvToJson _sut;
        private readonly CsvToJsonRequest _csvToJsonRequest = TestDataFactory.CreateCsvToJsonRequest();
        private readonly CsvToJsonRequest _csvToJsonInvalidRequest = TestDataFactory.CreateCsvToJsonInvalidRequest();
        private FakeHttpMessageHandler _httpMessageHandler;

        public CsvToJsonTest()
        {
            var testHost = new TestHost();
            _httpMessageHandler = testHost.FakeHttpMessageHandler;
            _logger =  testHost.Logger;
            _sut = new HttpTriggerCsvToJson();
        }

        [Fact]
        public async Task CSV_To_JSON_ForValidRequest()
        {
            var response = (OkObjectResult)await _sut.Run(_csvToJsonRequest, _logger);
            var jsonResult = (CsvToJsonResult)response.Value;
            var json = JsonConvert.SerializeObject(jsonResult);
            Assert.Equal(SetValidParsingResponse(), json);
        }

        [Fact]
        public async Task CSV_To_JSON_ForNotValidRequest()
        {
            var result = (ContentResult)await _sut.Run(_csvToJsonInvalidRequest, _logger);
            Assert.True(result.StatusCode.HasValue && result.StatusCode == (int)HttpStatusCode.InternalServerError);
        }

        private static string SetValidParsingResponse()
        {
            return "{\"FileName\":\"TestAccountCases.csv\",\"Rows\":[{\"ID\":\"1\",\"Name\":\"Aaron\",\"Score\":\"99\"},{\"ID\":\"2\",\"Name\":\"Dave\",\"Score\":\"55\"},{\"ID\":\"3\",\"Name\":\"Susy\",\"Score\":\"77\"}]}";
        }
    }
}
