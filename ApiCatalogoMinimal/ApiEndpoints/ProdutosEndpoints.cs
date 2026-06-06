using ApiCatalogoMinimal.Context;
using ApiCatalogoMinimal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoMinimal.ApiEndpoints
{
    public static class ProdutosEndpoints
    {
        public static void MapProdutoEndpoint(this WebApplication app)
        {
            //-------Endpoints-Produtos---------//
            app.MapGet("/produtos", async ([FromServices] AppDbContext db) =>
            {
                return await db.Produtos.ToListAsync();
            }).RequireAuthorization();

            app.MapGet("/produtos/{id:int}", async (int id, [FromServices] AppDbContext db) =>
            {
                return await db.Produtos.FindAsync(id)
                    is Produto produto ? Results.Ok(produto)
                                       : Results.NotFound();
            });

            app.MapPost("/produtos", async ([FromBody] Produto produto, [FromServices] AppDbContext db) =>
            {
                db.Produtos.Add(produto);
                await db.SaveChangesAsync();

                return Results.Created($"$/categorias/{produto.ProdutoId}", produto);
            });

            app.MapPut("/produtos/{id:int}", async (int id, [FromBody] Produto produto, [FromServices] AppDbContext db) =>
            {
                if (produto.ProdutoId != id)
                    return Results.BadRequest();

                var produtoDB = await db.Produtos.FindAsync(id);

                if (produtoDB is null)
                    return Results.NotFound();

                produtoDB.Nome = produto.Nome;
                produtoDB.Descricao = produto.Descricao;
                produtoDB.Preco = produto.Preco;
                produtoDB.Imagem = produto.Imagem;
                produtoDB.DataCompra = produto.DataCompra;
                produtoDB.Estoque = produto.Estoque;
                produtoDB.CategoriaId = produto.CategoriaId;

                await db.SaveChangesAsync();

                return Results.Ok(produtoDB);
            });

            app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
            {
                var produtoDB = await db.Produtos.FindAsync(id);

                if (produtoDB is null)
                    return Results.NotFound();

                db.Produtos.Remove(produtoDB);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}
