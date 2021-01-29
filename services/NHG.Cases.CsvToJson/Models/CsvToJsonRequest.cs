namespace NHG.Cases.CsvToJson.Models
{
    public class CsvToJsonRequest
    {
        public string RowsToSkip { get; set; }
        public string FileName { get; set; }
        public string Csv { get; set; }
    }
}
