using WebApi.Options.Base;

namespace WebApi.ExtensionMethods;

public static class OptionsConfiguration
{
    public static IServiceCollection AddAppOptions<TAppOptions>(
        this IServiceCollection services,
        IConfiguration configuration
    )
        where TAppOptions : AppOptions, new()
    {
        TAppOptions appOptions = new();
        appOptions.Bind(configuration: configuration);

        services.AddSingleton(implementationInstance: appOptions);

        return services;
    }
}
