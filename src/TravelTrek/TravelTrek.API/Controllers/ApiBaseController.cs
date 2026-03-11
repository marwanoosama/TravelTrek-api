using Microsoft.AspNetCore.Mvc;
using TravelTrek.Domain.Common;

namespace TravelTrek.API.Controllers
{
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        protected IActionResult ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);

            return StatusCode(GetStatusCode(result.Error), new { error = result.Error.Description });
        }

        protected IActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();

            return StatusCode(GetStatusCode(result.Error), new { error = result.Error.Description });
        }

        private static int GetStatusCode(Error error) => error.Type switch
        {
            ErrorType.Validation => 400,
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            ErrorType.Unauthorized => 401,
            ErrorType.Forbidden => 403,
            _ => 500
        };
    }
}
