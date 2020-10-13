using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace HuiswerkBot.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public LoggingService(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            
            Start();
        }

        public async Task Start()
        {
            await InstallLoggingServiceAsync();
        }

        public async Task InstallLoggingServiceAsync(Func<LogMessage, Task> handler = null)
        {
            await Task.Run( () => _client.Log += LoggingAsync);
        }

        private async Task LoggingAsync(LogMessage logMessage)
        {
            await Task.Run( () => 
            {
                Console.WriteLine(logMessage.ToString());
            });
        }
    }
}