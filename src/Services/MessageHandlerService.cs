using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace HuiswerkBot.Services
{
    public class MessageHandlerService
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public MessageHandlerService(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _client.MessageReceived += MessageRecieved;
            _client.MessageReceived += MessageRecievedAsync;
            Console.WriteLine("MessageHandler started!");
        }

        public async Task MessageRecievedAsync(SocketMessage rawMessage)
        {
            await Task.Run( () => rawMessage.Content);
        }

        private async Task MessageRecieved(SocketMessage socketMessage)
        {
            await Task.Run( () => 
            {
                Console.WriteLine(socketMessage.ToString());
            });
        }
    }
}