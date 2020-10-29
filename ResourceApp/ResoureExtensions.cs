using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ResourceApp.Resources;
using System.Globalization;

namespace ResourceApp
{
    public static class ResoureExtensions
    {
        public static IServiceCollection RegisterResoure(this IServiceCollection services)
        {
            //first step
            services.AddMvc()
     .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
     .AddDataAnnotationsLocalization(options =>
    options.DataAnnotationLocalizerProvider = (type, factory) =>
             factory.Create(typeof(SharedResources))
     );
            //second step
            services.Configure<RequestLocalizationOptions>(o =>
            {
                var supportedCultures = new[]
                {
          new CultureInfo("en-US"),
          new CultureInfo("fr-FR")
        };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                o.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
                o.SupportedCultures = supportedCultures;
                o.SupportedUICultures = supportedCultures;

                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
                o.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
            });
            return services;
        }

        public static IApplicationBuilder RegisAppBuilderLocalization(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            return app;
        }
    }
}