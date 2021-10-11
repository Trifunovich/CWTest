using Microsoft.Extensions.Configuration;

namespace DataAccess.Excel.Context
{
    public class ExcelContext : IExcelContext
    {
        public string ExcelPath { get; }

        public ExcelContext(IExcelConfiguration config)
        {
            ExcelPath = config.GetValue<string>("ExcelFilePath");
        }
    }
}