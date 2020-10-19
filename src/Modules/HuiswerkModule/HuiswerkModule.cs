using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using HuiswerkBot.Services;
using HuiswerkBot.Modules;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace HuiswerkBot.Modules
{
    [Group("hw")]
    public class HuiswerkModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _command;
        private readonly IServiceProvider _services;
        
        public HuiswerkModule(IServiceProvider services)
        {
            
            this._command = services.GetRequiredService<CommandService>();
            this._services = services;
        }

        /// <summary>
        /// Adds homework to 'huiswerk database
        /// </summary>
        /// <returns></returns>
        [Command("add")]
        [Summary("Add homework to the database")]
        public async Task AddHomework(string subject, string deadline, params string[] description)
        {
            // ReSharper disable once InconsistentNaming
            string _description = "";
            foreach (string s in description) _description += s + " ";

            // Console.WriteLine(_description);
            await Services.Database.Insert(subject, _description, deadline, DateTime.Now.AddDays(7), this.Context.User.Username, authorAvatar: this.Context.User.GetAvatarUrl());
        }

        /// <summary>
        /// Retrieves homework from 'huiswerk' database
        /// </summary>
        /// <returns></returns>
        [Command("see")]
        [Summary("Retrieves from the huiswerk database")]
        public async Task SeeHomework(int amount = 5)
        {
            HuiswerkList hw = await Services.Database.Read();
            foreach (Huiswerk huiswerk in hw)
            {
                EmbedBuilder builder = new EmbedBuilder()
                {
                    //Title = "Opdrachten",
                    ThumbnailUrl = (string)huiswerk.AuthorAvatar,
                    Fields =
                    {
                        new EmbedFieldBuilder
                        {
                            Name = "Vak",
                            Value = huiswerk.Subject,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Deadline",
                            Value = huiswerk.Deadline,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Made By",
                            Value = huiswerk.Author,
                            IsInline = true
                        },
                        new EmbedFieldBuilder
                        {
                            Name = "Description",
                            Value = huiswerk.Description,
                            IsInline = false
                        },
                    },
                    Color = new Color(0),
                };
                await this.Context.Channel.SendMessageAsync("", false, builder.Build());
            }
        }
    }
}