
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Repositories.EFcore;
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
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureLoggerService();
            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            var logger=app.Services.GetRequiredService<ILoggerService>();
            app.ConfigureExceptionHandler(logger);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Environment.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
