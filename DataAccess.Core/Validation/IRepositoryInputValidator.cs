using System;
using System.Collections.Generic;
using CWTest.Core.DataManipulation;

namespace DataAccess.Core.Validation
{
  /// <summary>
  /// Validates the inputs for the standard database repos
  /// </summary>
  public interface IRepositoryInputValidator
  {
    bool ValidateGetAllParams(DateTime createdAfter, DateTime? createdBefore = null);

    bool ValidateGenericId<TU>(TU id);

    bool ValidateIntId(int id);

    bool ValidateGuidId(Guid id);

    bool ValidateStringId(string id);

    bool ValidateValue<T>(T value);

    bool ValidateLabel(string label);

    bool ValidateInputList<T>(IEnumerable<T> records);

    bool ValidatePagingParams(PagingParameters pagingParameters);
  }
}