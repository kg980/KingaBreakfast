using ErrorOr;
using KingaBreakfast.Models;

namespace KingaBreakfast.ServiceErrors;


public static class Errors
{
    public static class Breakfast
    {
        public static Error InvalidName => Error.Validation(
            code: "Breakfast.InvalidName",
            description: $" Breakfast name must be between {Models.Breakfast.MinNameLength} and {Models.Breakfast.MaxNameLength} characters."
        );

        public static Error InvalidDescription => Error.Validation(
            code: "Breakfast.InvalidName",
            description: $" Breakfast description must be between {Models.Breakfast.MinDescriptionLength} and {Models.Breakfast.MaxDescriptionLength} characters."
        );

        // If a breakfast isn't found. Using ErrorOr, populating an Error object's fields. See Error class (from ErrorOr package) for more info.
        public static Error NotFound => Error.NotFound(
            code: "Breakfast.NotFound",
            description: " Breakfast not found"
        );
    }
}