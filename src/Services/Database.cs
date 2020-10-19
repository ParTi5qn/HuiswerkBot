using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
using Microsoft.Win32.SafeHandles;

namespace HuiswerkBot.Services
{
    public class Database
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly IConfigurationRoot _config;

        private static DatabaseHandler _dbHandler;

        public Database(IServiceProvider services, IConfigurationRoot config)
        {
            this._client = services.GetRequiredService<DiscordSocketClient>();
            this._commands = services.GetRequiredService<CommandService>();
            this._config = config;
            this._services = services;

            _dbHandler = new DatabaseHandler(this._services, this._config); 
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
        /// Calls DatabaseHandler.Insert() with the correct parameters.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="description"></param>
        /// <param name="deadline"></param>
        /// <param name="finishedDate"></param>
        /// <param name="author"></param>
        /// <param name="authorGroup"></param>
        /// <param name="authorAvatar"></param>
        /// <param name="finished"></param>
        /// <returns></returns>
        internal static async Task Insert(string subject, string description, string deadline, DateTime finishedDate, string author, string authorGroup = "", string authorAvatar = "", bool finished = false)
        {
            // if (deadline.GetType() != typeof(DateTime)) deadline = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            await _dbHandler.Insert(subject, description, deadline, finishedDate, author,  authorGroup, authorAvatar, finished);
        }
        
        internal static async Task<HuiswerkList> Read(int amount = 5)
        {
            return await _dbHandler.Read(amount);
        }
    }
}