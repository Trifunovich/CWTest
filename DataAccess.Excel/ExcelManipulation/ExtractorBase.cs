using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Excel.Models;

namespace DataAccess.Excel.ExcelManipulation
{
    internal abstract class ExtractorBase : IExtractor
    {
        protected static int maxx = 1;
        protected static int count = 1;
        protected const string _separator = "\\";
        protected const string _separatorBack = "/";

        protected abstract string Extension
        {
            get;
        }

        protected static string GetRandomName()
        {
            return DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace(":", "_").Replace("/", "_").Replace(" ", "_");
        }

        protected virtual void RaiseEvent(WorkType type)
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            WorkEvent?.Invoke(this, new FinishedWorkArgs
            {
                WorkType = type
            });
        }


        protected virtual void RaiseEvent(string msg, int i)
        {
            Console.WriteLine(msg);
            // Raise the event in a thread-safe manner using the ?. operator.
            StatusEvent?.Invoke(this, new WorkStatusArgs
            {
                Status = msg,
                Index = i,
                Max = maxx
            });
        }

        protected virtual void RaiseRowEvent(IExcelDataModel row)
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            RowFinished?.Invoke(this, new FinishedRowArgs
            {
                Row = row
            });
        }

        protected virtual void RaiseDocEvent(WorkType wt, bool oneBigFile)
        {

            // Raise the event in a thread-safe manner using the ?. operator.
            DocumentFinished?.Invoke(this, new DocumentReadArgs()
            {
                WorkType = wt,
                OneBigDoc = oneBigFile
            });
        }

        public virtual void Export(ref string outputFolder, IList<IExcelDataModel> list, CancellationToken token,
          Dictionary<string, bool> whatToExport)
        {
            outputFolder = outputFolder.Trim();
            Directory.CreateDirectory(outputFolder);
        }

        protected List<string> GetForbiddenColumns(Dictionary<string, bool> whatToExport)
        {
            return whatToExport.Where(x => !x.Value).Select(x => x.Key).ToList();
        }

   

        public abstract Task<Dictionary<string, IList<IExcelDataModel>>> Import(string fileTPath, CancellationToken token);

        public event EventHandler<DocumentReadArgs> DocumentFinished;
        public event EventHandler<FinishedWorkArgs> WorkEvent;
        public event EventHandler<FinishedRowArgs> RowFinished;
        public event EventHandler<WorkStatusArgs> StatusEvent;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
