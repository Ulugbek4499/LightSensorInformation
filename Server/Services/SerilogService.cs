using Serilog;

namespace Server.Services;

public class SerilogService
{
    public static void SerilogSettings(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/log.txt")
            .CreateLogger();
    }
}
