using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using HuiswerkBot.Services;
using HuisWerkBot.Modules;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace HuiswerkBot.Modules
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
        public async Task AddHomework(string subject, DateTime deadline, params string[] description)
        {
            string _d = "";
            foreach (string s in description)
            {
                _d += s + " ";
            }

            Console.WriteLine(_d);
            await HuiswerkDatabaseService.Insert(subject, _d, deadline, DateTime.Now.AddDays(7), Context.User.Username, author_avatar: Context.User.GetAvatarUrl());
        }

        /// <summary>
        /// Retrives homework from 'huiswerk database
        /// </summary>
        /// <returns></returns>
        [Command("see")]
        [Summary("Retrieves from the huiswerk database")]
        public async Task SeeHomework(int amount = 5)
        {
            HuiswerkList hw = await HuiswerkDatabaseService.Read();
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

                await Context.Channel.SendMessageAsync("", false, builder.Build());
            }

        }
    }
}