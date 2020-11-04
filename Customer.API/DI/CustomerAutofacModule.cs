using Autofac;
using Customer.API.Data;

namespace Customer.API.DI
{
    public class CustomerAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EFCustomersDataProvider>()
                .As<ICustomersDataProvider>()
                .InstancePerLifetimeScope();
        }
    }
}