using System.Reflection.Metadata.Ecma335;
using ErrorOr;
using KingaBreakfast.Contracts.Breakfast;
using KingaBreakfast.ServiceErrors;

namespace KingaBreakfast.Models;

public class Breakfast
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 50;

    public const int MinDescriptionLength = 50;
    public const int MaxDescriptionLength = 150;


    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    public DateTime LastModifiedDateTime { get; }
    public List<string> Savory { get; }
    public List<string> Sweet { get; }
    

    // Made constructor private, so only way to make instance of Breakfast is to use the static Create method which enforces invariants and handles errors
    private Breakfast(Guid id, string name, string description, DateTime startDateTime, DateTime endDateTime, DateTime lastModifiedDateTime, List<string> savory, List<string> sweet)
    {
        Id = id;
        Name = name;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        LastModifiedDateTime = lastModifiedDateTime;
        Savory = savory;
        Sweet = sweet;
    }
    
    
    public static ErrorOr<Breakfast> Create(
        string name, 
        string description, 
        DateTime startDateTime, 
        DateTime endDateTime, 
        //DateTime lastModifiedDateTime, 
        List<string> savory, 
        List<string> sweet,
        Guid? id = null // optional field id so can use this for the upsert method as well
    )
    {
        // enforce invariants - enforcing business rules on our internal service models.
        List<Error> errors = new();

        if(name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Breakfast.InvalidName);
            //return Errors.Breakfast.InvalidName;
        }

        if(description.Length is < MinDescriptionLength or > MaxDescriptionLength)
        {
            errors.Add(Errors.Breakfast.InvalidDescription);
            //return Errors.Breakfast.InvalidDescription;
        }

        if(errors.Count > 0)
        {
            return errors;
        }

        
        return new Breakfast(
            id ?? Guid.NewGuid(), //if id wasn't specified, generate one
            name, 
            description, 
            startDateTime, 
            endDateTime,
            DateTime.UtcNow, 
            savory, 
            sweet
        );
    }

    // static factory methods
    public static ErrorOr<Breakfast> From(CreateBreakfastRequest request)
    {
        return Create(
            //Guid.NewGuid(), // id
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            //DateTime.UtcNow, // lastModifiedDateTime
            request.Savory,
            request.Sweet
        );
    }
    public static ErrorOr<Breakfast> From(Guid id, UpsertBreakfastRequest request)
    {
        return Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet,
            id
        );
    }

}

