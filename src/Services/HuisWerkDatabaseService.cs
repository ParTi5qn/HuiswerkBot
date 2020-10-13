using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HuiswerkBot.Database;
using MySql.Data;
using MySql.Data.MySqlClient;
using HuiswerkBot.Modules;

namespace HuiswerkBot.Services
{
    public class HuiswerkDatabaseService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly IConfigurationRoot _config;

        private static DatabaseHandler _dbHandler;

        public HuiswerkDatabaseService(IServiceProvider services, IConfigurationRoot config)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _commands = services.GetRequiredService<CommandService>();
            _config = config;
            _services = services;
            
            _dbHandler = new DatabaseHandler(_config); 
        }

        internal static async Task CreateConnection()
        {
            if(_dbHandler.Connected)
            {
                Console.WriteLine("Connection already established");
                return;
            }
            await _dbHandler.OpenAsync();
            Console.WriteLine("CreateConnection executed");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="description"></param>
        /// <param name="deadline"></param>
        /// <param name="finised_date"></param>
        /// <param name="author"></param>
        /// <param name="author_group"></param>
        /// <param name="finised"></param>
        /// <returns></returns>
        internal static async Task Insert(string subject, string description, DateTime deadline, DateTime finised_date, string author, string author_group = "", string author_avatar = "", bool finised = false)
        {
            if (deadline == null) deadline = DateTime.Now;
            await _dbHandler.Insert(subject, description, deadline, finised_date, author,  author_group, author_avatar, finised);
        }
        
        internal static async Task<HuiswerkList> Read(int amount = 5)
        {
            return await _dbHandler.Read(amount);
        }
    }
}