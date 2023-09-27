using Hangfire;
using Hangfire.Storage;
using NLog.Targets;
using System.Collections.Generic;

namespace NLog.HangfireJobLogsTarget
{
    [Target("HangfireJobLogs")]
    public sealed class HangfireJobLogsTarget : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = Layout.Render(logEvent);

            var jobStorageConnection = JobStorage.Current.GetConnection();

            // I have to find where to store messages
            if (logEvent.Properties.TryGetValue(JobDecoratorLayoutRenderer.HANGFIRE_JOB_ID_PROPERTY_NAME, out object jobIdObj))
            {
                var jobId = (string)jobIdObj;

                using (var tran = jobStorageConnection.CreateWriteTransaction())
                {
                    var keyValuePairs = new[] {
                        new KeyValuePair<string, string>($"{logEvent.SequenceID}-message", logMessage)
                    };

                    tran.SetRangeInHash($"joblogs-jobId:{jobId}", keyValuePairs);
                    (tran as JobStorageTransaction).ExpireHash($"joblogs-jobId:{jobId}", JobStorage.Current.JobExpirationTimeout);
                    tran.Commit();
                }
            }
        }
    }
}
