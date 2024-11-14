using Microsoft.AspNetCore.Mvc;

namespace CleanApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {

        [NonAction]
        public IActionResult CreateActionResult<T>(App.Application.ServiceResult<T> result)
        {
            if (result.Status == System.Net.HttpStatusCode.NoContent)
            {
                return NoContent();
            }

            if (result.Status == System.Net.HttpStatusCode.NoContent)
            {
                return Created(result.UrlAsCreated, result);
            }
            return new ObjectResult(result) { StatusCode = result.Status.GetHashCode() };

        }

        [NonAction]
        public IActionResult CreateActionResult(App.Application.ServiceResult result)
        {
            if (result.Status == System.Net.HttpStatusCode.NoContent)
            {
                return new ObjectResult(null) { StatusCode = result.Status.GetHashCode() };
            }
            return new ObjectResult(result) { StatusCode = result.Status.GetHashCode() };

        }
    }
}
