namespace CryptoCoinsParser.Api.ApplicationRoutes;

[Authorize]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(typeof(ApplicationError), StatusCodes.Status400BadRequest)]
public abstract class BaseApiController : ControllerBase
{
}
