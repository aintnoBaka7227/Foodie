using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Foodie.Data.FoodieDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbcontext = services.GetRequiredService<Foodie.Data.FoodieDbContext>();
    dbcontext.Database.Migrate();
}

app.Run();