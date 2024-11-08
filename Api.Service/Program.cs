using Infrastructure;
using Infrastructure.Repository.Sample;
using Microsoft.EntityFrameworkCore;
using ApplicationServices.Sample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<SampleRepository>();
builder.Services.AddScoped<ISampleRepository, SampleRepository>();
builder.Services.AddScoped<ISampleService, SampleService>();

string connectionString = Environment.GetEnvironmentVariable("sqlconn")
                          ?? throw new InvalidOperationException("Database connection string not set.");

builder.Services.AddDbContext<SalesPlatformDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();