using System;

namespace DataServiceProvider.UnitOfWork
{
  /// <summary>
  /// Configuration for paged loading
  /// </summary>
  public class PagingParameters
  {
    /// <summary>
    /// From which element index paging starts
    /// </summary>
    public int FirstElementPosition { get; }

    /// <summary>
    /// From which page the paging is starting
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// The needed page size
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// How many should it be skipped
    /// </summary>
    public int Offset { get; set; }

    public PagingParameters(int page, int pageSize = 10, int offset = 0)
    {
      bool infPage = Math.Abs(Page) == int.MaxValue;
      bool infPageSize = Math.Abs(PageSize) == int.MaxValue;
      bool infOffset = Math.Abs(Offset) == int.MaxValue;

      if (infPage || infPageSize || infOffset)
      {
        return;
      }
      else
      {
        Page = page;
        PageSize = pageSize;
        Offset = offset;
        FirstElementPosition = (page - 1) * PageSize + offset;
      }
    }
  }
}
