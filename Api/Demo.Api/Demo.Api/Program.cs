using Demo.Api.Data;
using Demo.Api.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DemoDbContext>(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    x.UseSqlServer(connectionString);
});
//builder.Services.Configure<RabbitMqOptions>(Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<RabbitMqListener>();
builder.Services.AddHostedService<RabbitMqHostedService>();

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
