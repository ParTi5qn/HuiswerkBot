using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HuiswerkBot.Services;

namespace HuiswerkBot.Modules
{
    /// <summary>
    /// All commands without a prefix
    /// </summary>
    [Name("Basic commands")]
    public class BasicCommandsModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;

        public BasicCommandsModule(CommandService service)
        {
            this._service = service;
        }

        [Command("help")]
        [Summary("Display help")]
        public async Task Help()
        {
            EmbedBuilder builder = new EmbedBuilder();
            foreach (ModuleInfo module in this._service.Modules)
            {
                Console.WriteLine(module.Name);
                foreach (CommandInfo commandInfo in module.Commands)
                {
                    Console.WriteLine(commandInfo.Name);
                    builder.AddField(commandInfo.Name, commandInfo.Summary);
                }
            }
            await ReplyAsync("", false, builder.Build());
        }

        /// <summary>
        /// Sends a message back with the input from the user
        /// </summary>
        /// <param name="input"></param>
        [Command("echo")]
        [Summary("Says exactly what you say")]
        public async Task Echo([Remainder] string input)
        {
            await Context.Channel.SendMessageAsync(input);
        }

        /// <summary>
        /// Sends information about the bot.
        /// </summary>
        [Command("status")]
        [Summary("Display status of discord bot")]
        public async Task Status()
        {
            DateTime runtime = await Services.Status.GetRuntime();
            string uptime = await Services.Status.GetUpTime(runtime);
            string reply = $"```Bot uptime is: {uptime}```";

            await this.Context.Channel.SendMessageAsync(reply);
        }
    }
}