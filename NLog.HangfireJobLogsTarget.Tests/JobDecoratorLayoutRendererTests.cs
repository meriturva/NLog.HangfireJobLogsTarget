using Hangfire.PerformContextAccessor;
using NLog.HangfireJobLogsTarget.Tests.Mocks;
using Xunit;

namespace NLog.HangfireJobLogsTarget.Tests
{
    public class JobDecoratorLayoutRendererTests
    {
        private readonly PerformContextMock _context;
        private readonly PerformContextAccessor _performContextAccessor;

        public JobDecoratorLayoutRendererTests()
        {
            _context = new PerformContextMock();
            _performContextAccessor = new PerformContextAccessor();
        }

        [Fact]
        public void SuccessTest()
        {
            // Arrange
            _performContextAccessor.PerformingContext = _context.Object;
            var renderer = new JobDecoratorLayoutRenderer();
            renderer.PerformContextAccessor = _performContextAccessor;
            // Act
            var logEvent = new LogEventInfo();
            string result = renderer.Render(logEvent);
            // Assert
            Assert.Equal("", result);
            Assert.Equal(1, logEvent.Properties.Count);
            Assert.Equal("JobId", logEvent.Properties["hangfire-jobid"]);
        }

        [Fact]
        public void EmptyTest()
        {
            // Arrange
            var renderer = new JobDecoratorLayoutRenderer();
            renderer.PerformContextAccessor = _performContextAccessor;
            // Act
            var logEvent = new LogEventInfo();
            string result = renderer.Render(logEvent);
            // Assert
            Assert.Equal("", result);
            Assert.Equal(0, logEvent.Properties.Count);
        }

        [Fact]
        public void NoPerformContextAccessorTest()
        {
            // Arrange
            var renderer = new JobDecoratorLayoutRenderer();
            renderer.PerformContextAccessor = null;
            // Act
            var logEvent = new LogEventInfo();
            string result = renderer.Render(logEvent);
            // Assert
            Assert.Equal("", result);
            Assert.Equal(0, logEvent.Properties.Count);
        }
    }
}
