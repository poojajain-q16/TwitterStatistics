using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus
{
    public interface IEventBus
    {
        void Enqueue(TweetEvent tweetEvent);
        bool TryDequeue(out TweetEvent tweetEvent);
    }
}
