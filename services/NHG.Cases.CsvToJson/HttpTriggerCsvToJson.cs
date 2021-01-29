using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NHG.Cases.CsvToJson.Models;

namespace NHG.Cases.CsvToJson
{
    public class HttpTriggerCsvToJson
    {
        private readonly char[] _fieldSeparator = { ',' };
        private long _lineSkipCounter;
       

        [FunctionName("HttpTriggerCSVToJSON")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] CsvToJsonRequest req, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function CSVToJSON processed a request.");

            var fileName = req.FileName;
            var rowsToSkipStr = req.RowsToSkip;
            string errorMessage;

            if (rowsToSkipStr == null)
            {
                errorMessage = "Please pass a rowsToSkip on the query string or in the request body";
                log.LogInformation($"BadRequest: {errorMessage}");
                return new ContentResult
                {
                    StatusCode = 500,
                    Content = errorMessage
                };
            }

            int.TryParse(rowsToSkipStr, out var rowsToSkip);

            if (fileName == null)
            {
                errorMessage = "Please pass a fileName on the query string or in the request body";
                log.LogInformation($"BadRequest: {errorMessage}");
                return new BadRequestObjectResult(errorMessage);
            }

            var csvData = req.Csv;

            if (csvData == null)
            {
                errorMessage = "Please pass the csv data using the csv attribute in the request body";
                log.LogInformation($"BadRequest: {errorMessage}");
                return new BadRequestObjectResult(errorMessage);
            }

            var csvLines = await Task.Run(() => ToLines(csvData));

            log.LogInformation($"Found {csvLines.Count()} lines in file {fileName}");

            var headers = csvLines[0].Split(_fieldSeparator).ToList();

            var resultSet = new CsvToJsonResult(fileName);


            foreach (var line in csvLines.Skip(rowsToSkip))
            {
                //Check to see if a line is blank.
                //This can happen on the last row if improperly terminated.
                if (line != "" || line.Trim().Length > 0)
                {
                    var lineObject = new JObject();
                    var fields = line.Split(_fieldSeparator);

                    for (var x = 0; x < headers.Count; x++)
                    {
                        lineObject[headers[x]] = fields[x];
                    }

                    resultSet.Rows.Add(lineObject);
                }
                else
                {
                    _lineSkipCounter += 1;
                }
            }

            log.LogInformation($"{_lineSkipCounter} lines skipped, not including the header row.");

            return new OkObjectResult(resultSet);
        }

        private static string[] ToLines(string dataIn)
        {
            var eolMarkerR = new[] { '\r' };
            var eolMarkerN = new[] { '\n' };
            var eolMarker = eolMarkerR;

            //check to see if the file has both \n and \r for end of line markers.
            //common for files coming from Unix\Linux systems.
            if (dataIn.IndexOf('\n') > 0 && dataIn.IndexOf('\r') > 0)
            {
                //if we find both just remove one of them.
                dataIn = dataIn.Replace("\n", "");
            }
            //If the file only has \n then we will use that as the EOL marker to separate the lines.
            else if (dataIn.IndexOf('\n') > 0)
            {
                eolMarker = eolMarkerN;
            }

            //How do we know the dynamic data will have Split capability?
            return dataIn.Split(eolMarker);
        }
    }
}
