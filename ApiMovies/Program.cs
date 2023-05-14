using ApiMovies.Data;
using ApiMovies.MoviesMapper;
using ApiMovies.Repositories;
using ApiMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSecretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");

/**
 * Required packages for SQL server:
 * 
 * - Microsoft.EntityFrameworkCore.SqlServer
 * - Microsoft.EntityFrameworkCore.Tools
 * 
 * add-migration CreateCategoryTable
 * reverse-migration
 * update-database
 * 
 * Que es el Repository Pattern?
 * - Provee una capa de abstraccion de los datos. (Un intemediado entre EntityFramework y nuestra aplicacion)
 * Beneficios:
 * - Minimiza tener que duplicar codigo
 * - Desacopla la aplicacion de los frameworks como EntityFramework
 */

/**
 * Que es un DTO?
 * 
 * Data Transfer Object
 * Es un objeto que define cómo se enviarán los datos a través de la red.
 * 
 * Ventajas
 * - Oculta propiedades que el cliente no debe ver.
 * - Omite algunas propiedades con el fin de reducir la carga.
 * - Desacopla la capa de servicio del nivel de base de datos.
 * - Control absoluto sobre los atributos que recibe cuando de cree un nuevo recurso o se actualice.
 * - Puede haber diferentes DTO's para cada aversión de la API.
 * 
 * Que es CORS?
 * Se refiere al intercambio de recursos de origen cruzado.
 * Mecanismo que usa cabeceras adicionales para permitir que una app obtenga permiso para acceder a los recursos de un servidor (dominio diferente).
 */

// Configure the connection to sql server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

// Add cache settings
builder.Services.AddResponseCaching();

// Add services to the container.
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add automapper
builder.Services.AddAutoMapper(typeof(MoviesMapper));

// Authentication settings.
builder.Services.AddAuthentication(options =>
{
    // Use JWT schema for authentication
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters() 
    { 
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers(option =>
{
    // Cache Profile. Un cache global
    option.CacheProfiles.Add("PorDefecto20Segundos", new CacheProfile()
    {
        Duration = 20
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description =  @"
         JWT Authentication using Bearer schema.
         Insert 'Bearer' followed from a space and then your token in the input below.
         Ex. Bearer [yourToken]",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Soporte para CORS
// Se pueden habilidad: un dominio, multiples dominios, cualquier dominio (Tener en cuenta la seguridad).
// Por ejemplo la emprese me otorga el dominio: http://localhost:3223, se debe cambiar por el correcto.
// Se usa (*) para todos los dominios (API completamente publica).
builder.Services.AddCors(p => p.AddPolicy("Policiy Cors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Soporte para CORS
app.UseCors("Policiy Cors");

// Agregar para la autenticacion
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
