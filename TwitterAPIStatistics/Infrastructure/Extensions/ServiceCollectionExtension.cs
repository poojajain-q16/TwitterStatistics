using BusinessLogic;
using BusinessLogic.Contracts;
using EventBus;
using Models;
using TweetConsumer;
using TweetPublisher;

namespace TwitterAPIStatistics.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddSingleton<TwitterStatistics>();
            services.AddSingleton<IEventBus, InMemoryEventBus>();
            services.AddScoped<TweetPublisherService>();
            services.AddScoped<TweetCounterService>();
            return services;
        }

        public static IServiceCollection AddBusinessLayerDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITwitterStatisticsService, TwitterStatisticsService>();
            return services;
        }
    }
}
