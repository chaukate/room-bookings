using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RB.Api.Controllers.ClientPortal
{
    [Route("api/client-portal/[controller]")]
    [Authorize(Policy = "Client Portal Endpoint")]
    public class ClientsControllerController : ControllerBase
    {

    }
}