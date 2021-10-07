using System;
using System.ComponentModel.DataAnnotations;
using CWTest.Core.DataManipulation;

namespace DataAccess.Core.Abstractions
{
  public abstract class DataModelAbstractionBase : IDataModel
  {
    public abstract IDAbstraction IdAbstraction { get;}

    [MaxLength(100, ErrorMessage = "Label too long")]
    public virtual string Label { get; set; }

    public bool IsActive { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    public override string ToString()
    {
      return $"{Label}, {CreatedOn}, {UpdatedOn}, {IsActive}";
    }
  }
}