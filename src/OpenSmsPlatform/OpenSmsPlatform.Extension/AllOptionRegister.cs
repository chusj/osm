using Microsoft.Extensions.DependencyInjection;
using OpenSmsPlatform.Common.Option;

namespace OpenSmsPlatform.Extension
{
    public static class AllOptionRegister
    {
        public static void AddAllOptionRegister(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            foreach (var optionType in typeof(ConfigurableOptions).Assembly.GetTypes()
                .Where(s => !s.IsInterface && typeof(IConfigurableOptions).IsAssignableFrom(s)))
            {
                services.AddConfigurableOptions(optionType);
            }
        }
    }
}
