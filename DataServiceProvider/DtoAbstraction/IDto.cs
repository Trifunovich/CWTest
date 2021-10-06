using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceProvider.Core.DtoAbstraction
{
  public interface IdAbstraction
  {

  }

  /// <summary>
  /// Different database providers use different identifiers
  /// </summary>
  /// <typeparam name="T">The identifier, int, guid, long and similar</typeparam>
  public interface IdAbstraction<T> : IdAbstraction
  {
    T Value { get; }

    string ValueAsString { get; }
  }

  /// <summary>
  /// Data transfer object abstraction
  /// </summary>
  /// <typeparam name="TIdAbs">Id type</typeparam>
  public interface IDto<TIdAbs>
  {
    IdAbstraction<TIdAbs> Id { get;}
    string Label { get; set; }
    bool IsActive { get; set; }
    DateTime CreatedOn { get; set; }
    DateTime UpdatedOn { get; set; }
  }
}
