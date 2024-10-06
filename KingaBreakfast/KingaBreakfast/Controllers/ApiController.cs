using System.Security.Cryptography;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace KingaBreakFast.Controllers;

[ApiController]
//[Route("breakfasts")] // < allows me to not have to spam "breakfasts/{id:guid}" in all the below endpoints by specifuing the Route here
// because the route names are usually the name of the class (BreakfastsController) without the Controller (so just Breakfasts), 
// we can use this method to prefix the endpoints with the name of the class minus "controller", reducing hard-coded names, more flexible:
[Route("[controller]")]
public class ApiController : ControllerBase
{
    //Overwriting the Problem(); method from ControllerBase with my own implementation instead in ApiController class :)
    protected IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}