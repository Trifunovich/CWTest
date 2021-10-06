using Serilog.Core;
using Serilog.Events;

namespace LoggingLibrary.Enrichers
{
  internal abstract class LogFilePathEnricherBase : ILogEventEnricher
  {
    private string _cachedLogFilePath;
    private LogEventProperty _cachedLogFilePathProperty;
    protected static string LogFilePath = "testpath.txt";

    public const string LogFilePathPropertyName = "LogFilePath";

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
      LogEventProperty logFilePathProperty;

      if (LogFilePath.Equals(_cachedLogFilePath))
      {
        // Path hasn't changed, so let's use the cached property
        logFilePathProperty = _cachedLogFilePathProperty;
      }
      else
      {
        // We've got a new path for the log. Let's create a new property
        // and cache it for future log events to use
        _cachedLogFilePath = LogFilePath;

        _cachedLogFilePathProperty = logFilePathProperty =
          propertyFactory.CreateProperty(LogFilePathPropertyName, LogFilePath);
      }

      logEvent.AddPropertyIfAbsent(logFilePathProperty);
    }

    public static void SetPath(string path)
    {
      LogFilePath = path;
    }
  }
}