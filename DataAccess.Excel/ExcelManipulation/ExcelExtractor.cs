using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Excel.Models;
using ExcelDataReader;

namespace DataAccess.Excel.ExcelManipulation
{
    internal class ExcelExtractor : ExtractorBase
    {
        protected override string Extension => $".xlsx";

        public override Task<Dictionary<string, IList<IExcelDataModel>>> Import(string fileTPath, CancellationToken token)
        {
            using var stream = File.Open(fileTPath, FileMode.Open, FileAccess.Read);
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            if (result != null)
            {
                var dict = new Dictionary<string, IList<IExcelDataModel>>();
            }

            return null;
        }
    }
}