using ApiCatalogoMinimal.Context;
using ApiCatalogoMinimal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => 
                 options.
                 UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

app.MapGet("/", () => "Catálogo de Produtos - 2026");

app.MapPost("/categorias", async ([FromBody]Categoria categoria, [FromServices]AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

app.MapGet("/categoria", async ([FromServices]AppDbContext db) => await db.Categorias.ToListAsync());

app.MapGet("/categoria/{id:int}", async(int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id)
        is Categoria categoria ? Results.Ok(categoria)
                               : Results.NotFound();
});

app.MapPut("/categorias/{id:int}", async());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
