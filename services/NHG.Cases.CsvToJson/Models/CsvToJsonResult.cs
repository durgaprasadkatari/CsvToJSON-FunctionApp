using System.Collections.Generic;

namespace NHG.Cases.CsvToJson.Models
{
    public class CsvToJsonResult
    {
        public CsvToJsonResult(string fileName)
        {
            Rows = new List<object>();
            FileName = fileName;
        }

        public string FileName { get; set; }
        public List<object> Rows { get; set; }
    }
}