using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RB.Application.Common.Exceptions;
using RB.Application.Members.Commands;

namespace RB.Api.Controllers.ClientPortal
{
    [Route("api/client-portal/[controller]")]
    [Authorize(Policy = "Client Portal Endpoint")]
    public class MembersController : BaseController
    {
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPut("{memberId}/assign-to-teams")]
        public async Task<IActionResult> AsignToTeamsAsync([FromRoute] int memberId,
                                                           [FromBody] AssignMemberToTeamsCommand command,
                                                           CancellationToken cancellationToken)
        {
            try
            {
                // TODO
                command.MemberId = memberId;
                command.CurrentUser = "SA";
                await Mediator.Send(Request, cancellationToken);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
