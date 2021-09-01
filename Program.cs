using System.Net;
using System.Net.Http;
using System;
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

app.MapPost("/todos", async (HttpContext http, TodoService service) =>
{
    var dto = await http.Request.ReadFromJsonAsync<TodoDto>();
    
    var item = await service.CreateAsync(dto);

    await http.Response.WriteAsJsonAsync(item);
});
app.Run();
