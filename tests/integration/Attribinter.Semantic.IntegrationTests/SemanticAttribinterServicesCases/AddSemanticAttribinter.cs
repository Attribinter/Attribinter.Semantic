namespace Attribinter.Semantic.SemanticAttribinterServicesCases;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Moq;

using System;

using Xunit;

public sealed class AddSemanticAttribinter
{
    private static IServiceCollection Target(IServiceCollection services) => SemanticAttribinterServices.AddSemanticAttribinter(services);

    private readonly IServiceProvider ServiceProvider;

    public AddSemanticAttribinter()
    {
        HostBuilder host = new();

        host.ConfigureServices(static (services) => Target(services));

        ServiceProvider = host.Build().Services;
    }

    [Fact]
    public void NullServiceCollection_ArgumentNullException()
    {
        var exception = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void ValidServiceCollection_ReturnsSameServiceCollection()
    {
        var serviceCollection = Mock.Of<IServiceCollection>();

        var actual = Target(serviceCollection);

        Assert.Same(serviceCollection, actual);
    }

    [Fact]
    public void ISemanticTypeArgumentParser_ServiceCanBeResolved() => ServiceCanBeResolved<ISemanticTypeArgumentParser>();

    [Fact]
    public void ISemanticConstructorArgumentParser_ServiceCanBeResolved() => ServiceCanBeResolved<ISemanticConstructorArgumentParser>();

    [Fact]
    public void ISemanticNamedArgumentParser_ServiceCanBeResolved() => ServiceCanBeResolved<ISemanticNamedArgumentParser>();

    [AssertionMethod]
    private void ServiceCanBeResolved<TService>() where TService : notnull
    {
        var service = ServiceProvider.GetRequiredService<TService>();

        Assert.NotNull(service);
    }
}
