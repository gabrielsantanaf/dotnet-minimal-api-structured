using ApiCatalogoMinimal.Context;
using ApiCatalogoMinimal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoMinimal.ApiEndpoints
{
    public static class CategoriasEndpoints
    {
        public static void MapCategoriaEndpoint(this WebApplication app)
        {
            app.MapGet("/", () => "Catálogo de Produtos - 2026").ExcludeFromDescription();

            app.MapPost("/categorias", async ([FromBody] Categoria categoria, [FromServices] AppDbContext db) =>
            {
                db.Categorias.Add(categoria);
                await db.SaveChangesAsync();

                return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
            });

            app.MapGet("/categoria", async ([FromServices] AppDbContext db) => await db.Categorias.ToListAsync()).RequireAuthorization();

            app.MapGet("/categoria/{id:int}", async (int id, AppDbContext db) =>
            {
                return await db.Categorias.FindAsync(id)
                    is Categoria categoria ? Results.Ok(categoria)
                                           : Results.NotFound();
            });

            app.MapPut("/categorias/{id:int}", async (int id, [FromBody] Categoria categoria, AppDbContext db) =>
            {
                if (categoria.CategoriaId != id)
                    return Results.BadRequest();

                var categoriaDB = await db.Categorias.FindAsync(id);

                if (categoriaDB is null)
                    return Results.NotFound();

                categoriaDB.Nome = categoria.Nome;
                categoriaDB.Descricao = categoria.Descricao;

                await db.SaveChangesAsync();
                return Results.Ok(categoriaDB);
            });

            app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
            {
                var categoria = await db.Categorias.FindAsync(id);

                if (categoria is null)
                    return Results.NotFound();

                db.Categorias.Remove(categoria);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
