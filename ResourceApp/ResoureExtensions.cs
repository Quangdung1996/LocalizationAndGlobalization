using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace ResourceApp
{
    public static class ResoureExtensions
    {
        public static IServiceCollection RegisterResoure(this IServiceCollection services)
        {
            //first step
            services.AddLocalization(o =>
            {
                o.ResourcesPath = "";
            });
            services.AddMvc()
              .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              .AddDataAnnotationsLocalization();

            //services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization(options =>
            //options.DataAnnotationLocalizerProvider = (type, factory) =>
            //factory.Create(typeof(SharedResources))
            //);
            services.Configure<RequestLocalizationOptions>(o =>
            {
                var supportedCultures = new[]{ new CultureInfo("en-US"),
                                               new CultureInfo("fr-FR"),
                                               new CultureInfo("ja-JP"),
                                               new CultureInfo("ko-KR")
            };

                o.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
                o.SupportedCultures = supportedCultures;
                o.SupportedUICultures = supportedCultures;

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