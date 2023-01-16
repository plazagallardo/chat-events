using Chat.Core.Application.Domain;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Infrastructure.Persistence;
using ChatAPI.Controllers.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IRepository<AccessRoomEvent> _eventRepository;

        public UsersController(IRepository<AccessRoomEvent> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsRequest request)
        {
            //TODO: subscribe the user to the chat room

            await _eventRepository.AddAsync(new AccessRoomEvent(request.Date, request.Username, EventType.EnterRoom));

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] CredentialsRequest request)
        {
            //unsubscribe the user to the chat room

            await _eventRepository.AddAsync(new AccessRoomEvent(request.Date, request.Username, EventType.LeaveRoom));

            return Ok();
        }
    }
}