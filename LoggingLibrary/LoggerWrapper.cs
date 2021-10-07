
using Microsoft.Extensions.Logging;
using System;

namespace LoggingLibrary
{
  internal class LoggerWrapper<T> : IBasicLogger<T>
  {   
    // we implement the standard microsoft logging here, but it can be changed, of course 
    private ILogger<T> _defaultLogger;

    public LoggerWrapper(ILogger<T> defaultLogger)
    {
      _defaultLogger = defaultLogger;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
      return _defaultLogger.BeginScope(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
      return _defaultLogger.IsEnabled(logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      _defaultLogger.Log(logLevel, eventId, state, exception, formatter);
    }
  }
}