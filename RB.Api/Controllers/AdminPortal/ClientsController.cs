using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RB.Application.Clients.Commands;
using RB.Application.Clients.Queries;
using RB.Application.Common.Exceptions;

namespace RB.Api.Controllers.AdminPortal
{
    [Route("api/admin-portal/[controller]")]
    [Authorize(Policy = "Admin Portal Endpoint")]
    public class ClientsController : BaseController
    {
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ListClientsResponse>), 200)]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListClientsQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientCommand command)
        {
            try
            {
                command.CurrentUser = "SA";
                var response = await Mediator.Send(command);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [HttpPost("{clientId}/request-consent")]
        public async Task<IActionResult> RequestConsent([FromRoute] int clientId)
        {
            try
            {
                var command = new RequestConsentCommand
                {
                    ClientId = clientId,
                    CurrentUser = "SA"
                };
                var response = await Mediator.Send(command);
                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
