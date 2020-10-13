using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace HuisWerkBot.TypeReaders
{
    public class StringTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (input is string msg)
            {
                return Task.FromResult(TypeReaderResult.FromSuccess(true));
            }
            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as a string."));
        }
    }
}
