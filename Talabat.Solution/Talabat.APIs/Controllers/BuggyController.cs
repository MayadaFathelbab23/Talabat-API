using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.DTOs;
using Talabat.APIs.Error;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    
    public class BuggyController : APIBaseController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }
        // Add Endpoint to return different types of errors
        // 1. NotFound 
        [HttpGet("notfound")] // GET : api/buggy/notfound
        public ActionResult GetNotFound()
        {
            var product = _context.Products.Find(1000); // No product with id = 1000
            if(product == null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }

        // 2. Server Error
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(1000);
            var productToReturnDto = product.ToString(); // Null Reference Exception
            return Ok(productToReturnDto);
        }

        // 3. Bad Request
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        // 4. BadRequest -> validation Error
        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id) // api/buggy/five
        {
            var product = _context.Products.Find(id);
            return Ok(product);
        }
        // 5. Unauthorized
        [HttpGet("unauthorized")]
        public ActionResult GetUnAuthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }

    }
}
