using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var supabasePassword = new NpgsqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"))
{
    Password = builder.Configuration["SUPABASE_DB_PASSWORD"] ?? throw new InvalidOperationException("SUPABASE_DB_PASSWORD environment variable is not set.")
};

builder.Services.AddDbContext<Foodie.Data.FoodieDbContext>(options =>
    options.UseNpgsql(supabasePassword.ConnectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbcontext = services.GetRequiredService<Foodie.Data.FoodieDbContext>();
    dbcontext.Database.Migrate();
}

app.Run();