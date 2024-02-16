
using Catagory.Models;
using CatalogoApi.Context;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            var app = builder.Build();


            app.MapPost("/categorias", async (Categoria categoria, AppDbContext context) =>
            {
                context.Categorias.Add(categoria);
                await context.SaveChangesAsync();
                return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
            });

            app.MapGet("/categorias", async (AppDbContext context) =>
            {
                return await context.Categorias.ToListAsync();
            });

            app.MapGet("/categorias/{id}", async (int id, AppDbContext context) =>
            {
                var categoria = await context.Categorias.FindAsync(id);
                if (categoria is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(categoria);
            });

            app.MapPut("/categorias/{id}", async (int id, Categoria categoria, AppDbContext context) =>
            {
                if (id != categoria.CategoriaId)
                {
                    return Results.BadRequest();
                }

                context.Entry(categoria).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await context.Categorias.FindAsync(id) is null)
                    {
                        return Results.NotFound();
                    }
                    throw;
                }

                return Results.NoContent();
            });

            app.MapDelete("/categorias/{id}", async (int id, AppDbContext context) =>
            {
                var categoria = await context.Categorias.FindAsync(id);
                if (categoria is null)
                {
                    return Results.NotFound();
                }

                context.Categorias.Remove(categoria);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapPost("/produtos", async (Produto produto, AppDbContext context) =>
                {
                    context.Produtos.Add(produto);
                    await context.SaveChangesAsync();
                    return Results.Created($"/produtos/{produto.ProdutoId}", produto);
                }).Produces<Produto>(StatusCodes.Status201Created)
                .WithName("CriaProduto")
                .WithTags("Produtos");

            app.MapGet("/produtos", async (AppDbContext context) =>
            {
                return await context.Produtos.ToListAsync();
            });

            app.MapGet("/produtos/{id}", async (int id, AppDbContext context) =>
            {
                var produto = await context.Produtos.FindAsync(id);
                if (produto is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(produto);
            });

            app.MapPut("/produtos/{id}", async (int id, Produto produto, AppDbContext context) =>
            {
                if (id != produto.ProdutoId)
                {
                    return Results.BadRequest();
                }

                context.Entry(produto).State = EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await context.Produtos.FindAsync(id) is null)
                    {
                        return Results.NotFound();
                    }
                    throw;
                }

                return Results.NoContent();
            });

            app.MapDelete("/produtos/{id}", async (int id, AppDbContext context) =>
            {
                var produto = await context.Produtos.FindAsync(id);
                if (produto is null)
                {
                    return Results.NotFound();
                }

                context.Produtos.Remove(produto);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
