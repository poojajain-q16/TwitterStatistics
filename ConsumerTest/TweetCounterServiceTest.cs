using System.Reflection;
using EventBus;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using TweetConsumer;
using Xunit;

namespace ConsumerTest;

public class TweetCounterServiceTest
{
    private readonly Mock<ILogger<TweetCounterService>> mockLogger;
    private readonly Mock<TwitterStatistics> mockTwitterStatistics;
    private readonly Mock<IEventBus> mockEventBus;
    private readonly TweetCounterService service;

    public TweetCounterServiceTest()
    {
        mockLogger = new Mock<ILogger<TweetCounterService>>();
        mockTwitterStatistics = new Mock<TwitterStatistics>();
        mockEventBus = new Mock<IEventBus>();
        service = new TweetCounterService(mockLogger.Object, mockTwitterStatistics.Object, mockEventBus.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldUpdateTweetCountAndAverageTweetsPerMinute()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        var tweet = new Tweet { Id = "1", Text = "Test tweet" };
        TweetEvent tweetEvent = new TweetReceivedEvent(tweet: tweet);
        mockEventBus.SetupSequence(bus => bus.TryDequeue(out tweetEvent))
            .Returns(true)
            .Returns(false);
        var twitterStatistics = new TwitterStatistics();
        var tweetsCountField = typeof(TwitterStatistics).GetField("TweetCount", BindingFlags.NonPublic | BindingFlags.Instance);
        tweetsCountField.SetValue(twitterStatistics, 5);
        twitterStatistics.AverageTweetsPerMinute = 10;

        // Act
        MethodInfo executeAsyncMethod = typeof(TweetCounterService)
            .GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)executeAsyncMethod.Invoke(service, new object[] { cancellationTokenSource.Token });

        // Assert
        mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), "Error occured in TweetCounterService."), Times.Never);
        mockTwitterStatistics.Verify(statistics => statistics.UpdateTweetCount(), Times.Once);
        mockTwitterStatistics.VerifySet(statistics => statistics.AverageTweetsPerMinute = It.IsAny<double>(), Times.Once);
        mockEventBus.Verify(bus => bus.TryDequeue(out tweetEvent), Times.Exactly(2));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogError_WhenExceptionOccurs()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        var exceptionMessage = "An error occurred.";
        var exception = new Exception(exceptionMessage);
        mockEventBus.Setup(bus => bus.TryDequeue(out It.Ref<TweetEvent>.IsAny))
            .Throws(exception);

        // Act
        MethodInfo executeAsyncMethod = typeof(TweetCounterService)
            .GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        await (Task)executeAsyncMethod.Invoke(service, new object[] { cancellationTokenSource.Token });

        // Assert
        mockLogger.Verify(
        logger => logger.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            exception,
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
        Times.Once);
    }
}
