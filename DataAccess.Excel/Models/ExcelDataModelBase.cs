using CWTest.Core.DataManipulation;
using DataAccess.Core.Abstractions;

namespace DataAccess.Excel.Models
{
    internal abstract class ExcelDataModelBase : DataModelAbstractionBase, IExcelDataModel
    {
        public override IDAbstraction IdAbstraction => null;
        public override void SetId(IDAbstraction idAbstraction)
        {
            //not needed in excel there are none
            throw new System.NotImplementedException();
        }
    }
}
