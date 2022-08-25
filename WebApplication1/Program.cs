
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database;

// КОДИРОВКА ФАЙЛОВ ДОЛЖНА БЫТЬ BASE64 (в поле DATA),
// по сети передаётся в виде строки

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// провайдер БД - EF, БД - PostgreSQL
string connectionString = builder.Configuration.GetSection("ConnectionString").Value;
builder.Services.AddDbContext<BaseContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
