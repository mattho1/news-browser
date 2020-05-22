using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace NewsBrowser.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscribeService _subscribeService;
        private readonly IEmailService _emailService;

        public SubscriberController(ISubscribeService subscribeService, IEmailService emailService)
        {
            _subscribeService = subscribeService;
            _emailService = emailService;
        }

        [HttpGet("subscribe", Name = "SubscribeQuery")]
        public IActionResult SubscribeQuery(string email, string subscribeQuery)
        {
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(subscribeQuery))
            {
                _subscribeService.AddSubscribe(email, subscribeQuery);
                _subscribeService.SendConfirmationSubscribeMessage(email, subscribeQuery);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("unsubscribe", Name = "Unsubscribe")]
        public IActionResult Unsubscripe(string subscriberId)
        {
            if (!string.IsNullOrEmpty(subscriberId))
            {
                _subscribeService.RemoveSubscribe(subscriberId);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}