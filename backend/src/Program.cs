using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using backend.src.Data;
using backend.src.Repositories.Interfaces;
using backend.src.Repositories;
using backend.src.Services;
using backend.src.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configurar a conexão com o SQL Server
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependências
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ClienteService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// Middleware global de tratamento de erros
app.UseErrorHandling();

app.UseAuthorization();

app.MapControllers();

app.Run();
