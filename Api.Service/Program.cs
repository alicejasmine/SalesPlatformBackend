using ApplicationServices;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();       
builder.Services.AddRepositories();  
builder.Services.AddDbContext();
//Environment.SetEnvironmentVariable("sqlconn", "Server=localhost,1433;Database=SalesPlatformDB;User Id=sa;Password=Suits0811;TrustServerCertificate=True;");
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