using Microsoft.EntityFrameworkCore;
using Serilog;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;
using UserManagement.Infrastructure.Data;
using UserManagement.Domain.Interfaces;
using UserManagement.Infrastructure.Repositories;
using Prometheus;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Configuración de servicios
builder.Services.AddControllers();

// Configuración del DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dependencias
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


// Configuración de autenticación JWT
var jwtSecret = builder.Configuration["Jwt:Key"];
var key = Encoding.UTF8.GetBytes(jwtSecret);




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
}
);

// Configuración de Swagger con autenticación JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManagement API", Version = "v1" });

    // Configurar JWT para Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese 'Bearer' [espacio] seguido del token JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

// Agregar servicios de Prometheus
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configuración de middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManagement API v1");
        c.RoutePrefix = string.Empty; // Mostrar Swagger en la raíz
    });
}



app.UseHttpsRedirection();

// Configuración de Prometheus para exposición de métricas
app.UseHttpMetrics();

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();


app.MapMetrics();
app.MapControllers();

// Configurar Kestrel para escuchar en el puerto 80
app.Urls.Add("http://*:80");


// Endpoint para Health Checks
//app.MapHealthChecks("/health");

app.Run();
