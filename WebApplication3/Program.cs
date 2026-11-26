
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Repositories.EFcore;
using Services;
using Services.Contracts;
using WebApplication3.Extensions;


namespace WebApplication3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            LogManager.Setup().LoadConfigurationFromFile(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            // Add services to the container.

            builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter=true;

            });



            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebApplication3", Version = "v1" });
                s.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebApplication3", Version = "v2" });
                s.OperationFilter<ApiVersionHeaderOperationFilter>();
            });

            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureLoggerService();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.ConfigureCors();
            builder.Services.ConfigureDataShaper();
            builder.Services.AddCustomMediaTypes();
            builder.Services.ConfigureActionFilters();
            builder.Services.AddScoped<IBookLinks, BookLinks>();
            builder.Services.ConfigureVersioning();
            builder.Services.ConfigureResponseCaching();
            builder.Services.ConfigureHttpCacheHeaders();
            builder.Services.AddMemoryCache();
            builder.Services.ConfigureRateLimitingOptions();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthentication();
            builder.Services.ConfigureIdentity();
          

            var app = builder.Build();

            var logger=app.Services.GetRequiredService<ILoggerService>();
            app.ConfigureExceptionHandler(logger);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication3 v1");
                    s.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApplication3 v2");
                });
            }

            if (app.Environment.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseIpRateLimiting();
            app.UseCors("CorsPolicy");
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            app.UseAuthentication();    
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
