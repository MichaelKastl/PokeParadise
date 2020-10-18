using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using PokeApiNet;
using PokeParadise;
using PokeParadise.Attributes;
using PokeParadise.Classes;
using PokeParadise.Services;

namespace PokeParadise.Modules
{
    [Group("Pokemon")]
    [Alias("pkmn", "p")]
    public class PkmnManagementModule : InteractiveBase<SocketCommandContext>
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

        readonly BaseModule bm = new BaseModule();
        [Group("List")]
        public class PkmnListModule : InteractiveBase<SocketCommandContext>
        {
            PkmnManagementModule pmm = new PkmnManagementModule();
            readonly BaseModule bm = new BaseModule();
            [Command("", RunMode = RunMode.Async)]
            [Summary("This command lists all of your currently-obtained Pokemon! Usage: >pkmn list")]
            [RequireBegan()]
            public async Task ListCommand()
            {
                var sb = new StringBuilder();
                var embed = new EmbedFieldBuilder();
                var user = Context.User;
                Player player = bm.PlayerLoad(user.Id);
                int pageCnt = 1;
                int leftOff = 0;
                int totalPages = Convert.ToInt32(Math.Ceiling(player.pokemon.Count / 10m));
                IEnumerable<Pokemon> pokes = player.pokemon.OrderBy(x => x.id);
                List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
                while (pageCnt <= totalPages)
                {
                    int cnt = 1;
                    foreach (Pokemon p in pokes)
                    {
                        if (p.id > leftOff)
                        {
                            embed = new EmbedFieldBuilder();
                            string nickname = p.nickname;
                            if (p.nickname == null)
                            {
                                nickname = p.pkmnName;
                            }
                            embed.Name = $"**Pokemon #{p.id}: {p.nickname}**";
                            embed.Value = $"Species: {p.pkmnName}, Level: {p.level}";
                            cnt++;
                            leftOff = p.id;
                            pages.Add(embed);
                        }
                    }
                    pageCnt++;
                }
                PaginatedMessage message = new PaginatedMessage
                {
                    Pages = pages,
                    Title = $"{player.name}'s Pokemon List",
                    Color = new Color(247, 89, 213)
                };
                message.Options.Timeout = null;
                await PagedReplyAsync(message);
            }

            [Command("EggGroups", RunMode = RunMode.Async)]
            [Alias("egg", "egs")]
            [Summary("This command lists all of your Pokemon with their Egg Groups! Usage: >pkmn list eggroups")]
            [RequireBegan()]
            public async Task ListEggGroupsCommand()
            {
                var sb = new StringBuilder();
                var embed = new EmbedFieldBuilder();
                var user = Context.User;
                Player player = bm.PlayerLoad(user.Id);
                int pageCnt = 1;
                int leftOff = 0;
                int totalPages = Convert.ToInt32(Math.Ceiling(player.pokemon.Count / 10m));
                IEnumerable<Pokemon> pokes = player.pokemon.OrderBy(x => x.id);
                List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
                while (pageCnt <= totalPages)
                {
                    int cnt = 1;
                    foreach (Pokemon p in pokes)
                    {
                        if (p.id > leftOff)
                        {
                            embed = new EmbedFieldBuilder();
                            string nickname = p.nickname;
                            if (p.nickname == null)
                            {
                                nickname = p.pkmnName;
                            }
                            string group1 = p.eggGroup1.Substring(0, 1).ToUpper() + p.eggGroup1.Substring(1);
                            string group2 = "None";
                            if (p.eggGroup2 != null && p.eggGroup2 != "" && p.eggGroup2 != " ")
                            {
                                group2 = p.eggGroup2.Substring(0, 1).ToUpper() + p.eggGroup2.Substring(1);
                            }
                            embed.Name = $"**Pokemon #{p.id}: {p.nickname}**";
                            embed.Value = $"*Egg Group 1: {group1}, Egg Group 2: {group2}*";
                            cnt++;
                            leftOff = p.id;
                            pages.Add(embed);
                        }
                    }
                    pageCnt++;
                }
                PaginatedMessage message = new PaginatedMessage
                {
                    Pages = pages,
                    Title = $"{player.name}'s Pokemon List by Egg Group",
                    Color = new Color(247, 89, 213)
                };
                message.Options.Timeout = null;
                await PagedReplyAsync(message);
            }
        }

