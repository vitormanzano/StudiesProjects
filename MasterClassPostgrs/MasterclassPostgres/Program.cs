using MasterclassPostgres.Data;
using MasterclassPostgres.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.MapPost("v1/categories", async
    (AppDbContext context, Category category) =>
{
    await context.Categories.AddAsync(category);
    await context.SaveChangesAsync();
    
    return Results.Created($"v1/categories/{category.Id}", category);
});

app.MapPut("v1/categories/{id}", async
    (AppDbContext context, int id,Category model) =>
{
    var category = await context.Categories.FindAsync(id);
    
    if (category is null)
        return Results.NotFound();
    
    category.Heading = model.Heading;
    
    context.Categories.Update(category);
    await context.SaveChangesAsync();
    
    return Results.Ok(category);
});

app.MapDelete("v1/categories/{id}", async
    (AppDbContext context, int id) =>
{
    var category = await context.Categories.FindAsync(id);
    
    if (category is null)
        return Results.NotFound();
    
    context.Categories.Remove(category);
    await context.SaveChangesAsync();
    
    return Results.Ok(category);
});

app.MapGet("v1/categories", async
    (AppDbContext context) =>
{
    var categories = await context.Categories
        .AsNoTracking()
        .ToListAsync();
    
    return Results.Ok(categories);
});

app.MapGet("v1/categories/{id}", async
    (AppDbContext context, int id) =>
{
    var category = await context.Categories
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id);
    
    if (category is null)
        return Results.NotFound();

    return Results.Ok(category);
});



app.Run();