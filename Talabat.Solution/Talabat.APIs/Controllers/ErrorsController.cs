using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Error;

namespace Talabat.APIs.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // ignore documenting this controller
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return code switch
            {
                401 => Unauthorized(new ApiResponse(code)),
                404 => NotFound(new ApiResponse(code)),
                _=> StatusCode(code),
            };
        }
    }
}
