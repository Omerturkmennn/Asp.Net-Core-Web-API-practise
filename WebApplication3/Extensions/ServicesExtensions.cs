using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFcore;
using Services.Contracts;
using Services;
using Entities1.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace WebApplication3.Extensions
{
    public static class ServicesExtensions
    {
       
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        }

       
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
          
            services.AddScoped<IRepositoryManager, RepositoryManager>();



        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }

       
        public static void ConfigureLoggerService(this IServiceCollection services) 
        { 
            services.AddSingleton<ILoggerService,LoggerManager>();
        }

        public static void ConfigureCors(this IServiceCollection services) 
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",builder =>  
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination")
                );

            });
        }

        public static void ConfigureDataShaper(this IServiceCollection services) 
        {
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>(); 
        }


        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
               
                var systemTextJsonOutputFormatter = config
                    .OutputFormatters
                    .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault(); 

                if (systemTextJsonOutputFormatter is not null)
                {
                    
                    systemTextJsonOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.omerturkmen.hateoas+json");
                }

                var xmlOutputFormatter= config
                .OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

                if(xmlOutputFormatter is not null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.omerturkmen.hateoas+json");
                }
            });
        }

    }




}