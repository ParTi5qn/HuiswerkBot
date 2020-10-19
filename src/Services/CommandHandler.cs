using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Diagnostics;

namespace HuiswerkBot.Services
{
    public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            this._client = services.GetRequiredService<DiscordSocketClient>();
            this._commands = services.GetRequiredService<CommandService>();
            this._services = services;
            
            this._commands.CommandExecuted += this.CommandExecutedAsync;

            this.Start();
        }

        public async Task Start()
        {
            await InstallCommandAsync();
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if(!command.IsSpecified) return;

            if (result.IsSuccess)
            {
                await context.Channel.SendMessageAsync("Command executed successfully");
                return;
            }

            await context.Channel.SendMessageAsync($"error: {result}"); 
        }

        public async Task InstallCommandAsync()
        {
            // Hook the MessageRecieved event into command handler 
            this._client.MessageReceived += this.HandleCommandAsync;

            await this._commands.AddModulesAsync(Assembly.GetEntryAssembly(), this._services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process command if it was a system message
            var message = messageParam as SocketUserMessage;
            if(message == null) return;

            // Create a number to track where the prefix number ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure not bots trigger commands
            if(!(message.HasCharPrefix('!', ref argPos)) || message.HasMentionPrefix(this._client.CurrentUser, ref argPos) || message.Author.IsBot) return;

            var context = new SocketCommandContext(this._client, message);
            

            IResult result = await this._commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: this._services
            );

            if (result.IsSuccess) Console.WriteLine($"{messageParam.Content} executed successfully");

        }
    }
}