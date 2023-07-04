using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RB.Application.Interfaces;
using RB.Infrastructure.Services;

namespace RB.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Exchange Endpoint")]
    public class AccountsController : BaseController
    {
        private readonly ISlackService _slackService;
        public AccountsController(ISlackService slackService)
        {
            _slackService = slackService;
        }

        [Produces("aplication/json")]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(CancellationToken cancellationToken)
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> SendMessageAsync(string message, CancellationToken cancellationToken)
        {
            await _slackService.SendMessageAsync(message, cancellationToken);
            return Ok();
        }
    }
}