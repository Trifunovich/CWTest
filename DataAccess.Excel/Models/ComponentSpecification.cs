
using DataAccess.CWTest.Abstraction;

namespace DataAccess.Excel.Models
{
  internal class ComponentSpecification : ExcelDataModelBase, IComponentSpecification
  {
      public int ParentId { get; set; }
      public AssignmentType Assignment { get; set; }
      public bool InitializationTest { get; set; }
      public string SerialNumber { get; set; }
      public bool HasSerialNumber => !string.IsNullOrEmpty(SerialNumber);
  }
}
