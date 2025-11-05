using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc
    (
        "v1",
        new OpenApiInfo
        {
            Title = "UniShare API",
            Version = "v1",
            Description = "API for managing the UniShare application",
            Contact = new OpenApiContact
            {
                Name = "API Support",
                Email = "support@example.com",
            }
        });
});


builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI
    (c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniShare API V1");
            c.RoutePrefix = string.Empty; // Set Swagger UI at app's root
            c.DisplayRequestDuration();
        }
    );
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/db-check", async (ApplicationContext db) =>
{
    var result = new
    {
        canConnect = await db.Database.CanConnectAsync(),
        tables = new List<string>()
    };

    if (result.canConnect)
    {
        var conn = db.Database.GetDbConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT tablename FROM pg_tables WHERE schemaname='public'";
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.tables.Add(reader.GetString(0));
        }
    }

    return Results.Ok(result);
});


app.Run();