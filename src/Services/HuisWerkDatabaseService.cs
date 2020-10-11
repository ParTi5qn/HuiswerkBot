using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HuisWerkBot.Database;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace HuisWerkBot.Services
{
    public class HuisWerkDatabaseService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly IConfigurationRoot _config;

        private static DatabaseHandler _dbHandler;

        public HuisWerkDatabaseService(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _commands = services.GetRequiredService<CommandService>();
            _config = services.GetRequiredService<IConfigurationRoot>();
            _services = services;
            
            _dbHandler = new DatabaseHandler(_config); 
        }

        public static async Task CreateConnection()
        {
            if(_dbHandler.isConnected)
            {
                Console.WriteLine("Connection already established");
                return;
            }
            await _dbHandler.Connect();
            Console.WriteLine("CreateConnection executed");
        }

        public static async Task Insert(string Huiswerk, string Username)
        {
            await _dbHandler.InsertHuiswerk(Huiswerk, Username);
        }
        
        public static async Task<EmbedBuilder> Read()
        {
            return await _dbHandler.GetHuiswerk();
        }
    }
}