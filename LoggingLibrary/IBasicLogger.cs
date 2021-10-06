
using Microsoft.Extensions.Logging;

namespace LoggingLibrary
{
  /// <summary>
  /// Adapter for the logger, it is currently mimicking the basic ILogger from MS, but we might change it down the line, so it's better to implement this interface into our classes, for the future sake
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IBasicLogger<T> : ILogger<T>
  {

  }
}