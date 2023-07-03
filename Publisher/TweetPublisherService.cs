using EventBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;

namespace TweetPublisher
{
    public class TweetPublisherService : BackgroundService
    {
        private readonly ILogger<TweetPublisherService> logger;
        private readonly TwitterApiCredential credentials;
        private readonly IEventBus eventBus;
        private readonly HttpClient httpClient;
        private const int MaxDelaySeconds = 240; // Max Delay of 4 minutes

        public TweetPublisherService(ILogger<TweetPublisherService> logger, IEventBus eventBus, IHttpClientFactory httpClientFactory, IOptions<TwitterApiCredential> credentialOptions)
        {
            this.logger = logger;
            this.eventBus = eventBus;
            this.httpClient = httpClientFactory.CreateClient();
            this.credentials = credentialOptions.Value;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", credentials.BearerToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int delay = 1;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var responseStream = await httpClient.GetStreamAsync(credentials.Url);
                    using var streamReader = new StreamReader(responseStream);
                    while (!stoppingToken.IsCancellationRequested && !streamReader.EndOfStream)
                    {
                        var line = await streamReader.ReadLineAsync();
                        var tweet = !string.IsNullOrEmpty(line) ? JsonConvert.DeserializeObject<Tweet>(line) : null;

                        if (tweet != null)
                        {
                            eventBus.Enqueue(new TweetReceivedEvent(tweet));
                        }
                        // Reset delay after successfull Request
                        delay = 1;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred in TweetPublisherService");
                    // Wait before retrying and double delay for next error
                    await Task.Delay(TimeSpan.FromSeconds(Math.Min(MaxDelaySeconds, delay)), stoppingToken);
                    delay *= 2;
                }
            }
        }
    }
}