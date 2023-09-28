# NLog.HangfireJobLogsTarget

[![Latest version](https://img.shields.io/nuget/v/Bonura.NLog.HangfireJobLogsTarget.svg)](https://www.nuget.org/packages?q=Bonura.NLog.HangfireJobLogsTarget)

NLog target to send log messages to Hangfire storage

Installation
-------------

NLog.HangfireJobLogsTarget is available as a NuGet package. You can install it using the NuGet Package Console window:

```
PM> Install-Package Bonura.NLog.HangfireJobLogsTarget
```

**First install and configure package: `Hangfire.PerformContextAccessor`**.

See: https://github.com/meriturva/Hangfire.PerformContextAccessor

After installation, update your NLog settings:

```json
"NLog": {
  "extensions": [
    {
      "assembly": "NLog.HangfireJobLogsTarget"
    }
  ]
```

Layouts
-------------
To store log message with jobId information and so make storage works correctly is mandatory to add a decorator to message layout.
* `hangfire-decorator` -> add hangfire jobid to NLog log event properties (mandatory)

Use
-------------
A simple target configuration


```json
"targets": {
  "hangfire": {
     "layout": "${longdate}|${level:uppercase=true}|${logger}|${message}|${exception:format=toString}${hangfire-decorator}",
     "type": "HangfireJobLogs"
  }
}
```
