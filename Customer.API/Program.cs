using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Customer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
           .UseStartup<Startup>()
            .ConfigureAppConfiguration(builder =>
            {
                //builder.AddJsonFile("appsettings.json");

            }).ConfigureLogging(ConfigureLogging);


        private static void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder loggingBuilder)
        {
            var env = context.HostingEnvironment;
            var config = context.Configuration;

            loggingBuilder.AddDebug();

            if (!env.IsDevelopment())
            {
               
            }
            else
            {
                var serilogLogger = new LoggerConfiguration()
                                    .ReadFrom.Configuration(config)
                                    .WriteTo.ApplicationInsightsTraces(config["ApplicationInsights:InstrumentationKey"])
                                    .CreateLogger();

                loggingBuilder.AddSerilog(serilogLogger);
            }
        }
    }
}