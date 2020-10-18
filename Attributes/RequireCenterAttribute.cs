using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Attributes
{
    public class RequireCenterAttribute : PreconditionAttribute
    {
        BaseModule bm = new BaseModule();
        public RequireCenterAttribute()
        {

        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            Player player = bm.PlayerLoad(context.User.Id);
            if (player.breedingCenterUpgrades >= 1)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"You have to buy a Breeding Center before you can start breeding!"));
            }
        }
    }
}
