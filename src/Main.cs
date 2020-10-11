using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using HuisWerkBot.Services;
using System.Reflection;

namespace HuisWerkBot
{
    public class MainHuisWerk
    {
        public static void Main(string[] args)
            => Startup.RunAsync(args).GetAwaiter().GetResult(); 
    }
}