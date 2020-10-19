using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HuiswerkBot.TypeReaders;
using Microsoft.Extensions.Configuration;

namespace HuiswerkBot.Services
{
    public class Startup
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public Startup(IServiceProvider provider, DiscordSocketClient client, CommandService commands, IConfigurationRoot config)
        {
            this._provider = provider;
            this._client = client;
            this._commands = commands;
            this._config = config;
        }

        public async Task StartAsync()
        {
            string discordToken = this._config["token"];
            if (string.IsNullOrWhiteSpace(discordToken)) throw new Exception("Please enter your bot's token into _config.json");

            await this._client.LoginAsync(TokenType.Bot, discordToken);
            await this._client.StartAsync();

            this._client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.CompletedTask;
            };

            //_commands.AddTypeReader(typeof(string), new StringTypeReader());
            await this._commands.AddModulesAsync(Assembly.GetEntryAssembly(), this._provider);
        }
    }
}
