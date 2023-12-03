using MediatR;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Server.DataBase;
using Server.Middlewares;

namespace Server.Services
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder => builder.AddConsole()); 
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddSingleton<GlobalExceptionHandlingMiddleware>();

            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DbConnect"));
            });

            return services;
        }
    }
}
