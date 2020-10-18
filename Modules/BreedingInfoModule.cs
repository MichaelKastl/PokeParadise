using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Modules
{
    [Group("BreedingInfo")]
    [Alias("binfo", "bi")]
    public class BreedingInfoModule : InteractiveBase<SocketCommandContext>
    {
        BaseModule bm = new BaseModule();

        [Command("Pokemon", RunMode = RunMode.Async)]
        [Alias("pkmn", "p")]
        [Summary("Allows you to see a list of all Pokemon who can breed with the Pokemon with the specified ID! Usage: >breedinginfo pokemon [ID of Pokemon]")]
        [RequireBegan()]
        public async Task BreedingInfoCommand(string pokemonId)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            bool validId = Int32.TryParse(pokemonId, out int id);
            if (validId)
            {
                bool hasPokemon = false;
                Pokemon p = new Pokemon();
                foreach (Pokemon x in player.pokemon)
                {
                    if (x.id == id) { hasPokemon = true; p = x; }
                }
                if (hasPokemon)
                {
                    EmbedFieldBuilder efb = new EmbedFieldBuilder();
                    List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
                    IEnumerable<Pokemon> playerPokes = player.pokemon.OrderBy(x => x.id);
                    foreach (Pokemon x in playerPokes)
                    {
                        if (x.eggGroup1 == p.eggGroup1 || x.eggGroup2 == p.eggGroup1 || x.eggGroup1 == p.eggGroup2 || x.eggGroup2 == p.eggGroup2)
                        {
                            efb.Name = $"**#{x.id}: {x.nickname}**";
                            string group1 = x.eggGroup1.Substring(0, 1).ToUpper() + x.eggGroup1.Substring(1);
                            string group2 = x.eggGroup2;
                            if (x.eggGroup2 == null || x.eggGroup2 == "" || x.eggGroup2 == " ") { group2 = "None"; } else { group2 = x.eggGroup2.Substring(0, 1).ToUpper() + x.eggGroup2.Substring(1); }
                            efb.Value = $"Egg Group 1: {group1}; Egg Group 2: {group2}";
                            pages.Add(efb);
                            efb = new EmbedFieldBuilder();
                        }
                    }
                    PaginatedMessage msg = new PaginatedMessage();
                    msg.Color = new Color(247, 89, 213);
                    msg.Pages = pages;
                    msg.Options.Timeout = null;
                    msg.Title = $"#{p.id}: {p.nickname}'s Compatible Breeding Partners";
                    await PagedReplyAsync(msg);
                }
                else
                {
                    sb.AppendLine($"You don't own a Pokemon with the ID {id}!");
                    embed.Title = "Invalid Input";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());
                }
            }
            else
            {
                sb.AppendLine($"You didn't enter a valid number for your Pokemon's ID! Please enter a digit (0-9)!");
                embed.Title = "Invalid Input";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }

        [Command("All", RunMode = RunMode.Async)]
        [Summary("Shows you a paginated list of all of your Pokemon and all of the Pokemon you own that they can breed with! Usage: >breedinginfo all")]
        [RequireBegan()]
        public async Task BreedingInfoAllCommand()
        {
            List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            IEnumerable<Pokemon> playerPokes = player.pokemon.OrderBy(x => x.id);
            foreach (Pokemon p in playerPokes)
            {
                EmbedFieldBuilder efb = new EmbedFieldBuilder();
                efb.Name = $"**#{p.id}: {p.nickname}'s Possible Partners**";
                string desc = "";
                foreach (Pokemon x in playerPokes)
                {
                    if (x.eggGroup1 == p.eggGroup1 || x.eggGroup2 == p.eggGroup1 || x.eggGroup1 == p.eggGroup2 || x.eggGroup2 == p.eggGroup2)
                    {
                        desc += $"#{x.id}: {x.nickname}, ";
                    }
                    if (desc == "") { desc = "None"; }
                }
                efb.Value = desc;
                pages.Add(efb);
                efb = new EmbedFieldBuilder();
            }
            PaginatedMessage msg = new PaginatedMessage();
            msg.Color = new Color(247, 89, 213);
            msg.Pages = pages;
            msg.Title = $"{player.name}'s Pokemon Breeding Compatibility List";
            msg.Options.Timeout = null;
            await PagedReplyAsync(msg);
        }
    }
}
