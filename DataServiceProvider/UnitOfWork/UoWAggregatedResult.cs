namespace DataServiceProvider.UnitOfWork
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
  }
}
