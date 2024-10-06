using Microsoft.AspNetCore.Mvc;

namespace KingaBreakfast.Controllers;

public class ErrorsController : ControllerBase //Microsoft.AspNetCore.Mvc.ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        return Problem(); // returns generic internal program error 'HTTP/1.1 500 Internal Server Error' - abstracting the sensitive details of the error from end client :)
    }
}