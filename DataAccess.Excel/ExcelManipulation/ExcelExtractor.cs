using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DataAccess.CWTest.Abstraction;
using DataAccess.Excel.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DataAccess.Excel.ExcelManipulation
{
    internal class ExcelExtractor : ExtractorBase
    {
        protected override string Extension => $".xlsx";

       
        public override Task<Dictionary<string, IList<IExcelDataModel>>> Import(string fileTPath, CancellationToken token)
        {
            var dict = new Dictionary<string, IList<IExcelDataModel>>();

            try
            {
                token.ThrowIfCancellationRequested();
                using XLWorkbook workbook = XLWorkbook.OpenFromTemplate(fileTPath);
                var sheets = workbook.Worksheets;
                int wsInd = -1;
                foreach (var sheet in sheets)
                {
                    wsInd++;

                    if (wsInd > 0)
                    {
                        //for now
                        continue;
                    }

                    //two rows are header rows
                    var rows = sheet.Rows().Skip(2).ToList();
                    var dataList = new List<IExcelDataModel>();
                    dict[sheet.Name] = dataList;

                    int i = 0;
                    foreach (var r in rows)
                    {
                        dataList.Add(ReadExcelRow(r, token, wsInd));
                        double percentage = i / maxx * 100;
                        RaiseEvent($"Row {i} out of {maxx} ({percentage:N1})%", i);
                    }
                }

                RaiseDocEvent(WorkType.Import, true);
            }
            catch (Exception e)
            {
                RaiseEvent(e.ToString(), 0);
            }
            
            return Task.FromResult(dict);
        }


        private IExcelDataModel ReadExcelRow(IXLRow row, CancellationToken token, int sheetInd)
        {
            token.ThrowIfCancellationRequested();
            string text;
            IExcelDataModel newModel = null;
            switch (sheetInd)
            {
                case 0:
                    {
                        var test = row.Cell(1)?.Value?.ToString();
                        if (string.IsNullOrWhiteSpace(test) || string.IsNullOrEmpty(test))
                        {
                            break;
                        }

                        ComponentSpecification model = new ComponentSpecification();
                        var cells = row.Cells(true).ToList();
                        foreach (var c in cells)
                        {
                            if (c == null || c.Value == null)
                            {
                                continue;
                            }

                            int j = cells.IndexOf(c);
                            token.ThrowIfCancellationRequested();


                            text = c.Value?.ToString();
                            switch (c.Address.ColumnNumber)
                            {
                                case 1:
                                    {
                                        model.InternalId = text?.Trim();
                                        break;
                                    }
                                case 2:
                                    {
                                        model.Label = text;
                                        break;
                                    }
                                case 3:
                                    {
                                        model.ParentId = text.Trim();
                                        break;
                                    }
                                case 4:
                                    {
                                        model.Assignment = text.TryToParseAssignmentType();
                                        break;
                                    }
                                case 5:
                                    {
                                        model.InitializationTest = text.TryToParseBoolValue();
                                        break;
                                    }
                                case 6:
                                    {
                                        model.SerialNumber = text;
                                        break;
                                    }
                            }
                        }

                        newModel = model;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return newModel;
        }
    }

    internal static class DataRowExtensions
    {
     
        public static int TryToParseIntValue(this string row)
        {
            int num;
            var parsedId = int.TryParse(row, out num);

            if (!parsedId)
            {
                num = -1;
            }

            return num;
        }

        public static bool TryToParseBoolValue(this string row)
        {
            bool val;
            var parsed = bool.TryParse(row, out val);

            if (!parsed)
            {
                val = false;
            }

            return val;
        }

        public static AssignmentType TryToParseAssignmentType(this string row)
        {
            string typeStr = row;

            if (string.IsNullOrEmpty(typeStr))
            {
                return AssignmentType.Unknown;
            }

            var parsed = Enum.TryParse(typeStr, true, out AssignmentType resType);
            return parsed ? resType : AssignmentType.Unknown;
        }

    }
}