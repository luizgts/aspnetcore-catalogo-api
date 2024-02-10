using System.Text.Json.Serialization;
using CatalogoApi.Context;
using CatalogoApi.Extensions;
using CatalogoApi.Filter;
using CatalogoApi.Filters;
using CatalogoApi.Logging;
using CatalogoApi.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Obtendo a String de Conexão
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Obtendo valores do arquivo appsettings.json
var hosts = builder.Configuration["AllowedHosts"];

// Registra o contexto do banco na injeção de dependências do asp.net
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseMySql( mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

// Registrar o filtro de exceções de forma global
// Resolve o problema da referência cíclica durante a serialização do JSON
builder.Services
    .AddControllers(options => options.Filters.Add(typeof(ApiExceptionFilter)))
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrando um filtro personalizado
builder.Services.AddScoped<ApiLoggingFilter>();

// Registrando o Logger personalizado
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration {
    LogLevel = LogLevel.Information
}));

// Registro do serviço de repositório
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Middleware para tratamento global de exceptions
    app.ConfigureExceptionHandle();
}

// Adicionando um Middleware ao Pipeline
app.Use(async (context, next) => {

    Console.WriteLine("Middleware: Antes do Request");
    await next(context);
    Console.WriteLine("Middleware: depois do Request");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
