using Domain.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.LocalizationExtensions
{
    public static class LocalizationRegistry
    {
        public static void AddResources(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerDataTransferObjectShared, CustomerDataTransferObjectLocalizer>();
        }
    }
}