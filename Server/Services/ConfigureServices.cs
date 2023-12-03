using Microsoft.EntityFrameworkCore;
using Server.DataBase;
using Server.Middlewares;

namespace Server.Services
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();

           
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DbConnect"));
            });

            return services;
        }
    }
}
