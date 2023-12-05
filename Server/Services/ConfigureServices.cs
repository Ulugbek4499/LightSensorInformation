using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.DataBase;
using Server.Middlewares;
using Swashbuckle.AspNetCore.Filters;

namespace Server.Services
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder => builder.AddConsole());
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSingleton<GlobalExceptionHandlingMiddleware>();

            services.AddHttpLogging(x =>
            {
                x.LoggingFields = HttpLoggingFields.ResponseStatusCode
                   | HttpLoggingFields.ResponseHeaders
                   | HttpLoggingFields.ResponseBody;
            });

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                 .GetBytes(configuration.GetSection("Jwt:Token").Value)),
                             ValidateIssuer = false,
                             ValidateAudience = false
                         };
                     });

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DbConnect"));
            });

            return services;
        }
    }
}
