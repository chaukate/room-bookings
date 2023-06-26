using Microsoft.AspNetCore.Mvc;
using RB.Application.Common.Exceptions;
using RB.Application.Members.Commands;
using RB.Application.Members.Queries;
using RB.Application.Teams.Queries;

namespace RB.Api.Controllers
{
    public class MembersController : BaseController
    {
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ListMembersResponse>), 200)]
        [HttpGet]
        public async Task<IActionResult> ListAsync(CancellationToken cancellationToken)
        {
            var query = new ListTeamsQuery();
            var response = await Mediator.Send(query, cancellationToken);
            return Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(GetMemberResponse), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id,
                                                  CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetMemberQuery { Id = id };
                var response = await Mediator.Send(query, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateMemberCommand command,
                                                     CancellationToken cancellationToken)
        {
            try
            {
                // TODO
                command.CurrentUser = "SA";
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
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

        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id,
                                                     [FromBody] UpdateMemberCommand command,
                                                     CancellationToken cancellationToken)
        {
            try
            {
                // TODO
                command.Id = id;
                command.CurrentUser = "SA";
                await Mediator.Send(command, cancellationToken);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
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
