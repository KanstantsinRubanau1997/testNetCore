
using GenericHosts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetStandartLib;

Console.WriteLine("[.Net Standart text] " + Constants.SomeText);

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.SetBasePath(Directory.GetCurrentDirectory());
        configHost.AddJsonFile("hostsettings1.json");
        configHost.AddEnvironmentVariables(prefix: "PREFIX_");
    })
    .UseConsoleLifetime()
    .ConfigureServices((_, services) =>
    {
        services.AddTransient<IService, Service1>();
        services.AddScoped<IScopedService, ScopedService>();
        services.AddSingleton<ISingletonService, SingletonService>();
        services.AddHostedService<ExampleHostedService>();
    })
    .Build();

host.Services.GetService<IService>().WriteMessage();
host.Services.GetService<IScopedService>().WriteMessage();
host.Services.GetService<ISingletonService>().WriteMessage();

var host1Task = host.RunAsync();

using IHost host2 = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.SetBasePath(Directory.GetCurrentDirectory());
        configHost.AddJsonFile("hostsettings2.json");
        configHost.AddEnvironmentVariables(prefix: "PR_");
        configHost.AddCommandLine(args);
    })
    .ConfigureServices((_, services) =>
    {
        services.AddTransient<IService, Service2>();
        services.AddScoped<IScopedService, ScopedService>();
        services.AddSingleton<ISingletonService, SingletonService>();
        services.AddHostedService<ExampleHostedService>();
    })
    .Build();

var host2Task = host2.RunAsync();

using IHost host3 = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.Sources.Clear();

        configHost.SetBasePath(Directory.GetCurrentDirectory());
        configHost.AddEnvironmentVariables(prefix: "NOT_EXISTING_PREFIX_");
        configHost.AddJsonFile("hostsettings1.json");

        configHost.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["InMemoryKey"] = "In momnory value",
            });
    })
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<IService, Service2>();
        services.AddScoped<IScopedService, ScopedService>();
        services.AddSingleton<ISingletonService, SingletonService>();
        services.AddHostedService<ExampleHostedService>();

        services.AddOptions<ExampleHostedService.ExampleHostedServiceOptions>()
            .BindConfiguration("ExampleHostedServiceOptions")
            .ValidateDataAnnotations();
    })
    .Build();

var host3Task = host3.RunAsync();

await Task.WhenAll(host1Task, host2Task, host3Task);