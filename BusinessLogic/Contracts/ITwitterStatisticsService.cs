using Models;

namespace BusinessLogic.Contracts
{
    public interface ITwitterStatisticsService
    {
        TweetResponse GetTweetStatistics();
    }
}
