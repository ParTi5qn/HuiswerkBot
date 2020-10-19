using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HuiswerkBot.Modules;
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
        provider.GetRequiredService<Status>();
        provider.GetRequiredService<Logging>();
        provider.GetRequiredService<CommandHandler>();
        provider.GetRequiredService<MessageHandler>();
        provider.GetRequiredService<Database>();


        await provider.GetRequiredService<HuiswerkBot.Services.Startup>().StartAsync();
        await Task.Delay(-1);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton<CommandService>()
        .AddSingleton<Status>()
        .AddSingleton<CommandHandler>()
        .AddSingleton<HuiswerkBot.Services.Startup>()
        .AddSingleton<Logging>()
        .AddSingleton<MessageHandler>()
        .AddSingleton<Database>()
        .AddSingleton<Random>()
        .AddSingleton(this.Configuration);
    }
}