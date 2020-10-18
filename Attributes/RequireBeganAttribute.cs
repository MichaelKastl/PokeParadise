using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise
{
    public class RequireBeganAttribute : PreconditionAttribute
    {
        BaseModule bm = new BaseModule();
        public RequireBeganAttribute()
        {

        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            Player player = bm.PlayerLoad(context.User.Id);
            if (player.name != null)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"Please begin your journey first with the >begin command! Don't forget to give us your name, when you do!"));
            }
        }
    }
}
