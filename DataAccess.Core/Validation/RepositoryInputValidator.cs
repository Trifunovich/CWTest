using System;
using System.Collections.Generic;
using System.Linq;
using CWTest.Core.DataManipulation;
using LoggingLibrary;
using Microsoft.Extensions.Logging;

namespace DataAccess.Core.Validation
{
  internal class RepositoryInputValidator : IRepositoryInputValidator
  {
    private readonly IBasicLogger<RepositoryInputValidator> _logger;

    public RepositoryInputValidator(IBasicLogger<RepositoryInputValidator> logger)
    {
      _logger = logger;
    }

    public bool ValidateGetAllParams(DateTime createdAfter, DateTime? createdBefore = null)
    {
      if (createdAfter <= new DateTime(2000, 1, 1))
      {
        _logger?.LogError("createdAfter param with value {0} is invalid", createdAfter);
        return false;
      }

      if (createdBefore != null && createdBefore < createdAfter)
      {
        _logger?.LogError("createdBefore param with value {0} is invalid", createdBefore);
        return false;
      }

      return true;
    }

    public bool ValidateGenericId<U>(U id)
    {
      switch (id)
      {
        case int i:
          return ValidateIntId(i);
        case string s:
          return ValidateStringId(s);
        case Guid g:
          return ValidateGuidId(g);
      }

      return false;
    }

    public bool ValidateIntId(int id)
    {
      if (id <= 0)
      {
        _logger?.LogError("IdAbstraction with value {id} not found", id);
        return false;
      }

      return true;
    }

    public bool ValidateGuidId(Guid id)
    {
      if (id == Guid.Empty)
      {
        _logger?.LogError("Guid id with incorrect value {id}", id);
        return false;
      }

      return true;
    }

    public bool ValidateStringId(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        _logger?.LogError("String id with incorrect value {id}", id);
        return false;
      }

      return true;
    }

    public bool ValidateValue<T>(T value)
    {
      if (value == null)
      {
        _logger?.LogError("Element not found");
        return false;
      }

      return true;
    }

    public bool ValidateLabel(string label)
    {
      if (label is null || label.Length <= 100)
      {
        return true;
      }

      _logger?.LogError("Label invalid");
      return false;
    }

    public bool ValidateInputList<T>(IEnumerable<T> records)
    {
      if (records == null || !records.Any())
      {
        _logger?.LogError("Nothing retrieved");
        return false;
      }

      return true;
    }

    public bool ValidatePagingParams(PagingParameters pagingParameters)
    {
      bool infPage = Math.Abs(pagingParameters.Page) == int.MaxValue;
      bool infPageSize = Math.Abs(pagingParameters.PageSize) == int.MaxValue;
      bool infOffset = Math.Abs(pagingParameters.Offset) == int.MaxValue;

      if (infPage || infPageSize || infOffset)
      {
        _logger?.LogError("Parameters cannot be infinite");
        return false;
      }

      bool offsetVald = Math.Abs(pagingParameters.Offset) < Math.Abs(pagingParameters.PageSize);
      bool pageSizeValid = pagingParameters.PageSize > 0;
      bool pagePositionValid = pagingParameters.FirstElementPosition >= 0;

      if (!(offsetVald && pagePositionValid && pageSizeValid))
      {
        _logger?.LogError("One of the paging params is invalid. The values are - PagePosition: {0}, PageSize: {1}", pagingParameters.FirstElementPosition, pagingParameters.PageSize);
        return false;
      }

      return true;
    }
  }
}