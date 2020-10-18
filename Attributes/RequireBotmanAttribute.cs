using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Attributes
{
    class RequireBotmanAttribute : PreconditionAttribute
    {
        BaseModule bm = new BaseModule();
        public RequireBotmanAttribute()
        {

        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            SocketGuildUser gUser = context.User as SocketGuildUser;
            if (gUser.Roles.Any(r => r.Name == "Botman"))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"You need to be the Botman for that!"));
            }
        }
    }
}
