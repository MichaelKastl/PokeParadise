using Discord;
using Discord.Addons.Interactive;
using Discord.Audio.Streams;
using Discord.Commands;
using PokeParadise.Attributes;
using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
    [Group("Pokedex")]
    [Alias("dex", "lookup")]
    [NotPlayingTrivia()]
    public class PokedexModule : InteractiveBase<SocketCommandContext>
    {
        BaseModule bm = new BaseModule();
        [Command("Pokemon", RunMode = RunMode.Async)]
        [Alias("pkmn", "p")]
        [Summary("Allows you to search for a specific Pokemon by species name or national Pokedex number! Usage: >pokedex [name or dex number]")]
        [RequireBegan()]
        public async Task LookupCommand(string input, string name2 = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            Pokemon p;
            bool isDex = Int32.TryParse(input, out int dexNo);
            if (isDex)
            {
                p = new Pokemon(dexNo);
            }
            else
            {
                string speciesName = input;
                speciesName = speciesName.Substring(0, 1).ToUpper() + speciesName.Substring(1);
                if (name2 != null)
                {
                    name2 = name2.Substring(0, 1).ToUpper() + name2.Substring(1);
                    speciesName += " " + name2;
                }
                p = new Pokemon(speciesName);
            }
            if (p.pkmnName != null)
            {
                string type1 = p.type1.Substring(0, 1).ToUpper() + p.type1.Substring(1);
                string type2 = "None";
                if (p.type2 != "")
                {
                    type2 = p.type2.Substring(0, 1).ToUpper() + p.type2.Substring(1); ;
                }
                string speciesAbility1 = p.speciesAbility1.Substring(0, 1).ToUpper() + p.speciesAbility1.Substring(1);
                string speciesAbility2 = "None";
                if (p.speciesAbility2 != "")
                {
                    speciesAbility2 = p.speciesAbility2.Substring(0, 1).ToUpper() + p.speciesAbility2.Substring(1);
                }
                string eggGroup1 = p.eggGroup1.Substring(0, 1).ToUpper() + p.eggGroup1.Substring(1);
                string eggGroup2 = "None";
                if (p.eggGroup2 != "")
                {
                    eggGroup2 = p.eggGroup2.Substring(0, 1).ToUpper() + p.eggGroup2.Substring(1);
                }
                string evoLevel = p.evoLevel.ToString();
                if (evoLevel == "0" || evoLevel == "999")
                {
                    evoLevel = "None";
                }
                string nextEvo = p.nextEvo;
                if (nextEvo == "")
                {
                    nextEvo = "None";
                }
                string prevEvo = p.prevEvo;
                if (prevEvo == "")
                {
                    prevEvo = "None";
                }
                string dexLink = "https://veekun.com/dex/pokemon/" + p.pkmnName;
                embed.Title = $"ID #{p.id}: {p.pkmnName}";
                sb.AppendLine($"**Type 1:** {type1}");
                sb.AppendLine($"**Type 2:** {type2}");
                sb.AppendLine($"**Ability One:** {speciesAbility1}");
                sb.AppendLine($"**Ability Two:** {speciesAbility2}");
                sb.AppendLine($"**Base Stat Total:** {p.baseStatTotal}");
                sb.AppendLine($"**Base HP:** {p.baseHP}");
                sb.AppendLine($"**Base Attack:** {p.baseAtk}");
                sb.AppendLine($"**Base Defence:** {p.baseDef}");
                sb.AppendLine($"**Base Sp. Attack:** {p.baseSpAtk}");
                sb.AppendLine($"**Base Sp. Defence:** {p.baseSpDef}");
                sb.AppendLine($"**Base Speed:** {p.baseSpd}");
                sb.AppendLine($"**Evolves At Level:** {evoLevel}");
                sb.AppendLine($"**Next Evolution:** {nextEvo}");
                sb.AppendLine($"**Previous Evolution:** {prevEvo}");
                sb.AppendLine($"**First Egg Group:** {eggGroup1}");
                sb.AppendLine($"**Second Egg Group:** {eggGroup2}");
                sb.AppendLine($"**Leveling Speed:** {p.levelSpeed}");
                sb.AppendLine($"**Pokedex Link (Veekun):** {dexLink}");
                sb.Append($"*{p.blurb}*");
                Random rand = new Random();
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
                var fileName = p.dexNo + ".png";
                if (cnt == 7 || cnt == 8)
                {
                    fileName = p.dexNo + ".gif";
                }
                string fullPath = Path.Combine(paths[cnt], fileName);
                bool imgExists = UrlExists(fullPath);
                if (!imgExists)
                {
                    fullPath = Path.Combine(paths[player.backupPref], fileName);
                }
                if (player.thumbnailPref == 7 || player.thumbnailPref == 8)
                {
                    fileName = p.dexNo + ".gif";
                }
                string thumbPath = Path.Combine(paths[player.thumbnailPref], fileName);
                bool thumbExists = UrlExists(thumbPath);
                if (!thumbExists)
                {
                    fileName = p.dexNo + ".png";
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
                        fileName = p.dexNo + ".png";
                        if (pageCount == 7 || pageCount == 8)
                        {
                            fileName = p.dexNo + ".gif";
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
                sb.AppendLine($"Please make sure to either spell the name of the Pokemon correctly, or provide a valid Pokedex number!");
                embed.Title = "Invalid Input";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }

        [Command("EggGroup", RunMode = RunMode.Async)]
        [Alias("egg", "group")]
        [Summary("Allows you to see a paginated list of all Pokemon with the specified Egg Group! Usage: >pokedex egggroup [group name]")]
        [RequireBegan()]
        public async Task DexEggGroupCommand(string input, string input2 = null, string input3 = null)
        {
            if (input2 != null)
            {
                input += " " + input2;
            }
            if (input3 != null)
            {
                input += " " + input3;
            }
            input = input.ToLower();
            EmbedFieldBuilder efb = new EmbedFieldBuilder();
            List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
            int cnt = 0;
            var user = Context.User;
            Pokemon p = new Pokemon();
            Pokemon[] dex = p.FetchPokedex();
            foreach (Pokemon x in dex)
            {
                if (x.eggGroup1 == input || x.eggGroup2 == input)
                {
                    efb.Name = $"**#{x.dexNo}: {x.pkmnName}**";
                    string group1 = x.eggGroup1.Substring(0, 1).ToUpper() + x.eggGroup1.Substring(1);
                    string group2 = x.eggGroup2;
                    if (x.eggGroup2 == null || x.eggGroup2 == " " || x.eggGroup2 == "")
                    {
                        group2 = "None";
                    }
                    else
                    {
                        group2 = x.eggGroup2.Substring(0, 1).ToUpper() + x.eggGroup2.Substring(1);
                    }
                    efb.Value = $"*Egg Group 1: {group1}, Egg Group 2: {group2}*";
                    pages.Add(efb);
                    efb = new EmbedFieldBuilder();
                    cnt++;
                }
            }
            if (cnt == 0)
            {
                efb.Value = $"No Pokemon with that Egg Group could be found! This is probably because you entered an invalid Egg Group!";
                efb.Name = $"No Pokemon Found";
                pages.Add(efb);
            }
            PaginatedMessage msg = new PaginatedMessage();
            msg.Title = $"Pokemon With {input} Egg Group";
            msg.Color = new Color(247, 89, 213);
            msg.Pages = pages;
            msg.Options.FieldsPerPage = 10;
            msg.Options.Timeout = null;
            await PagedReplyAsync(msg);
        }

        [Command("Type", RunMode = RunMode.Async)]
        [Summary("Allows you to see a paginated list of all Pokemon with the specified Type! Usage: >pokedex type [type name]")]
        [RequireBegan()]
        public async Task DexTypeCommand(string input)
        {
            input = input.ToLower();
            input = input.Substring(0, 1).ToUpper() + input.Substring(1);
            EmbedFieldBuilder efb = new EmbedFieldBuilder();
            List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
            int cnt = 0;
            var user = Context.User;
            Pokemon p = new Pokemon();
            Pokemon[] dex = p.FetchPokedex();
            foreach (Pokemon x in dex)
            {
                if (x.type1 == input || x.type2 == input)
                {
                    efb.Name = $"**#{x.dexNo}: {x.pkmnName}**";
                    string type2 = x.type2;
                    if (x.type2 == null || x.type2 == " " || x.type2 == "")
                    {
                        type2 = "None";
                    }
                    efb.Value = $"*Type 1: {x.type1}, Type 2: {type2}*";
                    pages.Add(efb);
                    efb = new EmbedFieldBuilder();
                    cnt++;
                }
            }
            if (cnt == 0)
            {
                efb.Value = $"No Pokemon with that Type could be found! This is probably because you entered an invalid Type!";
                efb.Name = $"No Pokemon Found";
                pages.Add(efb);
            }
            PaginatedMessage msg = new PaginatedMessage();
            msg.Title = $"{input}-Type Pokemon";
            msg.Color = new Color(247, 89, 213);
            msg.Pages = pages;
            msg.Options.FieldsPerPage = 10;
            msg.Options.Timeout = null;
            await PagedReplyAsync(msg);
        }

        [Command("Stat", RunMode = RunMode.Async)]
        [Summary("Allows you to search Pokemon by their base stats! Usage: >pokedex stat [above, below, or equals] [amount] [stat name]")]
        [RequireBegan()]
        public async Task DexStatCommand(string comparatorInput, string amount, string statName1, string statName2 = null)
        {
            comparatorInput = comparatorInput.ToLower();
            bool validAmount = Int32.TryParse(amount, out int amt);
            if (statName1 == "sp.")
            {
                statName1 = "sp";
            }
            if (statName2 != null)
            {
                statName1 += statName2;
            }
            EmbedFieldBuilder efb = new EmbedFieldBuilder();
            List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
            if (comparatorInput == "above" || comparatorInput == "below" || comparatorInput == "equals")
            {
                if (validAmount)
                {
                    int cnt = 0;
                    var user = Context.User;
                    Pokemon p = new Pokemon();
                    Pokemon[] dex = p.FetchPokedex();
                    foreach (Pokemon x in dex)
                    {
                        bool matches = false;
                        string stat = null;
                        int statAmount = 0;
                        switch (statName1)
                        {
                            case "atk":
                                stat = "Attack";
                                statAmount = x.baseAtk;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseAtk > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseAtk < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseAtk == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "attack":
                                stat = "Attack";
                                statAmount = x.baseAtk;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseAtk > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseAtk < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseAtk == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "def":
                                stat = "Defense";
                                statAmount = x.baseDef;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseDef > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseDef < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseDef == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "defence":
                                stat = "Defense";
                                statAmount = x.baseDef;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseDef > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseDef < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseDef == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "defense":
                                stat = "Defense";
                                statAmount = x.baseDef;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseDef > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseDef < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseDef == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "hp":
                                stat = "Health";
                                statAmount = x.baseHP;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseHP > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseHP < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseHP == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "health":
                                stat = "Health";
                                statAmount = x.baseHP;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseHP > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseHP < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseHP == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "spatk":
                                stat = "Sp. Attack";
                                statAmount = x.baseSpAtk;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseSpAtk > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseSpAtk < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseSpAtk == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "spattack":
                                stat = "Sp. Attack";
                                statAmount = x.baseSpAtk;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseSpAtk > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseSpAtk < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseSpAtk == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "spdef":
                                stat = "Sp. Defense";
                                statAmount = x.baseSpDef;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseSpDef > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseSpDef < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseSpDef == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "spdefence":
                                stat = "Sp. Defense";
                                statAmount = x.baseSpDef;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseSpDef > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseSpDef < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseSpDef == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "spdefense":
                                stat = "Sp. Defense";
                                statAmount = x.baseSpDef;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseSpDef > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseSpDef < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseSpDef == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "spd":
                                stat = "Speed";
                                statAmount = x.baseSpd;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseSpd > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseSpd < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseSpd == amt) { matches = true; }
                                        break;
                                }
                                break;
                            case "speed":
                                stat = "Speed";
                                statAmount = x.baseSpd;
                                switch (comparatorInput)
                                {
                                    case "above":
                                        if (x.baseSpd > amt) { matches = true; }
                                        break;
                                    case "below":
                                        if (x.baseSpd < amt) { matches = true; }
                                        break;
                                    case "equals":
                                        if (x.baseSpd == amt) { matches = true; }
                                        break;
                                }
                                break;
                        }
                        if (matches)
                        {
                            efb.Name = $"**#{x.dexNo}: {x.pkmnName}**";
                            efb.Value = $"{stat}: {statAmount}";
                            pages.Add(efb);
                            cnt++;
                            efb = new EmbedFieldBuilder();
                        }
                    }
                    if (cnt == 0)
                    {
                        efb.Value = $"No Pokemon with that stat total could be found! This is probably because you entered an invalid Type!";
                        efb.Name = $"No Pokemon Found";
                        pages.Add(efb);
                    }
                }
                else
                {
                    efb.Name = $"Invalid Amount!";
                    efb.Value = $"{amount} is not a valid amount! Please enter a total in digits (0-9).";
                }
            }
            else
            {
                efb.Name = $"Invalid Comparator!";
                efb.Value = $"{comparatorInput} is not a valid comparator! Please enter either above, below, or equals.";
            }
            PaginatedMessage msg = new PaginatedMessage();
            msg.Title = $"Pokemon With {statName1} {comparatorInput} to ";
            msg.Color = new Color(247, 89, 213);
            msg.Pages = pages;
            msg.Options.FieldsPerPage = 10;
            msg.Options.Timeout = null;
            await PagedReplyAsync(msg);
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
