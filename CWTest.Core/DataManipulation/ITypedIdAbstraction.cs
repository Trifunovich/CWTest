namespace CWTest.Core.DataManipulation
{
  /// <summary>
  /// Different database providers use different identifiers
  /// </summary>
  /// <typeparam name="T">The identifier, int, guid, long and similar</typeparam>
  public interface ITypedIdAbstraction<T> : IDAbstraction
  {
    T Value { get; }    
  }
}