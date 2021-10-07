
using Microsoft.Extensions.Logging;

namespace LoggingLibrary
{
  /// <summary>
  /// This is used only for abstract classes, to pass the logger from the inherited classes to the constructor of an abstract class, it doesn't have the context
  /// - the generic parameter, so it shouldn't be used in regular classes
  /// 
  /// works kinda the same as Microsoft ILogger
  /// </summary>
  public interface IBasicLoggerAbstract : ILogger
  {

  }

  /// <summary>
  /// Adapter for the logger, it is currently mimicking the basic ILogger from MS, but we might change it down the line, so it's better to implement this interface into our classes, for the future sake
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IBasicLogger<T> : ILogger<T>, IBasicLoggerAbstract
  {

  }
}