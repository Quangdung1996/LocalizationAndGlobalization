using Autofac;
using Autofac.Extensions.DependencyInjection;
using Customer.API.Data;
using Customer.API.DI;
using Customer.API.Middleware;
using Domain.LocalizationExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QD.Swagger.Extensions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace Customer.API
{
    public class Startup
    {
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllers();
            services.AddOptions();
            services.AddDbContext<EFCustomersDataProvider>(options => options.UseInMemoryDatabase("CustomerDataDb"));
            services.AddSingleton(new ResourceManager("Customer.API.Resources.Controllers.CustomersController", typeof(Startup).GetTypeInfo().Assembly));

            services.AddSwaggerForApiDocs("'Customer.API v'VVVV", options => { });
            services.AddResources();
            return CreateAutofacServiceProvider(services);
        }

        private IServiceProvider CreateAutofacServiceProvider(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.RegisterModule(new CustomerAutofacModule());

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var timingLogger = loggerFactory.CreateLogger("Customer.API.Startup.TimingMiddleware");
            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                var timer = new Stopwatch();
                timer.Start();

                await next();
                timer.Stop();
                timingLogger.LogInformation("Request to {RequestMethod}:{RequestPath} processed in {ElapsedMilliseconds} ms", context.Request.Method, context.Request.Path, timer.ElapsedMilliseconds);
            });

            var supportedCultures = new[]{ new CultureInfo("en-US"),
                                               new CultureInfo("fr-FR"),
                                               new CultureInfo("ja-JP"),
                                               new CultureInfo("ko-KR") };

            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US", "en-US"),

                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseHttpsRedirection();

            app.UseRequestLocalization(requestLocalizationOptions);

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSwaggerForApiDocs("BizAction APIs", false);

            app.UseMiddleware<RequestCorrelationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}