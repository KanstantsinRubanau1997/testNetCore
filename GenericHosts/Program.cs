using GenericHosts;
using GenericHosts.BackgroundService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreLib;
using NetFrameworkLib;
using NetStandartLib;
using static GenericHosts.ScopedService;
using static GenericHosts.SingletonService;

Console.WriteLine("[.Net Standart text] " + Constants.SomeText);
Console.WriteLine("[.Net Framework text] " + FrameworkLIbConstants.GetSmth());
Console.WriteLine("[.Net Core with Framework link text] " + CoreLibConstants.GetSmth());

var isBackgroundService = true;

if (!isBackgroundService)
{
    using IHost host = Host.CreateDefaultBuilder()
    .ConfigureHostConfiguration(configHost =>
    {
        configHost.SetBasePath(Directory.GetCurrentDirectory());
        configHost.AddJsonFile("hostsettings1.json");
        configHost.AddEnvironmentVariables(prefix: "PREFIX_");

        configHost.AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["Counter"] = "1",
            });
    })
    .ConfigureLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();
    })
    .ConfigureServices((_, services) =>
    {
        services.AddTransient<IService, Service1>();
        services.AddScoped<IScopedService, ScopedService>();
        services.AddSingleton<ISingletonService, SingletonService>();
        services.AddHostedService<ExampleHostedService>();
    })
    .Build();

    var host1Task = host.RunAsync();

    Thread.Sleep(1000);

    using IHost host2 = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(configHost =>
        {
            configHost.SetBasePath(Directory.GetCurrentDirectory());
            configHost.AddJsonFile("hostsettings2.json");
            configHost.AddEnvironmentVariables(prefix: "PR_");
            configHost.AddCommandLine(args);

            configHost.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Counter"] = "2",
                });
        })
        .ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConsole();
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

    Thread.Sleep(1000);

    using IHost host3 = Host.CreateDefaultBuilder(args)
        .ConfigureHostConfiguration(configHost =>
        {
            configHost.Sources.Clear();

            configHost.SetBasePath(Directory.GetCurrentDirectory());
            configHost.AddEnvironmentVariables(prefix: "NOT_EXISTING_PREFIX_");

            configHost.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Counter"] = "3",
                    ["InMemoryKey"] = "In memnory value",
                });

            configHost.AddJsonFile("hostsettings1.json", false, true);
        })
        .ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConsole();
        })
        .ConfigureServices((context, services) =>
        {
            services.AddOptions<ExampleHostedService.ExampleHostedServiceOptions>()
                .BindConfiguration(nameof(ExampleHostedService.ExampleHostedServiceOptions))
                .ValidateDataAnnotations();

            services.AddOptions<ScopedServiceOptions>().BindConfiguration(nameof(ScopedServiceOptions));
            services.AddOptions<SingletonServiceOptions>().BindConfiguration(nameof(SingletonServiceOptions));

            services.AddTransient<IService, Service2>();
            services.AddScoped<IScopedService, ScopedService>();
            services.AddSingleton<ISingletonService, SingletonService>();
            services.AddHostedService<ExampleHostedService>();
        })
        .Build();
    var host3Task = host3.RunAsync();

    Thread.Sleep(1000);

    using IHost scoupedServicesHost = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(configHost =>
        {
            configHost.AddJsonFile("hostsettings2.json", false, true);
        })
        .ConfigureHostConfiguration(configHost =>
        {
            configHost.AddJsonFile("hostsettings1.json", false, true);
        })
        .ConfigureServices((_, services) =>
        {
            services.AddOptions<SingletonServiceOptions>().BindConfiguration(nameof(SingletonServiceOptions));

            services.AddTransient<IService, Service1>();
            services.AddScoped<IScopedService, ScopedService>();
            services.AddSingleton<ISingletonService, SingletonService>();

            services.AddHostedService<ScopedExampleHostedService>();
        })
        .ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConsole();
        })
        .Build();
    var scoupedServicesHostTask = scoupedServicesHost.RunAsync();

    Thread.Sleep(1000);

    using IHost unsetupedHost = new HostBuilder()
        .ConfigureHostConfiguration(configHost =>
        {
            configHost.SetBasePath(Directory.GetCurrentDirectory());
            configHost.AddJsonFile("hostservice_clear.json");
            configHost.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Counter"] = "Clear"
                });
        })
        .ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConsole();
        })
        .ConfigureServices((_, services) =>
        {
            services.AddTransient<IService, Service1>();
            services.AddScoped<IScopedService, ScopedService>();
            services.AddSingleton<ISingletonService, SingletonService>();
            services.AddHostedService<ExampleHostedService>();
        })
        .Build();
    var unsetupedHostTask = unsetupedHost.RunAsync();

    Thread.Sleep(1000);

    using IHost unsetupedHost2 = new HostBuilder()
        .ConfigureHostConfiguration(configHost =>
        {
            configHost.SetBasePath(Directory.GetCurrentDirectory());
            configHost.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Counter"] = "Clear",
                    ["SomeValue"] = "Some value InMemoryCollection"
                });
            configHost.AddJsonFile("hostservice_clear.json");
        })
        .ConfigureLogging((context, loggingBuilder) =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConfiguration(context.Configuration.GetSection("Logging:Console"));
            loggingBuilder.AddConsole(q => q.IncludeScopes = true);
            loggingBuilder.AddDebug();

            //loggingBuilder.AddFilter("CustomCategory", logLevel => logLevel == LogLevel.Trace);
        })
        .ConfigureServices((_, services) =>
        {
            services.AddHostedService<ClearExampleHostedService>();
        })
        .Build();
    var unsetupedHostTask2 = unsetupedHost2.RunAsync();

    await host1Task;
    await host2Task;
    await host3Task;
    await scoupedServicesHostTask;
    await unsetupedHostTask;
    await unsetupedHostTask2;
}
else
{
    using IHost timedBackgroundHost = new HostBuilder()
    .ConfigureLogging((context, loggingBuilder) =>
    {
        loggingBuilder.AddConsole();
    })
    .ConfigureServices((_, services) =>
    {
        services.AddHostedService<TimedBackgroundExampleHostedService>();
    }).Build();
    var timedBackgroundHostTask = timedBackgroundHost.RunAsync();

    using IHost backgroundHost = new HostBuilder()
        .ConfigureLogging((context, loggingBuilder) =>
        {
            loggingBuilder.AddConsole();
        })
        .ConfigureServices((_, services) =>
        {
            services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

            services.AddHostedService<ConsumeScopedServiceHostedService>();
        }).Build();
    var backgroundHostTask = backgroundHost.RunAsync();

    using IHost queuedBackgroundHost = new HostBuilder()
        .ConfigureLogging((context, loggingBuilder) =>
        {
            loggingBuilder.AddConsole();
        })
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<MonitorLoop>();
            services.AddSingleton<IBackgroundTaskQueue>(_ => new BackgroundTaskQueue(100));

            services.AddHostedService<QueuedHostedService>();
        }).Build();

    var monitorLoop = queuedBackgroundHost.Services.GetRequiredService<MonitorLoop>();
    monitorLoop.StartMonitorLoop();

    var queuedBackgroundHostTask = queuedBackgroundHost.RunAsync();

    await timedBackgroundHostTask;
    await backgroundHostTask;
    await queuedBackgroundHostTask;
}
