namespace Attribinter.Semantic;

using Microsoft.Extensions.DependencyInjection;

using System;

/// <summary>Allows the services of <i>Attribinter</i> to be registered with <see cref="IServiceCollection"/>.</summary>
public static class SemanticAttribinterServices
{
    /// <summary>Registers the services of <i>Attribinter</i> with the provided <see cref="IServiceCollection"/>.</summary>
    /// <param name="services">The <see cref="IServiceCollection"/> with which services are registered.</param>
    /// <returns>The provided <see cref="IServiceCollection"/>, so that calls can be chained.</returns>
    public static IServiceCollection AddSemanticAttribinter(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<ISemanticTypeArgumentParser, SemanticTypeArgumentParser>();
        services.AddSingleton<ISemanticConstructorArgumentParser, SemanticConstructorArgumentParser>();
        services.AddSingleton<ISemanticNamedArgumentParser, SemanticNamedArgumentParser>();

        return services;
    }
}
