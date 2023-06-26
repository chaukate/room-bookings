using Microsoft.AspNetCore.Mvc;
using RB.Application.Common.Exceptions;
using RB.Application.Rooms.Commands;
using RB.Application.Rooms.Queries;

namespace RB.Api.Controllers
{
    public class RoomsController : BaseController
    {
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ListRoomsResponse>), 200)]
        [HttpGet]
        public async Task<IActionResult> ListAsync(CancellationToken cancellationToken)
        {
            var query = new ListRoomsQuery();
            var response = await Mediator.Send(query, cancellationToken);
            return Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(GetRoomResponse), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetRoomQuery { Id = id, };
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
        public async Task<IActionResult> CreateAsync([FromBody] CreateRoomCommand command,
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
                                                     [FromBody] UpdateRoomCommand command,
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
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
