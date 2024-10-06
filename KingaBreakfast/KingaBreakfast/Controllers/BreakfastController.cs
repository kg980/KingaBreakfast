using System.Reflection.Metadata.Ecma335;
using ErrorOr;
using KingaBreakfast.ServiceErrors;
using KingaBreakfast.Services.Breakfasts;
using KingaBreakFast.Contracts.Breakfast;
using KingaBreakFast.Controllers;
using KingaBreakFast.Models;
using Microsoft.AspNetCore.Mvc;

namespace KingaBreakfast.Controllers;

// Added reference from this project to contracts project
// dotnet add .\KingaBreakfast\ reference .\KingaBreakfast.Contracts\

//[ApiController] -> moved to inherited class, ApiController
//[Route("[controller]")] -> moved to inherited class, ApiController
public class BreakfastsController : ApiController
{
    // Dependency injection
    private readonly IBreakfastService _breakfastService;

    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

    [HttpPost]
    
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        // map the data from the request to the language our app speaks (defined by our class in Models folder)
        var breakfast = new Breakfast(
            Guid.NewGuid(), // id
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            DateTime.UtcNow, // lastModifiedDateTime
            request.Savory,
            request.Sweet
        );

        // save breakfast to database
        ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);

        return createBreakfastResult.Match(
            created => CreatedAtGetBreakfast(breakfast),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        // read the breakfast we want from the dictionary
        // ErrorOr will contain either the breakfast value, or a list of the errors.
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

        //Match method from ErrorOr class handlestwo outcomes: if get value successfully, do one thing, if it gets an error list, do another thing.
        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors)
        );
        
        
        // if (getBreakfastResult.IsError &&
        // getBreakfastResult.FirstError == Errors.Breakfast.NotFound)
        // {
        //     return NotFound();
        // }

        // //If not an error, wont go into the above if, therefore populate breakfast value:
        // var breakfast = getBreakfastResult.Value;

        // // map the breakfast to the response
        // BreakfastResponse response = MapBreakfastResponse(breakfast);

        // return Ok(response);
       
    }

    

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            id, // ID arg of breakfast that we want to fetch
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            DateTime.UtcNow, // lastModifiedDateTime
            request.Savory,
            request.Sweet
        );

        ErrorOr<UpsertedBreakfast> upsertBreakfastResult =_breakfastService.UpsertBreakfast(breakfast);

        // return 201 if new breakfast created, otherwise return NoContent, just update record :)
        return upsertBreakfastResult.Match(
            upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors)
        ); 
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        ErrorOr<Deleted> deleteBreakfastResult = _breakfastService.DeleteBreakfast(id);
        
        return deleteBreakfastResult.Match(
            deleted => NoContent(), // if value, then successfully return NoContent
            errors => Problem(errors) //if error list, send the error list to our Problem handler.
        );
    }
    
    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
            breakfast.Id,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet
        );
    }
    private IActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {

        // convert that data back to the API definition so we can return appropriate response
        // return the response
        return CreatedAtAction(
            actionName: nameof(GetBreakfast), //the GetBreakfast endpoint
            routeValues: new { id = breakfast.Id }, //need to pass id because the Get endpoint below needs an id arg
            value: MapBreakfastResponse(breakfast));
    }
}