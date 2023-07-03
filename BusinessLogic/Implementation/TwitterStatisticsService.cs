using BusinessLogic.Contracts;
using Microsoft.Extensions.Logging;
using Models;

namespace BusinessLogic
{
    public class TwitterStatisticsService : ITwitterStatisticsService
    {
        private readonly ILogger<TwitterStatisticsService> logger;
        private readonly TwitterStatistics twitterStatistics;
        public TwitterStatisticsService(ILogger<TwitterStatisticsService> logger, TwitterStatistics twitterStatistics)
        {
            this.logger = logger;
            this.twitterStatistics = twitterStatistics;
        }
        public TweetResponse GetTweetStatistics()
        {
            this.logger.LogDebug($"Begin: GetTweetStatistics");

            return new TweetResponse
            {
                TotalTweetsCount = this.twitterStatistics.GetTweetCount(),
                AverageTweetsPerMinute = this.twitterStatistics.AverageTweetsPerMinute
            };
        }
    }
}