        [Group("Moves")]
        [Alias("m")]
        public class PkmnMovesModule : InteractiveBase<SocketCommandContext>
        {
            readonly BaseModule bm = new BaseModule();
            [Command("Learn", RunMode = RunMode.Async)]
            [Alias("l")]
            [Summary("Using this command lets you choose moves from your Pokemon's moveset to assign to one of your four Move Slots! Slots will be filled in order. Usage: >pkmn moves learn [ID of the Pokemon] [ID of the move]")]
            [RequireBegan()]
            public async Task MovesLearnCommand(string pkmnId, string moveId)
            {
                var sb = new StringBuilder();
                var embed = new EmbedBuilder();
                var user = Context.User;
                List<Pokemon> toRemove = new List<Pokemon>();
                bool validPkmnId = Int32.TryParse(pkmnId, out int pId);
                bool validMoveId = Int32.TryParse(moveId, out int mId);
                if (validPkmnId)
                {
                    if (validMoveId)
                    {
                        Player player = bm.PlayerLoad(user.Id);
                        bool hasPokemon = false;
                        Pokemon target = new Pokemon();
                        foreach (Pokemon p in player.pokemon)
                        {
                            if (p.id == pId)
                            {
                                hasPokemon = true;
                                toRemove.Add(p);
                                target = p;
                            }
                        }
                        if (hasPokemon)
                        {
                            target.moveList = await target.GetPokemonMovesetAsync(target.dexNo);
                            if (target.moveList.ContainsKey(mId))
                            {
                                Classes.Move x = target.moveList[mId];
                                bool allFull = true;
                                if (target.moveSet == null) { target.moveSet = new Classes.Move[4]; }
                                if (target.moveSet[0] == null) { allFull = false; target.moveSet[0] = x; }
                                else if (target.moveSet[1] == null) { allFull = false; target.moveSet[1] = x; }
                                else if (target.moveSet[2] == null) { allFull = false; target.moveSet[2] = x; }
                                else if (target.moveSet[3] == null) { allFull = false; target.moveSet[3] = x; }
                                if (allFull == false)
                                {
                                    if (x.learnedLevel <= target.level)
                                    {
                                        string moveName = x.name.Substring(0, 1).ToUpper() + x.name.Substring(1);
                                        foreach (Pokemon z in toRemove)
                                        {
                                            player.pokemon.Remove(z);
                                        }
                                        player.pokemon.Add(target);
                                        bm.PlayerSave(player);
                                        sb.AppendLine($"{target.nickname} has learned the move {moveName}!");
                                        embed.Title = "Success!";
                                    }
                                    else
                                    {
                                        sb.AppendLine($"Your Pokemon is too low-level to learn this move!");
                                        embed.Title = "Too Low Level";
                                    }
                                }
                                else
                                {
                                    sb.AppendLine($"Your Pokemon already knows four moves! Please use the command >pkmn moves replace.");
                                    embed.Title = "Move List Full";
                                }
                            }
                            else
                            {
                                sb.AppendLine($"Either the Move with the ID you specified doesn't exist, or your Pokemon can't learn it! Please try again.");
                                embed.Title = "Move ID Not Found";
                            }
                        }
                        else
                        {
                            sb.AppendLine($"Sorry, it doesn't look like you have a Pokemon with that ID! Please try again.");
                            embed.Title = "Pokemon Not Found";
                        }
                    }
                    else
                    {
                        sb.AppendLine($"The number you entered for the Move ID was not a digit (0-9)!");
                        embed.Title = "Invalid Move ID";
                    }
                }
                else
                {
                    sb.AppendLine($"The number you entered for your Pokemon ID was not a digit (0-9)!");
                    embed.Title = "Invalid Pokemon ID";
                }
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }

            [Command("Replace", RunMode = RunMode.Async)]
            [Alias("r")]
            [Summary("Using this command lets you replace an existing move in your Pokemon's moveset with another move that your Pokemon can learn! Usage: >pkmn moves replace [Pokemon ID] [Move ID] [Slot to Replace]")]
            [RequireBegan()]
            public async Task MovesReplaceCommand(string pkmnId, string moveId, string replaceSlotId)
            {
                var sb = new StringBuilder();
                var embed = new EmbedBuilder();
                var user = Context.User;
                List<Pokemon> toRemove = new List<Pokemon>();
                bool validPkmnId = Int32.TryParse(pkmnId, out int pId);
                bool validMoveId = Int32.TryParse(moveId, out int mId);
                bool validSlotId = Int32.TryParse(replaceSlotId, out int slot);
                slot--; // Because this is zero-indexed. Don't delete!
                if (validPkmnId)
                {
                    if (validMoveId)
                    {
                        if (validSlotId)
                        {
                            if (slot <= 4 && slot > 0)
                            {
                                Player player = bm.PlayerLoad(user.Id);
                                bool hasPokemon = false;
                                Pokemon target = new Pokemon();
                                foreach (Pokemon p in player.pokemon)
                                {
                                    if (p.id == pId)
                                    {
                                        hasPokemon = true;
                                        target = p;
                                        toRemove.Add(p);
                                    }
                                }
                                if (hasPokemon)
                                {
                                    target.moveList = await target.GetPokemonMovesetAsync(target.dexNo);
                                    if (target.moveList.ContainsKey(mId))
                                    {
                                        Classes.Move x = target.moveList[mId];
                                        if (x.learnedLevel <= target.level)
                                        {
                                            string oldMoveName = "None";
                                            if (target.moveSet[slot] != null)
                                            {
                                                oldMoveName = target.moveSet[slot].name.Substring(0, 1).ToUpper() + target.moveSet[slot].name.Substring(1);
                                            }
                                            target.moveSet[slot] = x;
                                            foreach (Pokemon z in toRemove)
                                            {
                                                player.pokemon.Remove(z);
                                            }
                                            player.pokemon.Add(target);
                                            bm.PlayerSave(player);
                                            string moveName = x.name.Substring(0, 1).ToUpper() + x.name.Substring(1);
                                            sb.AppendLine($"{target.nickname} has replaced the move {oldMoveName} with {moveName}!");
                                            embed.Title = "Success!";
                                        }
                                        else
                                        {
                                            sb.AppendLine($"Your Pokemon isn't high enough level to learn this move!");
                                            embed.Title = "Too Low Level!";
                                        }
                                    }
                                    else
                                    {
                                        sb.AppendLine($"Either the Move with the ID you specified doesn't exist, or your Pokemon can't learn it! Please try again.");
                                        embed.Title = "Move ID Not Found";
                                    }
                                }
                                else
                                {
                                    sb.AppendLine($"Sorry, it doesn't look like you have a Pokemon with that ID! Please try again.");
                                    embed.Title = "Pokemon Not Found";
                                }
                            }
                            else
                            {
                                sb.AppendLine($"Sorry, you specified a Slot Number higher than 4 or lower than 1! You can only replace up to 4 slots.");
                                embed.Title = "Invalid Slot Number";
                            }
                        }
                        else
                        {
                            sb.AppendLine($"The number you entered for the Slot to Replace was not a digit (0-9)!");
                            embed.Title = "Invalid Slot Number";
                        }
                    }
                    else
                    {
                        sb.AppendLine($"The number you entered for the Move ID was not a digit (0-9)!");
                        embed.Title = "Invalid Move ID";
                    }
                }
                else
                {
                    sb.AppendLine($"The number you entered for your Pokemon ID was not a digit (0-9)!");
                    embed.Title = "Invalid Pokemon ID";
                }
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }

