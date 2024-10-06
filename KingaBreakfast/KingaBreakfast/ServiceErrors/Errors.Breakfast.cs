using ErrorOr;

namespace KingaBreakfast.ServiceErrors;

public static class Errors
{
    public static class Breakfast
    {
        // If a breakfast isn't found. Using ErrorOr, populating an Error object's fields. See Error class (from ErrorOr package) for more info.
        public static Error NotFound => Error.NotFound(
            code: "Breakfast.NotFound",
            description: "Breakfast not found"
        );
    }
}