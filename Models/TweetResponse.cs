namespace Models
{
    public class TweetResponse
    {
        public double TotalTweetsCount { get; set; }

        public double AverageTweetsPerMinute { get; set; }

        public MessageStatusModel? ErrorModel { get; set; }
    }
}