            [Command("Learnset", RunMode = RunMode.Async)]
            [Alias("ls")]
            [Summary("Allows you to see all of the moves your Pokemon can learn, and at what level they learn them! Usage: >pkmn moves learnset [ID of Pokemon]")]
            [RequireBegan()]
            public async Task MovesLearnsetCommand(string pkmnId)
            {
                var sb = new StringBuilder();
                var embed = new EmbedBuilder();
                var user = Context.User;
                bool validPkmnId = Int32.TryParse(pkmnId, out int pId);
                if (validPkmnId)
                {
                    Player player = bm.PlayerLoad(user.Id);
                    bool hasPokemon = false;
                    Pokemon target = new Pokemon();
                    foreach (Pokemon p in player.pokemon)
                    {
                        if (p.id == pId)
                        {
                            hasPokemon = true;
                            target = p;
                        }
                    }
                    if (hasPokemon)
                    {
                        target.moveList = await target.GetPokemonMovesetAsync(target.dexNo);
                        List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
                        foreach (KeyValuePair<int, Classes.Move> kvp in target.moveList)
                        {
                            EmbedFieldBuilder efb = new EmbedFieldBuilder();
                            string name = kvp.Value.name.Substring(0, 1).ToUpper() + kvp.Value.name.Substring(1);
                            if (name.Contains("-"))
                            {
                                name = name.Substring(0, name.IndexOf("-")) + " " + name.Substring(name.IndexOf("-") + 1, 1).ToUpper() + name.Substring(name.IndexOf("-") + 2);
                                name = name.Replace("-"," ");
                            }
                            sb = new StringBuilder();
                            sb.AppendLine($"Condition: {kvp.Value.condition}; Appeal: {kvp.Value.appeal}; Jam: {kvp.Value.jam};");
                            sb.AppendLine($"Learnable At Level: {kvp.Value.learnedLevel}");
                            sb.AppendLine($"Effect: {kvp.Value.effect}");
                            efb.Value = sb.ToString();
                            efb.Name = $"**ID #{kvp.Value.id}: {kvp.Value.name.Substring(0, 1).ToUpper() + kvp.Value.name.Substring(1)}**";
                            pages.Add(efb);
                        }
                        PaginatedMessage msg = new PaginatedMessage();
                        msg.Pages = pages;
                        msg.Color = new Color(247, 89, 213);
                        msg.Options.Timeout = new TimeSpan(2, 0, 0);
                        msg.Title = $"{target.nickname}'s Learnable Moves";
                        await PagedReplyAsync(msg);
                    }
                    else
                    {
                        sb.AppendLine($"Sorry, it doesn't look like you have a Pokemon with that ID! Please try again.");
                        embed.Title = "Pokemon Not Found";
                        embed.Description = sb.ToString();
                        embed.WithColor(new Color(247, 89, 213));
                        await ReplyAsync(null, false, embed.Build());
                    }
                }
                else
                {
                    sb.AppendLine($"The number you entered for your Pokemon ID was not a digit (0-9)!");
                    embed.Title = "Invalid Pokemon ID";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());
                }
            }

