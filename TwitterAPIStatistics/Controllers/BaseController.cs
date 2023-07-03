using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace TwitterAPI.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        protected ActionResult ErrorResponse(string message, HttpStatusCode code)
        {
            MessageStatusModel messageStatusModel = new() { ResponseCode = code.ToString(), Description = message };
            return this.StatusCode((int)code, messageStatusModel);
        }
    }
}

