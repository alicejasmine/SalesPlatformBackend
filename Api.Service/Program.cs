using ApplicationServices;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Service;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.json", optional: false)
                              .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                              .AddEnvironmentVariables();

        builder.Services.AddServices();
        builder.Services.AddRepositories();
 
        builder.Services.AddDbContext();
         builder.Services.AddCosmosDb();

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
    }
}