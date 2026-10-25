using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFcore;
using Services.Contracts;
using Services;

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


    }




}