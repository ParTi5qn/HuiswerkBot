using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using Serilog.Core;

namespace HuiswerkBot.Services
{
    public class Logging
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        // private readonly Logger log; 

        public Logging(IServiceProvider services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("C:\\mylog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Horny");

            this._client = services.GetRequiredService<DiscordSocketClient>();
            this._services = services;
            
            this.Start();
        }

        public Task Start()
        {
#pragma warning disable 4014
            this.InstallLoggingServiceAsync();
#pragma warning restore 4014
            Log.CloseAndFlush();
            return Task.CompletedTask;
        }

        public async Task InstallLoggingServiceAsync(Func<LogMessage, Task> handler = null)
        {
            await Task.Run( () => this._client.Log += LoggingAsync);
        }

        private static async Task LoggingAsync(LogMessage logMessage)
        {
            await Task.Run(() =>
            {

            });
        }

        public async Task Write(string logMessage)
        {
            await Task.Run(() => Log.Information(logMessage));
        }


    }
}