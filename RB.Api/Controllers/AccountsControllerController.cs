using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RB.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Exchange Endpoint")]
    public class AccountsControllerController : BaseController
    {
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
    }
}