            [Command("Info", RunMode = RunMode.Async)]
            [Alias("i")]
            [Summary("Shows you all the information about any move you specify by ID! Usage: >pkmn moves info [Move ID]")]
            [RequireBegan()]
            public async Task MovesInfoCommand(string moveId)
            {
                var sb = new StringBuilder();
                var embed = new EmbedBuilder();
                bool validId = Int32.TryParse(moveId, out int mId);
                if (validId)
                {
                    Classes.Move m = new Classes.Move();
                    Dictionary<int, Classes.Move> moves = m.FetchMoveDict();
                    bool foundMove = false;
                    Classes.Move move = new Classes.Move();
                    if (moves.ContainsKey(mId))
                    {
                        move = moves[mId];
                    }
                    if (foundMove)
                    {
                        sb.AppendLine($"Contest Condition: {move.condition}; Appeal: {move.appeal}; Jam: {move.jam};");
                        sb.AppendLine($"Effect: {move.effect}");
                        embed.Title = $"ID #{move.id}: {move.name.Substring(0,1).ToUpper() + move.name.Substring(1)}";
                    }
                    else
                    {
                        sb.AppendLine($"No Move with that ID found!");
                        embed.Title = "Move Not Found";
                    }
                }
                else
                {
                    sb.AppendLine($"The Move ID you entered wasn't a digit (0-9)!");
                    embed.Title = "Invalid ID";
                }
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }

