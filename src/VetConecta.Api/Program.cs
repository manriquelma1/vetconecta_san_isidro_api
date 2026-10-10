using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using VetConecta.Api.Middleware;
using VetConecta.Application;
using VetConecta.Infrastructure;
using VetConecta.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

const string PoliticaAngular = "AngularDev";

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    // Sin esto los enums salen como numero en el JSON. Sin politica de nombres:
    // cada enum ya esta escrito (RolUsuario en minusculas, EstadoCita en
    // Mayuscula) para que el nombre del miembro coincida exacto con lo que
    // Angular espera, sin tener que forzar un casing global que no sirve para todos.
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddCors(options =>
{
    // En produccion, reemplazar por el dominio real donde se publique el Angular.
    options.AddPolicy(PoliticaAngular, policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("Falta la sección 'Jwt' en appsettings.json.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "VetConecta API", Version = "v1" });

    var esquemaBearer = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pega solo el token, sin el prefijo 'Bearer '."
    };
    options.AddSecurityDefinition("Bearer", esquemaBearer);

    // Microsoft.OpenApi 2.x: AddSecurityRequirement ahora recibe una fabrica con el
    // OpenApiDocument, porque la referencia al esquema necesita el documento host.
    options.AddSecurityRequirement(documento => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", documento), [] }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(PoliticaAngular);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
