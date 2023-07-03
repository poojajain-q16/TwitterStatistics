using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TwitterStatistics
    {
        private long TweetCount = 0;

        public double AverageTweetsPerMinute { get; set; }

        public void UpdateTweetCount()
        {
            Interlocked.Increment(ref TweetCount);
        }

        public long GetTweetCount()
        {
            return TweetCount;
        }

    }
}
