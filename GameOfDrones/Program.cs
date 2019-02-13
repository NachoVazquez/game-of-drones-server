using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameOfDrones.DataAccess.Contexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace GameOfDrones
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.RollingFile(Path.Combine("logs", "log-{HalfHour}.txt"))
                //.WriteTo.MSSqlServer()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                IWebHost webHost = CreateWebHostBuilder(args).Build();

                // Create a new scope
                using (var scope = webHost.Services.CreateScope())
                {
                    // Get the DbContext instance
                    var myDbContext = scope.ServiceProvider.GetRequiredService<GameOfDronesContext>();

                    //Do the migration asynchronously
                    await myDbContext.Database.MigrateAsync();
                }

                await webHost.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}