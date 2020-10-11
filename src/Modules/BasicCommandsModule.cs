using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HuisWerkBot.Services;

namespace HuisWerkBot.Modules
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
            _service = service;
        }
        
        [Command("help")]
        [Summary("Display help")]
        public async Task Help()
        {
            EmbedBuilder builder = new EmbedBuilder()
            {
                Color = new Color(106, 102, 163),
                Description = "These are the commands you may use"
            };

            foreach(ModuleInfo module in _service.Modules)
            {
                string description = "";
                foreach(CommandInfo command in module.Commands)
                {
                    PreconditionResult result = await command.CheckPreconditionsAsync(Context);
                    if(result.IsSuccess)
                    {
                        description += $"!{command.Aliases.First()}\n";
                    }

                    if(!string.IsNullOrWhiteSpace(description))
                    {
                        builder.AddField(x =>
                        {
                            x.Name = module.Name;
                            x.Value = description;
                            x.IsInline = true;
                        });
                    }
                }
                await ReplyAsync("", false, builder.Build());
            }
        }

        /// <summary>
        /// Sends information about the bot.
        /// </summary>
        [Command("status")]
        [Summary("Display status of discord bot")]
        public async Task Status()
        {
            DateTime runtime = await StatusService.GetRuntime();
            string uptime = await StatusService.GetUpTime(runtime);
            string reply = $"```Bot uptime is: {uptime}.```";
            
            await Context.Channel.SendMessageAsync(reply);
        }
    }
}