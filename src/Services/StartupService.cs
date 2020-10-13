using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HuisWerkBot.TypeReaders;
using Microsoft.Extensions.Configuration;

namespace HuiswerkBot.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public StartupService(IServiceProvider provider, DiscordSocketClient client, CommandService commands, IConfigurationRoot config)
        {
            _provider = provider;
            _client = client;
            _commands = commands;
            _config = config;
        }

        public async Task StartAsync()
        {
            string discordToken = _config["token"];
            if (string.IsNullOrWhiteSpace(discordToken)) throw new Exception("Please enter your bot's token into _config.json");

            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();

            _client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.CompletedTask;
            };

            //_commands.AddTypeReader(typeof(string), new StringTypeReader());
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}
