namespace DataServiceProvider.Core.UnitOfWork
{
  public class UoWAggregatedResult
  {
    /// <summary>
    /// Used for user interface GUI
    /// </summary>
    public UoWRegisterResult Result { get; }

    /// <summary>
    /// Used for deep logging, in order to get what happened
    /// </summary>
    public string ErrorDescription { get; }

    public UoWAggregatedResult(UoWRegisterResult result, string errorDescription)
    {
        Result = result;
        ErrorDescription = errorDescription;
    }
  }
}
