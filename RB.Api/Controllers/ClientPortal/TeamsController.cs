using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RB.Application.Common.Exceptions;
using RB.Application.Teams.Commands;

namespace RB.Api.Controllers.ClientPortal
{
    [Route("api/client-portal/[controller]")]
    [Authorize(Policy = "Client Portal Endpoint")]
    public class TeamsController : BaseController
    {
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPut("{teamId}/allocate-members")]
        public async Task<IActionResult> AllocateMembersAsync([FromRoute] int teamId,
                                                              [FromBody] AllocateTeamMembersCommand command,
                                                              CancellationToken cancellationToken)
        {
            try
            {
                // TODO
                command.TeamId = teamId;
                command.CurrentUser = "SA";
                await Mediator.Send(command, cancellationToken);
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
