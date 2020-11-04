﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [ApiController]
    [Area("Customers")]
    [ApiExplorerSettings(GroupName = "'Customer.API v'VVVV")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public abstract class BaseController : ControllerBase
    {
    }
}