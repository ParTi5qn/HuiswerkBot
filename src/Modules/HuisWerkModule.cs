using System.Threading.Tasks;
using Discord.Commands;
using MySql.Data;
using MySql.Data.MySqlClient;
using HuisWerkBot.Services;
using System;
using Discord;

namespace HuisWerkBot.Modules
{
    [Group("hw")]
    public class HuisWerkModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        
        public HuisWerkModule(CommandService service)
        {
            _service = service;
        }

        /// <summary>
        /// Adds homework to 'huiswerk database
        /// </summary>
        /// <returns></returns>
        [Command("add")]
        [Summary("Add homework to the database")]
        public async Task addHomework(string huiswerkText){
           await HuisWerkDatabaseService.CreateConnection();
           await HuisWerkDatabaseService.Insert(huiswerkText, Context.User.Username);
        }

        /// <summary>
        /// Retrives homework from 'huiswerk database
        /// </summary>
        /// <returns></returns>
        [Command("see")]
        [Summary("Retrieves from the huiswerk database")]
        public async Task seeHomework(){
            EmbedBuilder builder = await HuisWerkDatabaseService.Read();
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}