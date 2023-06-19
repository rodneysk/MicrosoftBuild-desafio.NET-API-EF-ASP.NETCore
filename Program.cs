using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PocarStore.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PocarC") ?? "Data Source=PocarC.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<PocarDb>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "PocarStore API",
         Description = "Quer economizar na passagem? Cola com a gente!",
         Version = "v1" });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "PocarStore API V1");
});

app.MapGet("/", () => "Hello World!");
app.MapGet("/pocar", async (PocarDb db) => await db.PocarC.ToListAsync());
app.MapGet("/pocar/{id}", async (PocarDb db, int id) => await db.PocarC.FindAsync(id));

app.MapPost("/pocar", async (PocarDb db, Pocar pocar) =>
{
    await db.PocarC.AddAsync(pocar);
    await db.SaveChangesAsync();
    return Results.Created($"/pocar/{pocar.Id}", pocar);
});

app.MapPut("/pocar/{id}", async (PocarDb db, Pocar updatepocar, int id) =>
{
      var pocar = await db.PocarC.FindAsync(id);
      if (pocar is null) return Results.NotFound();
      pocar.Marca = updatepocar.Marca;
      pocar.Modelo = updatepocar.Modelo;
      await db.SaveChangesAsync();
      return Results.NoContent();
});

app.MapDelete("/pocar/{id}", async (PocarDb db, int id) =>
{
   var pocar = await db.PocarC.FindAsync(id);
   if (pocar is null)
   {
      return Results.NotFound();
   }
   db.PocarC.Remove(pocar);
   await db.SaveChangesAsync();
   return Results.Ok();
});

app.Run();
