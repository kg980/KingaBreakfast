namespace   KingaBreakFast.Contracts.Breakfast;

// Response properties need to match what is in the Api.md for GET section
public record BreakfastResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    DateTime LastModified,
    List<string> Savory,
    List<string> Sweet);

