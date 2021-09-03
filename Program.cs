using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependencies
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

app.MapGet("/todos", async (HttpContext http, TodoService service) =>
{
    var items = await service.GetAllAsync();

    await http.Response.WriteAsJsonAsync(items);
});

app.MapGet("/todos/{id}", async (HttpContext http, TodoService service) =>
{
    http.Request.RouteValues.TryGetValue("id", out var id);
    var item = await service.GetByIdAsync(id.ToString());  

    await http.Response.WriteAsJsonAsync(item);
});

app.MapPost("/todos", async (HttpContext http, TodoService service) =>
{       
    var dto = await http.Request.ReadFromJsonAsync<TodoDto>();

    var item = await service.CreateAsync(dto);

    await http.Response.WriteAsJsonAsync(item);
}); 

app.MapPut("/todos/{id}", async (HttpContext http, TodoService service) =>
{
    http.Request.RouteValues.TryGetValue("id", out var id);
    var dto = await http.Request.ReadFromJsonAsync<TodoDto>();

    var item = await service.UpdateAsync(id.ToString(), dto);  

    await http.Response.WriteAsJsonAsync(item);
});

app.MapDelete("/todos/{id}", async (HttpContext http, TodoService service) =>
{
    http.Request.RouteValues.TryGetValue("id", out var id);

    await service.RemoveAsync(id.ToString());  

    http.Response.StatusCode = 204;
});

app.MapPut("/todos/mark-as-done/{id}", async (HttpContext http, TodoService service) =>
{
    http.Request.RouteValues.TryGetValue("id", out var id);
    var item = await service.MarkAsDoneAsync(id.ToString());  

    await http.Response.WriteAsJsonAsync(item);
});

app.Run();
