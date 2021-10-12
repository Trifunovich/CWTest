
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.CWTest.Abstraction;

namespace DataAccess.Sql.Models
{
  [Core.Attributes.Table("ComponentSpecification")]
  public class ComponentSpecification : SqlDataModelBase, IComponentSpecification
  {
      public int ParentId { get; set; }
      public AssignmentType Assignment { get; set; }
      public bool InitializationTest { get; set; }
      public string SerialNumber { get; set; }

      [NotMapped]
      public bool HasSerialNumber => !string.IsNullOrEmpty(SerialNumber);
  }
}
