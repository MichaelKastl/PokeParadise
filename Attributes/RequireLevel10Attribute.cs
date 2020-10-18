using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Attributes
{
    public class RequireLevel10Attribute : PreconditionAttribute
    {
        BaseModule bm = new BaseModule();
        public RequireLevel10Attribute()
        {

        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            Player player = bm.PlayerLoad(context.User.Id);
            if (player.level >= 5)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"You have to at least be Level 5 to buy the Breeding Center!"));
            }
        }
    }
}
