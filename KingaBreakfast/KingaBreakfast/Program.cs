using KingaBreakfast.Services.Breakfasts;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    // every time program instantiates an object, that object requests the IBreakfastService in the contsructor, and then make instance of BreakfastService object to implement it
    builder.Services.AddScoped<IBreakfastService, BreakfastService>(); 
    // AddScoped = within the lifetime of a single request, first time we request the IBreakfastService, create instance of BreakfastService, but until
    // end of life of the request, all subsequent requests will ue the same BreakfastService object.
    // prevent constantly making new instances of BreakfastService (We want to re-use the same one because we want to re-use & share our static breakfast dictionary!!!)

}

// Pipeline that a request goes through
var app = builder.Build();
{
    // Adding middleware to prevent exceptions getting thrown to client -> may contain sensitive info!
    app.UseExceptionHandler("/error"); // built-in 'try-catch' surrounding the following midlewares, if exception thrown, changes request route to the defined route "/error" and re-executes the req (see ErrorsController.cs)
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();  
}

