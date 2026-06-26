using Business.Interfaces.Implements.Products;
using Business.Mapping;
using Business.Services.Products;
using Data.Interfaces.Implements.Products;
using Data.Interfaces.IRepository;
using Data.Repository;
using Data.Service.Products;
using Entity.Infrastructure.Context;
using Entity.Infrastructure.DataInit;
using Entity.Model;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Mapster
var mapsterConfig = MapsterConfig.Register();
builder.Services.AddSingleton(mapsterConfig);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// Data layer
builder.Services.AddScoped<IDataGeneric<Product>, DataGeneric<Product>>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Business layer
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Data seeder
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DataSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();