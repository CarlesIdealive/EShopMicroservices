using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);
// BEFORE - Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//Exception Handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();

// AFTER - Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(options => { });




app.Run();
