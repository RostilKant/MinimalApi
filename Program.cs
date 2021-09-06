using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependencies
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("db"));
builder.Services.AddScoped<TodoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello Minimal API!");

app.MapGet("/todos", (TodoService service) =>
{
    reutrn service.GetAllAsync();
});

app.MapGet("/todos/{id}", (string id, TodoService service) =>
{
    return service.GetByIdAsync(id);
});

app.MapPost("/todos", (TodoDto dto, TodoService service) =>
{       
    return service.CreateAsync(dto);
}); 

app.MapPut("/todos/{id}", (string id, TodoDto dto, TodoService service) =>
{
    return service.UpdateAsync(id, dto);
});

app.MapDelete("/todos/{id}", async (string id, TodoService service) =>
{
    await service.RemoveAsync(id);

    return Results.NoContent();
});

app.MapPut("/todos/mark-as-done/{id}", (string id, TodoService service) =>
{
    return service.MarkAsDoneAsync(id);
});

app.Run();
