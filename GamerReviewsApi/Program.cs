using GamerReviewsApi.EndPoints.BusquedaEndP.Handlers;
using GamerReviewsApi.EndPoints.JuegoEndP.Handlers;
using GamerReviewsApi.EndPoints.LoginEndP.Handlers;
using GamerReviewsApi.Repository.Helpers;
using GamerReviewsApi.Repository.Interfaces;
using GamerReviewsApi.Repository.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
}
);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DatabaseConecction>();
builder.Services.AddSingleton<JWTGen>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<postGame>();
builder.Services.AddScoped<getGames>();
builder.Services.AddScoped<postFavoriteGame>();
builder.Services.AddScoped<FavoriteHandler>();
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<UpdateGameHandler>();

builder.Services.AddScoped<getSearchGames>();

builder.Services.AddScoped<DatabaseConecction>();
builder.Services.AddScoped<DeleteProxJuegoHandler>();
builder.Services.AddScoped<DeleteJuegoHandler>();



builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseCors("AllowReact");

app.UseStaticFiles(); // Esto habilita wwwroot por defecto para cargar las imagenes de la carpeta uploads

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
