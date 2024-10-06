namespace   KingaBreakfast.Contracts.Breakfast;


// Create properties need to match what is in the Api.md for POST section
public record UpsertBreakfastRequest(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    List<string> Savory,
    List<string> Sweet);



