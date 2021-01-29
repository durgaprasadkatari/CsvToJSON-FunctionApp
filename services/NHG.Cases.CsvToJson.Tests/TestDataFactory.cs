using NHG.Cases.CsvToJson.Models;

namespace NHG.Cases.CsvToJson.Tests
{
    public class TestDataFactory
    {
        public static CsvToJsonRequest CreateCsvToJsonRequest()
        {
            return new CsvToJsonRequest()
            {
                RowsToSkip = "1",
                FileName = "TestAccountCases.csv",
                Csv = @"ID,Name,Score
1,Aaron,99
2,Dave,55
3,Susy,77"
            };
        }

        public static CsvToJsonRequest CreateCsvToJsonInvalidRequest()
        {
            return new CsvToJsonRequest()
            {
                FileName = "TestAccountCases.csv",
                Csv = @"ID,Name,Score
1,Aaron,99
2,Dave,55
3,Susy,77"
            };
        }
    }
}
