using Business.Interfaces.Implements.Users;
using Business.Services.Users;
using Business.Interfaces.Implements.Auth;
using Business.Interfaces.Implements.Products;
using Business.Mapping;
using Business.Services.Auth;
using Business.Services.Products;
using Data.Interfaces.Implements.Auth;
using Data.Interfaces.Implements.Products;
using Data.Interfaces.Implements.Users;
using Data.Interfaces.IRepository;
using Data.Repository;
using Data.Service.Auth;
using Data.Service.Products;
using Data.Service.Users;
using Entity.Infrastructure.Context;
using Entity.Infrastructure.DataInit;
using Entity.Model;
using Entity.Model.Auth;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger con JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Inventario API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Ingrese el token JWT con el formato: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? "SistemaInventarioSecretKeyDevelopment123456789*";

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? "SistemaInventario";

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? "SistemaInventarioUsers";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        ),

        ClockSkew = TimeSpan.Zero
    };
});

// Mapster
var mapsterConfig = MapsterConfig.Register();
builder.Services.AddSingleton(mapsterConfig);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// Data layer
builder.Services.AddScoped<IDataGeneric<Product>, DataGeneric<Product>>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IDataGeneric<User>, DataGeneric<User>>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IDataGeneric<RefreshToken>, DataGeneric<RefreshToken>>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Business layer
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();