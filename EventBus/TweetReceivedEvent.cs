using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus
{
    public class TweetReceivedEvent : TweetEvent
    {
        public TweetReceivedEvent(Tweet tweet)
        {
            this.Tweet = tweet;
        }

        private DateTime receivedAt = DateTime.UtcNow;
    }
}
