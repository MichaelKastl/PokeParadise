using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Attributes
{
    public class RequireTriviaCooldownAttribute : PreconditionAttribute
    {
        BaseModule bm = new BaseModule();
        public RequireTriviaCooldownAttribute()
        {

        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            Player player = bm.PlayerLoad(context.User.Id);
            if (DateTimeOffset.Now - player.lastTrivia > new TimeSpan(0, 5, 0))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                TimeSpan diff = new TimeSpan(0, 5, 0) - (DateTimeOffset.Now - player.lastTrivia);
                return Task.FromResult(PreconditionResult.FromError($"Sorry, you won Trivia recently! You need to wait another **{diff.Minutes}m{diff.Seconds}s** before you can play Trivia again!"));
            }
        }
    }
}
