using Hangfire.PerformContextAccessor;
using Microsoft.Extensions.DependencyInjection;
using NLog.Common;
using NLog.Config;
using NLog.LayoutRenderers;
using System;
using System.Text;

namespace NLog.HangfireJobLogsTarget
{
    /// <summary>
    /// Rendering HangFire JobId
    /// </summary>
    /// <remarks>
    /// <code>${hangfire-jobid}</code>
    /// </remarks>
    [LayoutRenderer("hangfire-decorator")]
    public class JobDecoratorLayoutRenderer : LayoutRenderer
    {
        public static readonly string HANGFIRE_JOB_ID_PROPERTY_NAME = "hangfire-jobid";

        /// <summary>
        /// Context for DI
        /// </summary>
        private IPerformContextAccessor _performContextAccessor;

        /// <summary>
        /// Provides access to the current request PerformContext.
        /// </summary>
        /// <returns>HttpContextAccessor or <c>null</c></returns>
        [NLogConfigurationIgnoreProperty]
        public IPerformContextAccessor PerformContextAccessor
        {
            get => _performContextAccessor ?? (_performContextAccessor = RetrievePerformContextAccessor(ResolveService<IServiceProvider>(), LoggingConfiguration));
            set => _performContextAccessor = value;
        }

        internal static IPerformContextAccessor RetrievePerformContextAccessor(IServiceProvider serviceProvider, LoggingConfiguration loggingConfiguration)
        {
            IPerformContextAccessor performContextAccessor = serviceProvider.GetService<IPerformContextAccessor>();
            return performContextAccessor;
        }

        /// <summary>
        /// Validates that the PerformContext is available and delegates append to subclasses.<see cref="StringBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder" /> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var performContextAccessor = PerformContextAccessor;
            if (performContextAccessor == null)
            {
                return;
            }

            if (performContextAccessor.PerformingContext == null)
            {
                InternalLogger.Debug("No available PerformContext, because outside valid job context. Logger: {0}", logEvent.LoggerName);
                return;
            }

            DoAppend(builder, logEvent);
        }

        protected void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var id = PerformContextAccessor.PerformingContext.BackgroundJob.Id;

            if (!string.IsNullOrEmpty(id))
            {
                if (!logEvent.Properties.ContainsKey(HANGFIRE_JOB_ID_PROPERTY_NAME))
                {
                    logEvent.Properties.Add(HANGFIRE_JOB_ID_PROPERTY_NAME, id);
                }
            }
        }
    }
}