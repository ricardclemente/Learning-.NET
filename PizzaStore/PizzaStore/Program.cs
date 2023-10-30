using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;
//using PizzaStore.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo { 
        Title= "PizzaStore API - With entity framework", 
        Description = "Making the Pizzas you love", 
        Version = "v2" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "PizzaStore API V2");
});

app.MapGet("/", () => "Hello World!");
app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());
app.MapGet("/pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));
app.MapPost("/pizza", async (PizzaDb db, Pizza pizza)=>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}",pizza);
});
app.MapPut("/pizza/{id}", async (PizzaDb db, Pizza updatepizza, int id) => 
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null) return Results.NotFound();
    pizza.Name = updatepizza.Name;
    pizza.Description=updatepizza.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/pizza/{id}", async (PizzaDb db, int id) =>
{
   var pizza = await db.Pizzas.FindAsync(id);
   if (pizza is null) return Results.NotFound();
   db.Pizzas.Remove(pizza);
   await db.SaveChangesAsync();
   return Results.Ok();
});

/*app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));
app.MapGet("/pizzas", () => PizzaDB.GetPizzas());
app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));
app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));
app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));*/

app.Run();
