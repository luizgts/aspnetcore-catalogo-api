using System.Text.Json.Serialization;
using CatalogoApi.Context;
using CatalogoApi.DTOs.Mapping;
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

// Habilita o filtro de exceções Global
// Habilita a correção da referência cíclica durante a serialização do JSON
// Habilita o suporte ao JSON Patch
builder.Services
    .AddControllers(options => options.Filters.Add(typeof(ApiExceptionFilter)))
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilita filtro personalizado
builder.Services.AddScoped<ApiLoggingFilter>();

// Habilita Logger personalizado
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration {
    LogLevel = LogLevel.Information
}));

// Habilita o serviço de repositório
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
// builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Habilita o AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

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
