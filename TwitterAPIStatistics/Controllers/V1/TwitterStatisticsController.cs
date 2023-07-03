using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net;
using TwitterAPI.Controllers;

namespace TwitterAPIStatistics.Controllers.V1
{
    [Route("api/v1/twitter/statistics/")]
    [ApiController]
    public class TwitterStatisticsController : BaseController
    {
        private readonly ILogger<TwitterStatisticsController> logger;
        private readonly ITwitterStatisticsService twitterStatisticsService;

        public TwitterStatisticsController(ILogger<TwitterStatisticsController> logger, ITwitterStatisticsService twitterStatisticsService)
        {
            this.logger = logger;
            this.twitterStatisticsService = twitterStatisticsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TweetResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MessageStatusModel), (int)HttpStatusCode.NotFound)]
        public IActionResult GetTweetStatistics()
        {
            try
            {
                this.logger.LogDebug("Begin: GetTweetStatistics");
                var response = this.twitterStatisticsService.GetTweetStatistics();
                this.logger.LogDebug("End: GetTweetStatistics");
                
                return this.Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error: GetTweetStatistics");
                return this.ErrorResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
