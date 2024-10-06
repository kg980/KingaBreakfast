using ErrorOr;
using KingaBreakfast.Contracts.Breakfast;
using KingaBreakfast.Models;

namespace KingaBreakfast.Services.Breakfasts;

public interface IBreakfastService
{
    // Each method can either return the expected result Created,Deleted,Breakfast,UpsertedBreakfast OR a list of errors. => Handled by ErrorOr lib
    ErrorOr<Created> CreateBreakfast(Breakfast breakfast);
    ErrorOr<Breakfast> GetBreakfast(Guid id);
    ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast);
    ErrorOr<Deleted> DeleteBreakfast(Guid id);
}