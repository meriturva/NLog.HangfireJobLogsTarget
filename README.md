# NLog.HangfireLayouts

[![Latest version](https://img.shields.io/nuget/v/Bonura.NLog.HangfireLayouts.svg)](https://www.nuget.org/packages?q=Bonura.NLog.HangfireLayouts)

Few hangfire layouts to use with NLog logging library

Installation
-------------

NLog.HangfireLayouts is available as a NuGet package. You can install it using the NuGet Package Console window:

```
PM> Install-Package Bonura.NLog.HangfireLayouts
```

**First install and configure package: `Hangfire.PerformContextAccessor`**.

See: https://github.com/meriturva/Hangfire.PerformContextAccessor

After installation, update your NLog settings:

```json
"NLog": {
  "extensions": [
    {
      "assembly": "NLog.HangfireLayouts"
    }
  ]
```

Layouts
-------------
* `hangfire-jobid` -> hangfire job id

Use
-------------
A simple message layout with jobid


```json
"targets": {
  "console": {
    "layout": "${longdate}|${level:uppercase=true}|${logger}|${message}|${hangfire-jobid}|${exception:format=toString}",
    "type": "ColoredConsole"
  }
}
```
