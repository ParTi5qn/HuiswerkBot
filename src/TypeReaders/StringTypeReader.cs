using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace HuiswerkBot.TypeReaders
{
    public class StringTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            return Task.FromResult(input != null ? TypeReaderResult.FromSuccess(true) : TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as a string."));
        }
    }
}
