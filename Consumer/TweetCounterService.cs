using EventBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;

namespace TweetConsumer
{
    public class TweetCounterService : BackgroundService
    {
        private readonly ILogger<TweetCounterService> logger;
        private readonly TwitterStatistics twitterStatistics;
        private readonly IEventBus eventBus;
        private readonly DateTime startTime = DateTime.UtcNow;

        public TweetCounterService(ILogger<TweetCounterService> logger, TwitterStatistics twitterStatistics, IEventBus eventBus)
        {
            this.logger = logger;
            this.twitterStatistics = twitterStatistics;
            this.eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    while (eventBus.TryDequeue(out var @event))
                    {
                        if (@event is TweetReceivedEvent)
                        {
                            this.twitterStatistics.UpdateTweetCount();
                            this.twitterStatistics.AverageTweetsPerMinute = (this.twitterStatistics.GetTweetCount() / (DateTime.UtcNow - startTime).TotalMinutes);
                        }
                    }
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error occured in TweetCounterService.");
            }
        }
    }
}