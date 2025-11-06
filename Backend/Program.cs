using Backend.Features.Items;
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
builder.Services.AddScoped<GetItemsHandler>();
builder.Services.AddScoped<GetItemHandler>();
builder.Services.AddScoped<PostItemHandler>();
builder.Services.AddScoped<DeleteItemHandler>();
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

app.MapGet("/items", async (GetItemsHandler handler) => await handler.Handle());
app.MapGet("items/{id:guid}", async (Guid id, GetItemHandler handler) => await handler.Handle(new GetItemRequest(id)));
app.MapPost("items", async (PostItemRequest request, PostItemHandler handler) =>  await handler.Handle(request));
app.MapDelete("items/{id:guid}", async (Guid id, DeleteItemHandler handler) => await handler.Handle(new DeleteItemRequest(id)));
await app.RunAsync();
