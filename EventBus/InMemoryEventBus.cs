using System.Collections.Concurrent;

namespace EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ConcurrentQueue<TweetEvent> _queue = new();

        void IEventBus.Enqueue(TweetEvent tweetEvent)
        {
            _queue.Enqueue(tweetEvent);
        }

        bool IEventBus.TryDequeue(out TweetEvent tweetEvent)
        {
            return _queue.TryDequeue(out tweetEvent);
        }
    }
}