using System.Security.Cryptography;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KingaBreakfast.Controllers;

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
        // if all the errors in the error list are validation errors, iterate through the errors and add error codes & descriptions to the dictionary
        // allows us to output all validation errors, such as name and description lengths, to the client so they can correct their request rather than only the first error.
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(modelStateDictionary);
        }

        // If any one of the errors in the list are an unexpected type, we cant trust any of them therefore return internal server error.
        if (errors.Any(e => e.Type == ErrorType.Unexpected))
        {
            return Problem();
        }

        // else only return first error
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




//example error list working:
// "errors": {
// "Breakfast.InvalidName": [
//     "Breakfast name must be between 3 and 50 characters.",
//     "Breakfast description must be between 50 and 150 characters."
// ]
// },