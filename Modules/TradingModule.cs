using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Modules
{
    /*

    This template is to easily create new commands. Keep it commented out, and copy and paste it where needed.

    [Command("", RunMode = RunMode.Async)]
    [Summary("")]
    [RequireBegan()]
    public async Task COMMANDNAMECommand()
    {
    var sb = new StringBuilder();
    var embed = new EmbedBuilder();
    var user = Context.User;
    sb.AppendLine($"");
    embed.Title = "";
    embed.Description = sb.ToString();
    embed.WithColor(new Color(247, 89, 213));
    await ReplyAsync(null, false, embed.Build());
    }
    */
    [Group("Trade")]
    [Alias("t")]
    public class TradingModule : InteractiveBase<SocketCommandContext>
    {
        BaseModule bm = new BaseModule();
         
        [Command("Pokemon", RunMode = RunMode.Async)]
        [Alias("p")]
        [Summary("Allows you to trade Pokemon with another player! Usage: >trade pokemon [ID of your Pokemon to be traded] [Ping the user you'd like to trade with]")]
        [RequireBegan()]
        public async Task TradePokemonCommand(string pokemonId, string target)
        {
            ulong partnerId = 0;
            Pokemon userTarget = new Pokemon();
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            List<Pokemon> userRemove = new List<Pokemon>();
            Player player = bm.PlayerLoad(user.Id);
            bool validId = Int32.TryParse(pokemonId, out int id);
            if (validId)
            {
                if (target != null)
                {
                    IReadOnlyCollection<SocketUser> mentions = Context.Message.MentionedUsers;
                    partnerId = mentions.FirstOrDefault().Id;
                    bool validSelection = false;
                    foreach (Pokemon p in player.pokemon)
                    {
                        if (p.id == id)
                        {
                            userTarget = p;
                            userRemove.Add(p);
                            validSelection = true;
                        }
                    }
                    if (validSelection)
                    {
                        sb.AppendLine($"ID #{userTarget.id}: {userTarget.nickname} (Species: {userTarget.pkmnName}) Selected. <@{partnerId}>, If you want to trade, then respond with the ID of the Pokemon you want to trade!");
                        embed.Title = $"Pokemon Selected by {player.name}";
                    }
                    else
                    {
                        sb.AppendLine($"Please only enter the ID of a Pokemon that you own!");
                        embed.Title = "Pokemon Not Found";
                    }
                }
                else
                {
                    sb.AppendLine($"You didn't mention someone to trade with!");
                    embed.Title = "No Trading Partner";
                }
            }
            else
            {
                sb.AppendLine($"Please enter a valid ID using digits (0-9)!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            IUserMessage msg = await ReplyAsync(null, false, embed.Build());
            bool userHasAnswered = false;
            bool validResponse = false;
            TimeSpan timeout = new TimeSpan(0, 0, 60);
            while (!userHasAnswered && DateTimeOffset.Now - msg.Timestamp <= timeout)
            {
                var response = await NextMessageAsync();
                if (response != null)
                {
                    if (response.Author.Id == partnerId)
                    {
                        bool isInt = Int32.TryParse(response.Content, out int chosenInt);
                        if (isInt)
                        {
                            List<Pokemon> partnerRemove = new List<Pokemon>();
                            userHasAnswered = true;
                            Pokemon partnerTarget = new Pokemon();
                            Player partner = bm.PlayerLoad(partnerId);
                            foreach (Pokemon x in partner.pokemon)
                            {
                                if (x.id == chosenInt)
                                {
                                    partnerRemove.Add(x);
                                    partnerTarget = x;
                                    validResponse = true;
                                }
                            }
                            if (validResponse)
                            {
                                sb = new StringBuilder();
                                embed = new EmbedBuilder();
                                sb.AppendLine($"S<@{user.Id}>, do you accept this trade? Type y or yes if you do, or anything else if you don't.");
                                embed.Description = sb.ToString();
                                embed.WithColor(new Color(247, 89, 213));
                                embed.Title = $"Accept Trade?";
                                msg = await ReplyAsync(null, false, embed.Build());
                                userHasAnswered = false;
                                validResponse = false;
                                timeout = new TimeSpan(0, 0, 60);
                                while (!userHasAnswered && DateTimeOffset.Now - msg.Timestamp <= timeout)
                                {
                                    response = await NextMessageAsync();
                                    if (response != null)
                                    {
                                        if (response.Author.Id == partnerId)
                                        {
                                            if (response.Content.ToLower() == "yes" || response.Content.ToLower() == "y")
                                            {
                                                foreach (Pokemon y in partnerRemove)
                                                {
                                                    partner.pokemon.Remove(y);
                                                }
                                                foreach (Pokemon y in userRemove)
                                                {
                                                    player.pokemon.Remove(y);
                                                }
                                                int partnerTargetID = partnerTarget.id;
                                                int userTargetID = userTarget.id;
                                                partnerTarget.id = userTargetID;
                                                partnerTarget.originalId = userTargetID;
                                                userTarget.id = partnerTargetID;
                                                userTarget.originalId = partnerTargetID;
                                                partnerTarget.trainer = player;
                                                userTarget.trainer = partner;
                                                player.pokemon.Add(partnerTarget);
                                                partner.pokemon.Add(userTarget);
                                                bm.PlayerSave(player);
                                                bm.PlayerSave(partner);
                                                embed = new EmbedBuilder();
                                                sb = new StringBuilder();
                                                sb.AppendLine($"Congratulations! {player.name} traded their {userTarget.nickname} for {partner.name}'s {partnerTarget.nickname}!");
                                                embed.Description = sb.ToString();
                                                embed.WithColor(new Color(247, 89, 213));
                                                embed.Title = $"Trade Successful!";
                                                await ReplyAsync(null, false, embed.Build());
                                            }
                                            else
                                            {
                                                embed = new EmbedBuilder();
                                                sb = new StringBuilder();
                                                sb.AppendLine($"Trade has been declined. Sorry!");
                                                embed.Description = sb.ToString();
                                                embed.WithColor(new Color(247, 89, 213));
                                                embed.Title = $"Trade Cancelled";
                                                await ReplyAsync(null, false, embed.Build());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        embed = new EmbedBuilder();
                                        sb = new StringBuilder();
                                        sb.AppendLine($"Sorry, you're out of time! Try again later!");
                                        embed.Description = sb.ToString();
                                        embed.WithColor(new Color(247, 89, 213));
                                        embed.Title = $"Trade Failed";
                                        await ReplyAsync(null, false, embed.Build());
                                    }
                                }
                            }
                            else
                            {
                                embed = new EmbedBuilder();
                                sb = new StringBuilder();
                                sb.AppendLine($"Sorry, you don't have a Pokemon with that ID! Please try again later!");
                                embed.Description = sb.ToString();
                                embed.WithColor(new Color(247, 89, 213));
                                embed.Title = $"Pokemon Not Found!";
                                await ReplyAsync(null, false, embed.Build());
                                validResponse = false;
                                break;
                            }
                        }
                        else
                        {
                            embed = new EmbedBuilder();
                            sb = new StringBuilder();
                            sb.AppendLine($"Sorry, that's not a valid ID! Try again later.");
                            embed.Description = sb.ToString();
                            embed.WithColor(new Color(247, 89, 213));
                            embed.Title = $"Pokemon Not Found!";
                            await ReplyAsync(null, false, embed.Build());
                            validResponse = false;
                            break;
                        }
                    }
                }
                else
                {
                    embed = new EmbedBuilder();
                    sb = new StringBuilder();
                    sb.AppendLine($"Sorry, you're out of time! Try again later!");
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    embed.Title = $"Trade Failed";
                    await ReplyAsync(null, false, embed.Build());
                }
            }
        }

        [Command("Loan", RunMode = RunMode.Async)]
        [Alias("l")]
        [Summary("Lets you loan out a pokemon with the ID of your choice to a person you specify for an amount of minutes that you specify! Usage: >trade loan [Pokemon ID] [Time to loan in minutes] [Ping the user you'd like to loan it to!")]
        [RequireBegan()]
        public async Task TradeLoanCommand(string pokemonId, string timeInMinutes, string target)
        {
            ulong partnerId = 0;
            Player partner = new Player();
            Pokemon userTarget = new Pokemon();
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            List<Pokemon> userRemove = new List<Pokemon>();
            Player player = bm.PlayerLoad(user.Id);
            bool validId = Int32.TryParse(pokemonId, out int id);
            if (validId)
            {
                IReadOnlyCollection<SocketUser> mentions = Context.Message.MentionedUsers;
                partnerId = mentions.FirstOrDefault().Id;
                partner = bm.PlayerLoad(partnerId);
                bool validSelection = false;
                foreach (Pokemon p in player.pokemon)
                {
                    if (p.id == id)
                    {
                        userTarget = p;
                        userRemove.Add(p);
                        validSelection = true;
                    }
                }
                if (validSelection)
                {
                    bool validTime = Int32.TryParse(timeInMinutes, out int mins);
                    if (validTime)
                    {
                        TimeSpan loanTime = new TimeSpan(0, mins, 0);
                        userTarget.timeToLoan = loanTime;
                        userTarget.isLoaned = true;
                        userTarget.whenLoaned = DateTimeOffset.Now;
                        List<int> allIds = new List<int>();
                        foreach (Pokemon p in partner.pokemon)
                        {
                            allIds.Add(p.id);
                        }
                        int largestId = 0;
                        if (allIds.Count > 0)
                        {
                            largestId = allIds.Max();
                        }
                        userTarget.id = largestId + 1;
                        partner.pokemon.Add(userTarget);
                        foreach (Pokemon p in userRemove)
                        {
                            player.pokemon.Remove(p);
                        }
                        bm.PlayerSave(partner);
                        bm.PlayerSave(player);
                        sb.AppendLine($"{player.name} has loaned their {userTarget.nickname} to {partner.name} for {mins} Minutes!");
                        embed.Title = $"Pokemon Loaned";
                    }
                    else
                    {
                        sb.AppendLine($"The number of minutes you specified wasn't valid! Please only use digits (0-9)!");
                        embed.Title = $"Invalid Time";
                    }
                }
                else
                {
                    sb.AppendLine($"You don't own a Pokemon with that ID!");
                    embed.Title = $"Pokemon Not Found";
                }
            }
            else
            {
                sb.AppendLine($"That ID wasn't a valid ID! Please only enter digits (0-9).");
                embed.Title = $"Invalid ID";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Give", RunMode = RunMode.Async)]
        [Alias("g")]
        [Summary("Allows you to give a Pokemon to another player! Usage: >trade give [ID of Pokemon] [Ping the player you'd like to give it to!]")]
        [RequireBegan()]
        public async Task TradeGiveCommand(string pokemonId, string target)
        {
            ulong partnerId = 0;
            Player partner = new Player();
            Pokemon userTarget = new Pokemon();
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            List<Pokemon> userRemove = new List<Pokemon>();
            Player player = bm.PlayerLoad(user.Id);
            bool validId = Int32.TryParse(pokemonId, out int id);
            if (validId)
            {
                IReadOnlyCollection<SocketUser> mentions = Context.Message.MentionedUsers;
                partnerId = mentions.FirstOrDefault().Id;
                partner = bm.PlayerLoad(partnerId);
                bool validSelection = false;
                foreach (Pokemon p in player.pokemon)
                {
                    if (p.id == id)
                    {
                        userTarget = p;
                        userRemove.Add(p);
                        validSelection = true;
                    }
                }
                if (validSelection)
                {
                    List<int> allIds = new List<int>();
                    foreach (Pokemon p in partner.pokemon)
                    {
                        allIds.Add(p.id);
                    }
                    int largestId = 0;
                    if (allIds.Count > 0)
                    {
                        largestId = allIds.Max();
                    }
                    userTarget.id = largestId;
                    userTarget.trainer = partner;
                    partner.pokemon.Add(userTarget);
                    foreach (Pokemon p in userRemove)
                    {
                        player.pokemon.Remove(p);
                    }
                    allIds = new List<int>();
                    foreach (Pokemon p in player.pokemon)
                    {
                        allIds.Add(p.id);
                    }
                    if (allIds.Count > 0)
                    {
                        largestId = allIds.Max();
                    }
                    foreach (Pokemon p in player.pokemon)
                    {
                        if (p.id == largestId)
                        {
                            p.id = id;
                            p.originalId = id;
                        }
                    }
                    bm.PlayerSave(partner);
                    bm.PlayerSave(player);
                    sb.AppendLine($"{player.name} has given their {userTarget.nickname} to {partner.name}!");
                    embed.Title = $"Pokemon Gifted";
                }
                else
                {
                    sb.AppendLine($"You don't own a Pokemon with that ID!");
                    embed.Title = $"Pokemon Not Found";
                }
            }
            else
            {
                sb.AppendLine($"That ID wasn't a valid ID! Please only enter digits (0-9).");
                embed.Title = $"Invalid ID";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }
    }
}
