using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Excel.Models;

namespace DataAccess.Excel.ExcelManipulation
{
    public enum WorkType
    {
        Import,
        Export
    }

    public class FinishedWorkArgs
    {
        public WorkType WorkType { get; set; } = WorkType.Import;
    }

    internal class FinishedRowArgs
    {
        public IExcelDataModel Row { get; set; }
    }

    public class WorkStatusArgs
    {
        public int Index { get; set; } = 1;
        public int Max { get; set; } = 1;
        public string Status { get; set; } = "Empty status";
    }

    public class DocumentReadArgs
    {
        public WorkType WorkType { get; set; } = WorkType.Import;

        public bool OneBigDoc { get; set; }
    }

    internal interface IExtractor : IDisposable
    {
        void Export(ref string outputFolder, IList<IExcelDataModel> list, CancellationToken token, Dictionary<string, bool> whatToExport);

        Task<Dictionary<string, IList<IExcelDataModel>>> Import(string fileTPath, CancellationToken token);

        event EventHandler<DocumentReadArgs> DocumentFinished;
        event EventHandler<FinishedWorkArgs> WorkEvent;
        event EventHandler<FinishedRowArgs> RowFinished;
        event EventHandler<WorkStatusArgs> StatusEvent;
    }
}
