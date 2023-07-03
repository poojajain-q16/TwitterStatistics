using System;
using System.Reflection;
using BusinessLogic;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using Xunit;

namespace ApiTest
{
	public class TwitterStatisticsServiceTest
	{
        private readonly Mock<ILogger<TwitterStatisticsService>> mockLogger;
        private readonly Mock<TwitterStatistics> mockTwitterStatistics;
        private readonly TwitterStatisticsService service;

        public TwitterStatisticsServiceTest()
        {
            mockLogger = new Mock<ILogger<TwitterStatisticsService>>();
            mockTwitterStatistics = new Mock<TwitterStatistics>();
            service = new TwitterStatisticsService(mockLogger.Object, mockTwitterStatistics.Object);
        }

        [Fact]
        public void GetTweetStatistics_ShouldReturnTweetResponseWithStatistics()
        {
            // Arrange
            var twitterStatistics = new TwitterStatistics();
            var tweetsCountField = typeof(TwitterStatistics).GetField("TweetCount", BindingFlags.NonPublic | BindingFlags.Instance);
            tweetsCountField.SetValue(twitterStatistics, 10);
            twitterStatistics.AverageTweetsPerMinute = 5;

            var loggerMock = new Mock<ILogger<TwitterStatisticsService>>();
            var twitterStatisticsService = new TwitterStatisticsService(loggerMock.Object, twitterStatistics);

            // Act
            var result = twitterStatisticsService.GetTweetStatistics();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.TotalTweetsCount);
            Assert.Equal(twitterStatistics.AverageTweetsPerMinute, result.AverageTweetsPerMinute);

        }
    }
}

