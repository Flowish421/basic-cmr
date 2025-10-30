using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using BasicCMR.Infrastructure.Data;
using BasicCMR.Application;
using BasicCMR.Application.Interfaces;
using BasicCMR.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// Databas — EF Core
// =====================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================================================
// Caching (för DashboardService)
// =====================================================
builder.Services.AddMemoryCache();

// =====================================================
//  Controllers
// =====================================================
builder.Services.AddControllers();

// =====================================================
// Application services + Dependency Injection
// =====================================================
builder.Services.AddApplication();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// =====================================================
// CORS-policy (React-klienten)
// =====================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// =====================================================
// JWT Authentication Setup (warning-free)
// =====================================================
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("❌ JWT Key saknas i appsettings.json");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("❌ JWT Issuer saknas i appsettings.json");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("❌ JWT Audience saknas i appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// =====================================================
// Swagger (med JWT-stöd)
// =====================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BasicCMR API",
        Version = "v1",
        Description = "Ett enkelt CRM-system med JWT-auth, job applications och användarhantering."
    });

    // JWT-integration i Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Skriv **Bearer** följt av ett mellanslag och sedan din token.\n\nExempel: `Bearer eyJhbGciOiJIUzI1NiIs...`"
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
            new string[] {}
        }
    });
});

// =====================================================
//  Bygger och konfigurera appen
// =====================================================
var app = builder.Build();

// =====================================================
// Middleware-pipeline
// =====================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
