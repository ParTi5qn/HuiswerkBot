using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace HuiswerkBot.Services
{
    public class MessageHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public MessageHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _client.MessageReceived += MessageRecievedAsync;
            Console.WriteLine("MessageHandler started!");
        }

        public async Task MessageRecievedAsync(SocketMessage socketMessage)
        {
            await Task.Run( () => Console.WriteLine(socketMessage.Content));
        }

        private async Task MessageRecieved(SocketMessage socketMessage)
        {
            await Task.Run( () => 
            {
                Console.WriteLine(socketMessage.Content);
            });
        }
    }
}