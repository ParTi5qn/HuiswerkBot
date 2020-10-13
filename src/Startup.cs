using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HuiswerkBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    IConfigurationRoot Configuration { get; }

   public Startup(string[] args)
   {
       IConfigurationBuilder builder = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("_config.json");

        Configuration= builder.Build();
   }

    public static async Task RunAsync(string[] args)
    {
        Startup startup = new Startup(args);
        await startup.RunAsync();
    }

    public async Task RunAsync()
    {
        ServiceCollection services = new ServiceCollection();
        ConfigureServices(services);

        ServiceProvider provider = services.BuildServiceProvider();
        provider.GetRequiredService<StatusService>();
        provider.GetRequiredService<LoggingService>();
        provider.GetRequiredService<CommandHandlerService>();
        provider.GetRequiredService<MessageHandlerService>();
        provider.GetRequiredService<HuiswerkDatabaseService>();


        await provider.GetRequiredService<StartupService>().StartAsync();
        await Task.Delay(-1);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services
        .AddSingleton<StatusService>()
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton<CommandService>()
        .AddScoped<CommandHandlerService>()
        .AddSingleton<StartupService>()
        .AddSingleton<LoggingService>()
        .AddSingleton<MessageHandlerService>()
        .AddSingleton<HuiswerkDatabaseService>()
        .AddSingleton<Random>()
        .AddSingleton(Configuration);
    }
}