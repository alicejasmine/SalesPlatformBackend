using ApplicationServices;

namespace Api.Service;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServices();
        services.AddRepositories();

        services.AddDbContext(Configuration);
        services.AddCosmosDb(Configuration);

        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SupportNonNullableReferenceTypes();
            c.EnableAnnotations();
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsStaging()) app.UseDeveloperExceptionPage();

        app.UseSwagger();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapSwagger();
        });

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
