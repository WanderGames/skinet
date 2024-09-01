using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//add our db context service and tell it what options to use
builder.Services.AddDbContext<StoreContext>(options => 
{
    //set up the connection string to our database using the connection string that is in our appsettings.json
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//add our repositories, this is scoped meaning it will be created once per request

//specify the interface and the implementation class
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//since this repository uses generics we need to add it with the typeof(repository)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

//seed the database, this runs when application starts
try
{
    //any code we create that uses this variable will removed after the try catch
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    //updates the database or creates it if it doesn't exist, applies all pending migrations
    await context.Database.MigrateAsync();

    //call our seed method
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

app.Run();
