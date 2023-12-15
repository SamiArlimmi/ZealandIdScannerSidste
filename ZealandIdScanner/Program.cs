using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZealandIdScanner.EBbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using static ZealandIdScanner.Models.Sensors;
//using ZealandIdScanner.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ZealandIdContext>();
//builder.Services.AddSingleton<DbService, DbService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
var connectionString = configuration.GetConnectionString("DefaultConnectionSQLAuth");
builder.Services.AddDbContext<ZealandIdContext>(options => 
    options.UseSqlServer(connectionString));


// Add CORS

var allowAll = "AllowAll";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowAll, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(allowAll);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();