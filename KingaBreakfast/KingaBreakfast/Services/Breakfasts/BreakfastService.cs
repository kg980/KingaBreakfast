using ErrorOr;
using KingaBreakfast.ServiceErrors;
using KingaBreakFast.Models;

namespace KingaBreakfast.Services.Breakfasts;

public class BreakfastService : IBreakfastService
{
    // storing breakfasts in dictionary so can look them up by id in the Get method
    // static so we dont make a new dictionary every time obj instantiated!!
    private static readonly Dictionary<Guid, Breakfast> _breakfasts = new(); // new dictionary called _breakfasts, key guid, value breakfast object
    public ErrorOr<Created> CreateBreakfast(Breakfast breakfast)
    {
        _breakfasts.Add(breakfast.Id, breakfast); // add key-value pair to Dictionary
        return Result.Created; // Comes from ErrorOr lib
    }

    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        //_breakfasts.Remove(id);
        //return Result.Deleted;\
        if (_breakfasts.TryGetValue(id, out var breakfast))
        {
            _breakfasts.Remove(id);
            return Result.Deleted;
        }
        
        return Errors.Breakfast.NotFound;
    }

    public ErrorOr<Breakfast> GetBreakfast(Guid id)
    {
        if (_breakfasts.TryGetValue(id, out var breakfast))
        {
            return breakfast;
        }
        
        return Errors.Breakfast.NotFound;
    }

    public ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast)
    {
        // replace breakfast with matching Id with the new data, or add a new entry if id not found
        var isNewlyCreated = !_breakfasts.ContainsKey(breakfast.Id);
        _breakfasts[breakfast.Id] = breakfast;

        return new UpsertedBreakfast(isNewlyCreated); //Tells us true or false whether the breakfast is a new record or an updated one.
    }
}