        [Command("Info", RunMode = RunMode.Async)]
        [Summary("Shows you the specific information for a Pokemon with the ID or Nickname you specify! Usage: >pkmn info [ID number or nickname]")]
        [RequireBegan()]
        [NotPlayingTrivia()]
        public async Task InfoCommand(string inputId)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            bool isId = true;
            Player player = bm.PlayerLoad(user.Id);
            Pokemon pkmn = new Pokemon();
            List<Pokemon> toRemove = new List<Pokemon>();
            bool validId = Int32.TryParse(inputId, out int id);
            if (!validId)
            {
                inputId = inputId.ToLower();
                foreach (Pokemon p in player.pokemon)
                {
                    if (p.nickname.ToLower() == inputId)
                    {
                        validId = true;
                        isId = false;
                    }
                }
            }
            if (validId)
            {
                bool foundMatch = false;
                foreach (Pokemon p in player.pokemon)
                {
                    if (isId)
                    { 
                        if (p.id == id)
                        {
                            pkmn = p;
                            toRemove.Add(p);
                            foundMatch = true;
                        }
                    }
                    else
                    {
                        if (p.nickname.ToLower() == inputId)
                        {
                            pkmn = p;
                            toRemove.Add(p);
                            foundMatch = true;
                        }
                    }
                }
                if (toRemove.Count <= 1)
                {
                    if (foundMatch)
                    {
                        string type2 = pkmn.type2;
                        if (pkmn.type2 == null || pkmn.type2 == " " || pkmn.type2 == "")
                        {
                            type2 = "None";
                        }
                        string firstLet = pkmn.nature.Substring(0, 1).ToUpper();
                        string nature = firstLet + pkmn.nature.Substring(1);
                        firstLet = pkmn.pkmnName.Substring(0, 1).ToUpper();
                        string species = firstLet + pkmn.pkmnName.Substring(1);
                        firstLet = pkmn.gender.Substring(0, 1).ToUpper();
                        string gender = firstLet + pkmn.gender.Substring(1);
                        Random rand = new Random();
                        if (pkmn.ability == null)
                        {
                            int coinflip = rand.Next(1, 2);
                            if (coinflip == 1)
                            {
                                pkmn.ability = pkmn.speciesAbility1;
                            }
                            else
                            {
                                pkmn.ability = pkmn.speciesAbility2;
                            }
                        }
                        foreach (Pokemon x in toRemove)
                        {
                            player.pokemon.Remove(x);
                        }
                        player.pokemon.Add(pkmn);
                        bm.PlayerSave(player);
                        firstLet = pkmn.ability.Substring(0, 1).ToUpper();
                        string ability = firstLet + pkmn.ability.Substring(1);
                        string isShiny = "No";
                        if (pkmn.isShiny)
                        {
                            isShiny = "Yes";
                        }
                        string nickname = pkmn.nickname;
                        if (pkmn.nickname == null)
                        {
                            nickname = pkmn.pkmnName;
                        }
                        string heldItem = "None";
                        if (pkmn.heldItem != null) { heldItem = pkmn.heldItem.name; }
                        embed.Title = $"ID #{pkmn.id}: {player.name}'s {nickname}";
                        sb.AppendLine($"**Species:** #{pkmn.dexNo} {species}");
                        sb.AppendLine($"**Level:** {pkmn.level}");
                        sb.AppendLine($"**Nature:** {nature}");
                        sb.AppendLine($"**Type 1:** {pkmn.type1}");
                        sb.AppendLine($"**Type 2:** {type2}");
                        sb.AppendLine($"**Gender:** {gender}");
                        sb.AppendLine($"**Ability:** {ability}");
                        sb.AppendLine($"**Experience:** {pkmn.xp}");
                        sb.AppendLine($"**Held Item:** {heldItem}");
                        sb.AppendLine($"**Shiny:** {isShiny}");
                        embed.Description = sb.ToString();
                        string[] paths = new string[10];
                        paths[0] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/official-artwork/";
                        paths[1] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/home-icons/";
                        paths[2] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/anime/";
                        paths[3] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/dream-world/";
                        paths[4] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/mystery-dungeon/";
                        paths[5] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/8bit-sprites/";
                        paths[6] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/gen7-icons/";
                        paths[7] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/gen6-gifs/";
                        paths[8] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/gen4-gifs/";
                        paths[9] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/hgss-sprites/";
                        List<EmbedBuilder> pages = new List<EmbedBuilder>();
                        int cnt = player.imgPref;
                        if (cnt == 10)
                        {
                            cnt = rand.Next(0, 9);
                        }                       
                        var fileName = pkmn.dexNo + ".png";
                        if (cnt == 7 || cnt == 8)
                        {
                            fileName = pkmn.dexNo + ".gif";
                        }
                        string fullPath = Path.Combine(paths[cnt], fileName);
                        bool imgExists = UrlExists(fullPath);
                        if (!imgExists)
                        {
                            fullPath = Path.Combine(paths[player.backupPref], fileName);
                        }
                        if (player.thumbnailPref == 7 || player.thumbnailPref == 8)
                        {
                            fileName = pkmn.dexNo + ".gif";
                        }
                        string thumbPath = Path.Combine(paths[player.thumbnailPref], fileName);
                        bool thumbExists = UrlExists(thumbPath);
                        if (!thumbExists)
                        {
                            fileName = pkmn.dexNo + ".png";
                            thumbPath = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + fileName;
                        }
                        embed.WithImageUrl($"{fullPath}");
                        embed.WithThumbnailUrl($"{thumbPath}");
                        embed.Description = sb.ToString();
                        embed.WithColor(new Color(247, 89, 213));
                        pages.Add(embed);
                        int pageCount = 0;
                        while (pageCount < 9)
                        {
                            embed = new EmbedBuilder();
                            if (pageCount != cnt)
                            {
                                fileName = pkmn.dexNo + ".png";
                                if (pageCount == 7 || pageCount == 8)
                                {
                                    fileName = pkmn.dexNo + ".gif";
                                }
                                fullPath = Path.Combine(paths[pageCount], fileName);
                                if (UrlExists(fullPath))
                                {
                                    embed.ImageUrl = $"{fullPath}";
                                    embed.ThumbnailUrl = $"{thumbPath}";
                                    embed.Description = sb.ToString();
                                    embed.WithColor(new Color(247, 89, 213));
                                    pages.Add(embed);
                                }
                            }
                            pageCount++;
                        }
                        var msg = new PaginatedMessage
                        {
                            Pages = pages
                        };
                        msg.Options.Timeout = null;
                        await PagedReplyAsync(msg, true);
                    }
                    else
                    {
                        sb.AppendLine($"You don't have a Pokemon with that ID!");
                        embed.Title = $"No Pokemon with ID {id} Found";
                        embed.Description = sb.ToString();
                        embed.WithColor(new Color(247, 89, 213));
                        await ReplyAsync(null, false, embed.Build());
                    }
                }
                else
                {
                    sb.AppendLine($"Please either specify them by ID or re-nickname one to something else!");
                    embed.Title = $"You have multiple Pokemon with that nickname.";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());
                }
            }
            else
            {
                sb.AppendLine($"Please enter the Pokemon's ID with digits (0-9)!");
                embed.Title = $"Invalid Input";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }

        [Command("ContestStats", RunMode = RunMode.Async)]
        [Alias("cs")]
        [Summary("Shows you the contest information for a Pokemon with the ID or Nickname you specify! Usage: >pkmn conteststats [ID number or nickname]")]
        [RequireBegan()]
        public async Task ContestStatsCommand(string inputId)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            bool isId = true;
            Player player = bm.PlayerLoad(user.Id);
            Pokemon pkmn = new Pokemon();
            List<Pokemon> toRemove = new List<Pokemon>();
            bool validId = Int32.TryParse(inputId, out int id);
            if (!validId)
            {
                inputId = inputId.ToLower();
                foreach (Pokemon p in player.pokemon)
                {
                    if (p.nickname.ToLower() == inputId)
                    {
                        validId = true;
                        isId = false;
                    }
                }
            }
            if (validId)
            {
                bool foundMatch = false;
                foreach (Pokemon p in player.pokemon)
                {
                    if (isId)
                    {
                        if (p.id == id)
                        {
                            pkmn = p;
                            toRemove.Add(p);
                            foundMatch = true;
                        }
                    }
                    else
                    {
                        if (p.nickname.ToLower() == inputId)
                        {
                            pkmn = p;
                            toRemove.Add(p);
                            foundMatch = true;
                        }
                    }
                }
                if (toRemove.Count <= 1)
                {
                    if (foundMatch)
                    {
                        string firstLet = pkmn.nature.Substring(0, 1).ToUpper();
                        string nature = firstLet + pkmn.nature.Substring(1);
                        firstLet = pkmn.pkmnName.Substring(0, 1).ToUpper();
                        string species = firstLet + pkmn.pkmnName.Substring(1);
                        string nickname = pkmn.nickname;
                        if (pkmn.nickname == null)
                        {
                            nickname = pkmn.pkmnName;
                        }
                        if (pkmn.moveSet == null) { pkmn.moveSet = new Classes.Move[4]; }
                        string move1Name = "None";
                        if (pkmn.moveSet[0] != null)
                        {
                            move1Name = pkmn.moveSet[0].name.Substring(0, 1).ToUpper() + pkmn.moveSet[0].name.Substring(1);
                            if (move1Name.Contains("-"))
                            {
                                move1Name = move1Name.Substring(0, move1Name.IndexOf("-")) + " " + move1Name.Substring(move1Name.IndexOf("-") + 1, 1).ToUpper() + move1Name.Substring(move1Name.IndexOf("-") + 2);
                                move1Name = move1Name.Replace("-", " ");
                            }
                        }

                        string move2Name = "None";
                        if (pkmn.moveSet[1] != null)
                        {
                            move2Name = pkmn.moveSet[1].name.Substring(0, 1).ToUpper() + pkmn.moveSet[1].name.Substring(1);
                            if (move2Name.Contains("-"))
                            {
                                move2Name = move2Name.Substring(0, move2Name.IndexOf("-")) + " " + move2Name.Substring(move2Name.IndexOf("-") + 1, 1).ToUpper() + move2Name.Substring(move2Name.IndexOf("-") + 2);
                                move2Name = move2Name.Replace("-", " ");
                            }
                        }
                        string move3Name = "None";
                        if (pkmn.moveSet[2] != null)
                        {
                            move3Name = pkmn.moveSet[2].name.Substring(0, 1).ToUpper() + pkmn.moveSet[2].name.Substring(1);
                            if (move3Name.Contains("-"))
                            {
                                move3Name = move3Name.Substring(0, move3Name.IndexOf("-")) + " " + move3Name.Substring(move3Name.IndexOf("-") + 1, 1).ToUpper() + move3Name.Substring(move3Name.IndexOf("-") + 2);
                                move3Name = move3Name.Replace("-", " ");
                            }
                        }
                        string move4Name = "None";
                        if (pkmn.moveSet[0] != null)
                        {
                            move4Name = pkmn.moveSet[3].name.Substring(0, 1).ToUpper() + pkmn.moveSet[3].name.Substring(1);
                            if (move4Name.Contains("-"))
                            {
                                move4Name = move4Name.Substring(0, move4Name.IndexOf("-")) + " " + move4Name.Substring(move4Name.IndexOf("-") + 1, 1).ToUpper() + move4Name.Substring(move4Name.IndexOf("-") + 2);
                                move4Name = move4Name.Replace("-", " ");
                            }
                        }
                        embed.Title = $"ID #{pkmn.id}: {player.name}'s {nickname}";
                        sb.AppendLine($"**Species:** #{pkmn.dexNo} {species}");
                        sb.AppendLine($"**Nature:** {nature}");
                        sb.AppendLine($"**Sheen:** {pkmn.sheen}");
                        sb.AppendLine($"**Coolness:** {pkmn.coolness}");
                        sb.AppendLine($"**Beauty:** {pkmn.beauty}");
                        sb.AppendLine($"**Cuteness:** {pkmn.cuteness}");
                        sb.AppendLine($"**Cleverness:** {pkmn.cleverness}");
                        sb.AppendLine($"**Toughness:** {pkmn.toughness}");
                        sb.AppendLine($"**Move 1:** {move1Name}");
                        sb.AppendLine($"**Move 2:** {move2Name}");
                        sb.AppendLine($"**Move 3:** {move3Name}");
                        sb.AppendLine($"**Move 4:** {move4Name}");
                        embed.Description = sb.ToString();
                        string[] paths = new string[10];
                        paths[0] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/official-artwork/";
                        paths[1] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/home-icons/";
                        paths[2] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/anime/";
                        paths[3] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/dream-world/";
                        paths[4] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/mystery-dungeon/";
                        paths[5] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/8bit-sprites/";
                        paths[6] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/gen7-icons/";
                        paths[7] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/gen6-gifs/";
                        paths[8] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/gen4-gifs/";
                        paths[9] = "https://raw.githubusercontent.com/RampageRobot/pokemon/main/hgss-sprites/";
                        List<EmbedBuilder> pages = new List<EmbedBuilder>();
                        Random rand = new Random();
                        int cnt = player.imgPref;
                        if (cnt == 10)
                        {
                            cnt = rand.Next(0, 9);
                        }
                        var fileName = pkmn.dexNo + ".png";
                        if (cnt == 7 || cnt == 8)
                        {
                            fileName = pkmn.dexNo + ".gif";
                        }
                        string fullPath = Path.Combine(paths[cnt], fileName);
                        bool imgExists = UrlExists(fullPath);
                        if (!imgExists)
                        {
                            fullPath = Path.Combine(paths[player.backupPref], fileName);
                        }
                        if (player.thumbnailPref == 7 || player.thumbnailPref == 8)
                        {
                            fileName = pkmn.dexNo + ".gif";
                        }
                        string thumbPath = Path.Combine(paths[player.thumbnailPref], fileName);
                        bool thumbExists = UrlExists(thumbPath);
                        if (!thumbExists)
                        {
                            fileName = pkmn.dexNo + ".png";
                            thumbPath = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/" + fileName;
                        }
                        embed.WithImageUrl($"{fullPath}");
                        embed.WithThumbnailUrl($"{thumbPath}");
                        embed.Description = sb.ToString();
                        embed.WithColor(new Color(247, 89, 213));
                        pages.Add(embed);
                        int pageCount = 0;
                        while (pageCount < 9)
                        {
                            embed = new EmbedBuilder();
                            if (pageCount != cnt)
                            {
                                fileName = pkmn.dexNo + ".png";
                                if (pageCount == 7 || pageCount == 8)
                                {
                                    fileName = pkmn.dexNo + ".gif";
                                }
                                fullPath = Path.Combine(paths[pageCount], fileName);
                                if (UrlExists(fullPath))
                                {
                                    embed.ImageUrl = $"{fullPath}";
                                    embed.ThumbnailUrl = $"{thumbPath}";
                                    embed.Description = sb.ToString();
                                    embed.WithColor(new Color(247, 89, 213));
                                    pages.Add(embed);
                                }
                            }
                            pageCount++;
                        }
                        var msg = new PaginatedMessage
                        {
                            Pages = pages
                        };
                        msg.Options.Timeout = null;
                        await PagedReplyAsync(msg, true);
                    }
                    else
                    {
                        sb.AppendLine($"You don't have a Pokemon with that ID!");
                        embed.Title = $"No Pokemon with ID {id} Found";
                        embed.Description = sb.ToString();
                        embed.WithColor(new Color(247, 89, 213));
                        await ReplyAsync(null, false, embed.Build());
                    }
                }
                else
                {
                    sb.AppendLine($"Please either specify them by ID or re-nickname one to something else!");
                    embed.Title = $"You have multiple Pokemon with that nickname.";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());
                }
            }
            else
            {
                sb.AppendLine($"Please enter the Pokemon's ID with digits (0-9)!");
                embed.Title = $"Invalid Input";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }

        [Command("Nickname", RunMode = RunMode.Async)]
        [Alias("nick")]
        [Summary("Lets you nickname your Pokemon! Usage: >pkmn nickname [Pokemon's ID] [nickname (five word maximum)]")]
        [RequireBegan()]
        public async Task NicknameCommand(string pkmnId, string name, string name2 = null, string name3 = null, string name4 = null, string name5 = null)
        {
            if (name2 != null) {  name += " " + name2; }
            if (name3 != null){ name += " " + name3; }
            if (name4 != null) { name += " " + name4; }
            if (name5 != null) { name += " " + name5; }
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            bool validId = Int32.TryParse(pkmnId, out int id);
            if (validId)
            {
                var user = Context.User;
                Player player = bm.PlayerLoad(user.Id);
                bool exists = false;
                foreach (Pokemon x in player.pokemon)
                {
                    if (x.id == id)
                    {
                        exists = true;
                        x.nickname = name;
                        sb.AppendLine($"Your {x.pkmnName} looks so happy to have gotten its nickname of {x.nickname}!");
                        embed.Title = $"{x.pkmnName} has gotten a nickname!";
                        bm.PlayerSave(player);
                        embed.Description = sb.ToString();
                        embed.WithColor(new Color(247, 89, 213));
                        await ReplyAsync(null, false, embed.Build());
                    }
                }
                if (!exists)
                {
                    sb.AppendLine($"You don't have a Pokemon with that ID!");
                    embed.Title = "No Pokemon with ID " + id + " Found";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());
                }
            }
            else
            {
                sb.AppendLine($"Please only enter digits (0-9) for the Pokemon ID!");
                embed.Title = "Invalid Input";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }

        [Command("UseItem", RunMode = RunMode.Async)]
        [Summary("Lets you use an item on a Pokemon you specify! Usage: >pkmn useitem [ID of Pokemon] [item name]")]
        [RequireBegan()]
        public async Task UseItemCommand(string pokemonId, string name, string name2 = null, string name3 = null, string name4 = null, string name5 = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            name = name.Substring(0, 1).ToUpper() + name.Substring(1);
            if (name2 != null) { name += " " + name2.Substring(0, 1).ToUpper() + name2.Substring(1); }
            if (name3 != null) { name += " " + name3.Substring(0, 1).ToUpper() + name3.Substring(1); }
            if (name4 != null) { name += " " + name4.Substring(0, 1).ToUpper() + name4.Substring(1); }
            if (name5 != null) { name += " " + name5.Substring(0, 1).ToUpper() + name5.Substring(1); }
            bool hasItem = false;
            foreach (Classes.Item i in player.inventory)
            {
                if (i.name == name && i.qty >= 1) { hasItem = true; }
            }
            if (hasItem)
            {
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
                        EvolutionHandler eh = new EvolutionHandler();
                        EvolutionHandler.EvolvePkg ep = eh.EvolveCheck(p, player, false, null, name);
                        if (ep.hasEvolved)
                        {
                            p.dexNo = ep.evolvedTo.dexNo;
                            p.pkmnName = ep.evolvedTo.pkmnName;
                            p.type1 = ep.evolvedTo.type1;
                            p.type2 = ep.evolvedTo.type2;
                            p.speciesAbility1 = ep.evolvedTo.speciesAbility1;
                            p.speciesAbility2 = ep.evolvedTo.speciesAbility2;
                            p.baseStatTotal = ep.evolvedTo.baseStatTotal;
                            p.baseHP = ep.evolvedTo.baseHP;
                            p.baseAtk = ep.evolvedTo.baseAtk;
                            p.baseDef = ep.evolvedTo.baseDef;
                            p.baseSpAtk = ep.evolvedTo.baseSpAtk;
                            p.baseSpDef = ep.evolvedTo.baseSpDef;
                            p.baseSpd = ep.evolvedTo.baseSpd;
                            p.evoLevel = ep.evolvedTo.evoLevel;
                            p.legendStatus = ep.evolvedTo.legendStatus;
                            p.mythicStatus = ep.evolvedTo.mythicStatus;
                            p.nextEvo = ep.evolvedTo.nextEvo;
                            p.prevEvo = ep.evolvedTo.prevEvo;
                            p.eggGroup1 = ep.evolvedTo.eggGroup1;
                            p.eggGroup2 = ep.evolvedTo.eggGroup2;
                            p.levelSpeed = ep.evolvedTo.levelSpeed;
                            p.genderThreshhold = ep.evolvedTo.genderThreshhold;
                            p.blurb = ep.evolvedTo.blurb;
                            embed.Title = "Item Used!";
                            sb.AppendLine($"{p.nickname} has evolved! They're now a {p.pkmnName}!");
                            foreach (Classes.Item x in player.inventory) { if (x.name == name) { x.qty--; } }
                            bm.PlayerSave(player);
                        }
                        else
                        {
                            sb.AppendLine($"You give your {p.nickname} a {name}, but it had no effect! They look at you curiously. You put the {name} back in your bag.");
                            embed.Title = "No Effect!";
                        }
                    }
                    else
                    {
                        sb.AppendLine($"You don't own a Pokemon with the ID {id}!");
                        embed.Title = "Invalid Input";
                    }
                }
                else
                {
                    sb.AppendLine($"You didn't enter a valid number for your Pokemon's ID! Please enter a digit (0-9)!");
                    embed.Title = "Invalid Input";
                }
            }
            else
            {
                sb.AppendLine($"You don't seem to own that item, {player.name}!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("GiveItem", RunMode = RunMode.Async)]
        [Summary("Lets you use an item on a Pokemon you specify! Usage: >pkmn useitem [ID of Pokemon] [item name]")]
        [RequireBegan()]
        public async Task GiveItemCommand(string pokemonId, string name, string name2 = null, string name3 = null, string name4 = null, string name5 = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            name = name.Substring(0, 1).ToUpper() + name.Substring(1);
            if (name2 != null) { name += " " + name2.Substring(0, 1).ToUpper() + name2.Substring(1); }
            if (name3 != null) { name += " " + name3.Substring(0, 1).ToUpper() + name3.Substring(1); }
            if (name4 != null) { name += " " + name4.Substring(0, 1).ToUpper() + name4.Substring(1); }
            if (name5 != null) { name += " " + name5.Substring(0, 1).ToUpper() + name5.Substring(1); }
            bool hasItem = false;
            foreach (Classes.Item i in player.inventory)
            {
                if (i.name == name && i.qty >= 1) { hasItem = true; }
            }
            if (hasItem)
            {
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
                        sb.AppendLine($"You gave your {name} to {p.nickname} to hold!");
                        embed.Title = "Item Given!";
                        Classes.Item y = new Classes.Item(name, 1);
                        foreach (Classes.Item x in player.inventory) { if (x.name == name) { x.qty--; } }
                        foreach (Pokemon x in player.pokemon) { if (p.id == x.id) { p.heldItem = y; } }
                        bm.PlayerSave(player);
                    }
                    else
                    {
                        sb.AppendLine($"You don't own a Pokemon with the ID {id}!");
                        embed.Title = "Invalid Input";
                    }
                }
                else
                {
                    sb.AppendLine($"You didn't enter a valid number for your Pokemon's ID! Please enter a digit (0-9)!");
                    embed.Title = "Invalid Input";
                }
            }
            else
            {
                sb.AppendLine($"You don't seem to own that item, {player.name}!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("TakeItem", RunMode = RunMode.Async)]
        [Summary("Lets you take an item back from a Pokemon you specify! Usage: >pkmn useitem [ID of Pokemon]")]
        [RequireBegan()]
        public async Task TakeItemCommand(string pokemonId)
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
                    sb.AppendLine($"You took your {p.heldItem.name} back from {p.nickname}! You pet them in thanks.");
                    embed.Title = "Item Received!";
                    bool hasItem = false;
                    foreach (Classes.Item x in player.inventory) { if (x.name == p.heldItem.name) { x.qty++; hasItem = true; } }
                    foreach (Pokemon y in player.pokemon) { if (y.id == id) { y.heldItem = new Classes.Item(); } }
                    if (!hasItem) { player.inventory.Add(new Classes.Item(p.heldItem.name, 1)); }
                    bm.PlayerSave(player);
                }
                else
                {
                    sb.AppendLine($"You don't own a Pokemon with the ID {id}!");
                    embed.Title = "Invalid Input";
                }
            }
            else
            {
                sb.AppendLine($"You didn't enter a valid number for your Pokemon's ID! Please enter a digit (0-9)!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        public bool UrlExists(string url)
        {
            bool test = true;
            Uri urlCheck = new Uri(url);
            WebRequest request = WebRequest.Create(urlCheck);
            request.Timeout = 15000;

            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (Exception)
            {
                test = false; //url does not exist
            }
            return test;
        }
    }
}
