using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Attributes
{
    public class NotPlayingTriviaAttribute : PreconditionAttribute
    {
        BaseModule bm = new BaseModule();
        public NotPlayingTriviaAttribute()
        {

        }
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            Player player = bm.PlayerLoad(context.User.Id);
            if (!player.isDoingTrivia)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"Nice try! You can't access Pokedex or Pokemon Info commands while you're playing Trivia!"));
            }
        }
    }
}
