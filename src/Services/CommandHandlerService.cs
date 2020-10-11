using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace HuisWerkBot.Services
{
    public class CommandHandlerService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public CommandHandlerService(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _commands = services.GetRequiredService<CommandService>();
            _services = services;
            
            _commands.CommandExecuted += CommandExecutedAsync;

            Start();
        }

        public async Task Start()
        {
            await InstallCommandAsync();
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if(!command.IsSpecified)
            return;

            if(result.IsSuccess)
            return;

            await context.Channel.SendMessageAsync($"error: {result}"); 
        }

        public async Task InstallCommandAsync()
        {
            // Hook the MessageRecieved event into command handler 
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process command if it was a system message
            var message = messageParam as SocketUserMessage;
            if(message == null) return;

            // Create a number to track where the prefix number ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure not bots trigger commands
            if(!(message.HasCharPrefix('!', ref argPos)) || message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.Author.IsBot) return;

            var context = new SocketCommandContext(_client, message);

            var result = await _commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: _services
            );
        }
    }
}