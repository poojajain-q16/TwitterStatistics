using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using System;
using System.Net;
using TwitterAPIStatistics.Controllers.V1;
using Xunit;

namespace ApiTest
{
    public class TwitterStatisticsControllerTests
    {
        private readonly Mock<ILogger<TwitterStatisticsController>> mockLogger;
        private readonly Mock<ITwitterStatisticsService> mockTwitterStatisticsService;
        private readonly TwitterStatisticsController controller;

        public TwitterStatisticsControllerTests()
        {
            mockLogger = new Mock<ILogger<TwitterStatisticsController>>();
            mockTwitterStatisticsService = new Mock<ITwitterStatisticsService>();
            controller = new TwitterStatisticsController(mockLogger.Object, mockTwitterStatisticsService.Object);
        }

        [Fact]
        public void GetTweetStatistics_ShouldReturnOkResultWithResponse()
        {
            // Arrange
            var tweetResponse = new TweetResponse();
            mockTwitterStatisticsService.Setup(service => service.GetTweetStatistics()).Returns(tweetResponse);

            // Act
            var result = controller.GetTweetStatistics();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(tweetResponse, okResult.Value);
        }

        [Fact]
        public void GetTweetStatistics_ShouldReturnErrorResponse_WhenExceptionThrown()
        {
            // Arrange
            var errorMessage = "An error occurred.";
            var exception = new Exception(errorMessage);
            mockTwitterStatisticsService.Setup(service => service.GetTweetStatistics()).Throws(exception);

            // Act
            var result = controller.GetTweetStatistics();

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
            var errorResponse = (MessageStatusModel)objectResult.Value;
            Assert.Equal(errorMessage, errorResponse.Description);
        }
    }
}