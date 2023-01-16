using Chat.Core.Application.Domain;
using ChatAPI.Controllers.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatEventsController : ControllerBase
    {

        private readonly IChatEventService _chatEventService;

        public ChatEventsController(IChatEventService chatEventService)
        {
            _chatEventService = chatEventService;
        }

        [HttpGet(Name = "GetActivity")]
        public async Task<IActionResult> GetActivity(string timeGranularity)
        {
            var events = await _chatEventService.GetAllEventsActivityAsync(timeGranularity);

            return Ok(events);
        }

        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> SendMessage([FromBody] SendHighFiveRequest request)
        {
            await _chatEventService.SendHighFive(request.Date, request.RecipientName, request.SenderName);

            return Ok();
        }

        [HttpPost]
        [Route("highfive")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            await _chatEventService.SendTextMessage(request.Date, request.SenderName, request.Text);

            return Ok();
        }
    }
}