using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using PokeParadise;
using PokeParadise.Modules;
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
public class BaseModule : InteractiveBase<SocketCommandContext>
{
    [Command("Begin", RunMode = RunMode.Async)]
    [Summary("Starts your journey! Usage: >begin [enter your name]")]
    public async Task BeginCommand(string name = null)
    {
        var sb = new StringBuilder();
        var embed = new EmbedBuilder();
        var user = Context.User;      
        Player player = PlayerLoad(user.Id);
        if (player.name == null)
        {
            if (name != null)
            {
                Egg e = new Egg(DateTimeOffset.Now, 1);
                sb.AppendLine($"As you wander through the forest near your house, you stumble upon an Egg! You look around, but can't seem to find the Pokemon who laid it.");
                sb.AppendLine($"It's getting cold, so you decide to take it home with you for safety.");
                sb.AppendLine($"[To check on the egg, enter the command >check to see when it's ready to hatch! When it's ready, enter the command >hatch!]");
                player = new Player(name, user.Id);
                player.eggs.Add(e);
                PlayerSave(player);
                embed.Title = $"Welcome, {name}!";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
            else
            {
                sb.AppendLine($"Er, this is a bit awkward. You forgot to sign your name on your application! Please try again.");
                sb.AppendLine($"(Psst! Try entering the command like this: >apply [your name])");
                embed.Title = "No name on application!";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }
        else
        {
            sb.AppendLine($"You can't apply here when you already work here, silly!");
            embed.Title = "Employee Record for " + user.Username + " found.";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }
    }

    [Command("Stats", RunMode = RunMode.Async)]
    [Summary("Shows your Breeder Card! Usage: >stats")]
    [RequireBegan()]
    public async Task StatsCommand()
    {
        var sb = new StringBuilder();
        var embed = new EmbedBuilder();
        var user = Context.User;      
        Player player = PlayerLoad(user.Id);
        sb.AppendLine($"Breeder Name: {player.name}");
        sb.AppendLine($"Breeder Level: {player.level}");
        sb.AppendLine($"Coins Earned: {player.coins}");
        sb.AppendLine($"Pokemon Bred: {player.pokemon.Count}");
        sb.AppendLine($"Experience Earned: {player.xp}");
        embed.Title = $"{user.Username}'s Breeder Card";
        embed.Description = sb.ToString();
        embed.WithColor(new Color(247, 89, 213));
        await ReplyAsync(null, false, embed.Build());
    }

    [Command("Hatch", RunMode = RunMode.Async)]
    [Summary("Hatches any ready-to-hatch eggs in your inventory! Note: You have to use this command for the eggs to hatch! Usage: >hatch")]
    [RequireBegan()]
    public async Task HatchCommand()
    {
        var sb = new StringBuilder();
        EmbedFieldBuilder efb = new EmbedFieldBuilder();
        var user = Context.User;
        Player player = PlayerLoad(user.Id);
        List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
        List<Egg> toRemove = new List<Egg>();
        foreach (Egg e in player.eggs)
        {
            TimeSpan diff = DateTimeOffset.Now - e.obtained;
            if (diff > e.timeToHatch)
            {
                List<int> allIds = new List<int>();
                foreach (Pokemon p in player.pokemon)
                {
                    allIds.Add(p.id);
                }
                int largestId = 0;
                if (allIds.Count > 0)
                {
                    largestId = allIds.Max();
                }
                e.pkmn.id = largestId + 1;
                e.pkmn.level = 1;
                e.pkmn.originalId = e.pkmn.id;
                player.pokemon.Add(e.pkmn);
                toRemove.Add(e);
                efb.Name = $"**Egg #{e.eggId}**";
                efb.Value = $"Your Egg has hatched! A newborn **{e.pkmn.pkmnName}** was inside!";
                pages.Add(efb);
                efb = new EmbedFieldBuilder();
            }
            else 
            { 
                efb.Name = $"**Egg #{e.eggId}**"; 
                TimeSpan timeRemaining = e.timeToHatch - (DateTimeOffset.Now - e.obtained); 
                efb.Value = $"Your Egg still has **{timeRemaining.Minutes}m{timeRemaining.Seconds}s** left before it hatches!"; 
                pages.Add(efb); efb = new EmbedFieldBuilder(); 
            }
        }
        if (player.eggs.Count <= 0)
        {
            efb.Name = $"No eggs found!";
            efb.Value = "Earn or breed some eggs first!";
            pages.Add(efb);
        }
        foreach (Egg e in toRemove)
        {
            player.eggs.Remove(e);
        }
        PaginatedMessage msg = new PaginatedMessage();
        msg.Pages = pages;
        msg.Title = "**Hatchery Information**";
        msg.Color = new Color(247, 89, 213);
        await PagedReplyAsync(msg);
        EmbedBuilder embed = new EmbedBuilder();
        bool hasLeveled = XpHandler(player, Context.Guild.Id);
        if (hasLeveled)
        {
            sb.AppendLine($"");
            sb.AppendLine($"{player.name} has leveled up! They're now level {player.level}.");
        }
        if (hasLeveled && player.level == 5)
        {
            sb.AppendLine($"");
            sb.AppendLine($"{player.name}, you go into town looking for some supplies after the Egg's hatching, and you find a flyer!");
            sb.AppendLine($"The top of the flier reads: \"For Sale: Perfect Location for Pokemon Farm\". Underneath is a picture of a beautiful plot of land out by the forest. It's mostly undeveloped, except for a small house and a giant fence around the grazing area. You flip the paper over to read more.");
            sb.AppendLine($"");
            sb.AppendLine($"The back of the flier reads: \"Hello! My name is Joseph Arnold. Once upon a time, I took care of many Pokemon in this farm. Now, I've found that I'm just too old to do the work anymore, and it's time for me to retire. However, I'd love for the farm to go to someone who will do something beautiful with it.\"");
            sb.AppendLine($"");
            sb.AppendLine($"\"The land is well-taken-care of, and the house is sturdy. It's not a shiny-new mansion, mind you; you may want to remodel a time or ten! But it's fantastic for beginners. I'm asking a very reasonable price of 2500 coins for the place, so I can retire in comfort. Please call me if you're interested!\"");
            sb.AppendLine($"Underneath the information about the land, there's a phone number typed neatly along the bottom. You fold up the flyer for later reference.");
            sb.AppendLine($"");
            sb.AppendLine($"[If you want to buy the farm, type >center buy when you have enough money!]");
            embed.Description = sb.ToString();
            embed.Title = "Hatchery Information";
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }
    }

    [Command("Check", RunMode = RunMode.Async)]
    [Summary("Checks the status of your eggs, to see if any are ready to hatch! If you want to see all eggs: Usage: >check! If you want to see one egg in particular: Usage: >check {egg number}")]
    [RequireBegan()]
    public async Task CheckCommand(string eggNum = null)
    {
        var sb = new StringBuilder();
        var embed = new EmbedBuilder();
        var user = Context.User;
        if (eggNum != null)
        {
            bool validInput = Int32.TryParse(eggNum, out int eggId);
            if (validInput)
            {
                Player player = PlayerLoad(user.Id);
                Egg egg = new Egg();
                egg.pkmn.pkmnName = "null";
                foreach (Egg e in player.eggs)
                {
                    if (e.eggId == eggId)
                    {
                        egg = e;
                    }
                }
                if (egg.pkmn.pkmnName != "null")
                {
                    embed.AddField($"**Egg ID:**", egg.eggId);
                    TimeSpan timeRemaining = egg.timeToHatch - (DateTimeOffset.Now - egg.obtained);
                    embed.AddField($"**Time Left to Hatch:**", $"{timeRemaining.Minutes}m{timeRemaining.Seconds}s");
                    embed.Title = "Egg #" + egg.eggId + " Information";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());
                }
                else
                {
                    sb.AppendLine($"You don't have any Eggs with that number!");
                    embed.Title = "Egg #" + egg.eggId + " Not Found";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());
                }
            }
            else
            {
                sb.AppendLine($"The Egg number you entered was not in numerical format! Please enter the Egg number in digits (0-9)!");
                embed.Title = "Invalid Input";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }
        else
        {
            Player player = PlayerLoad(user.Id);
            EmbedFieldBuilder efb = new EmbedFieldBuilder();
            List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
            foreach (Egg e in player.eggs)
            {
                TimeSpan diff = DateTimeOffset.Now - e.obtained;
                TimeSpan timeRemaining = e.timeToHatch - (DateTimeOffset.Now - e.obtained);
                if (diff > e.timeToHatch)
                {
                    efb.Name = $"**Egg #{e.eggId}**";
                    efb.Value = $"Your Egg is cracking! It's ready to hatch!";
                    pages.Add(efb);
                }
                else { efb.Name = $"**Egg #{e.eggId}**"; efb.Value = $"Your Egg still has **{timeRemaining.Minutes}m{timeRemaining.Seconds}s** left before it hatches!"; pages.Add(efb); }
                efb = new EmbedFieldBuilder();
            }
            if (player.eggs.Count <= 0)
            {
                efb.Name = $"No eggs found!";
                efb.Value = "Earn or breed some eggs first!";
                pages.Add(efb);
            }
            PaginatedMessage msg = new PaginatedMessage();
            msg.Pages = pages;
            msg.Title = "Hatchery Information";
            msg.Color = new Color(247, 89, 213);
            await PagedReplyAsync(msg);
        }
    }

    [Command("Help")]
    [Summary("Lists all commands available in the bot! You can also specify a specific module, or even a specific command! Usage: >help [module or command]")]
    public async Task Help(string modOrCom = null, string name2 = null)
    {
        if (name2 != null)
        {
            modOrCom += " " + name2;
        }
        if (modOrCom != null)
        {
            modOrCom = modOrCom.ToLower();
        }
        List<CommandInfo> commands = PokeParadise.Services.CommandHandler.cmds.Commands.ToList<CommandInfo>();
        List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
        commands.OrderBy(x => x.Module.Group);
        if (modOrCom == null)
        {
            foreach (CommandInfo command in commands)
            {
                // Get the command Summary attribute information
                string embedFieldText = command.Summary ?? "No description available\n";
                EmbedFieldBuilder efb = new EmbedFieldBuilder();
                if (command.Module.Name != "BaseModule")
                {
                    efb.Name = $"**>{command.Module.Group} {command.Name}**";
                }
                else
                {
                    efb.Name = $"**>{command.Name}**";
                }
                efb.Value = embedFieldText;
                if (command.Module.Name != "Admin")
                {
                    pages.Add(efb);
                }
            }
        }
        else
        {
            List<string> modules = new List<string>();
            List<string> cmds = new List<string>();
            foreach (CommandInfo command in commands)
            {
                if (!modules.Contains(command.Module.Name.ToLower()))
                {
                    modules.Add(command.Module.Name.ToLower());
                }
                if (!modules.Contains(command.Name.ToLower()))
                {
                    cmds.Add(command.Name.ToLower());
                }
            }
            string modOrComChoice = null;
            if (name2 != null)
            {
                modOrCom = modOrCom.Substring(modOrCom.IndexOf(" ") + 1);
            }
            if (modules.Contains(modOrCom)) { modOrComChoice = "Module"; }
            else if (cmds.Contains(modOrCom)) { modOrComChoice = "Command"; }
            if (modOrComChoice == "Command")
            {
                foreach (CommandInfo command in commands)
                {
                    if (command.Name.ToLower() == modOrCom)
                    {
                        string embedFieldText = command.Summary ?? "No description available\n";
                        EmbedFieldBuilder efb = new EmbedFieldBuilder();
                        if (command.Module.Name != "BaseModule")
                        {
                            efb.Name = $"**>{command.Module.Group} {command.Name}**";
                        }
                        else
                        {
                            efb.Name = $"**>{command.Name}**";
                        }
                        efb.Value = embedFieldText;
                        pages.Add(efb);
                    }
                }
            }
            else if (modOrComChoice == "Module")
            {
                foreach (CommandInfo command in commands)
                {
                    if (command.Module.Name.ToLower() == modOrCom)
                    {
                        string embedFieldText = command.Summary ?? "No description available\n";
                        EmbedFieldBuilder efb = new EmbedFieldBuilder();
                        if (command.Module.Name != "BaseModule")
                        {
                            efb.Name = $"**>{command.Module.Group} {command.Name}**";
                        }
                        else
                        {
                            efb.Name = $"**>{command.Name}**";
                        }
                        efb.Value = embedFieldText;
                        pages.Add(efb);
                    }
                }
            }
            else
            {
                EmbedFieldBuilder efb = new EmbedFieldBuilder();
                efb.Name = $"Command or Module not found!";
                efb.Value = $"Please try again!";
                pages.Add(efb);
            }
        }
        PaginatedMessage msg = new PaginatedMessage();
        msg.Pages = pages;
        msg.Title = "**Commands**";
        msg.Options.Timeout = new TimeSpan(0, 2, 0);
        await PagedReplyAsync(msg);
    }

    public void Serialize(object o, string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Stream s = File.Open("D:\\PokeParadise Files\\" + fileName + ".xml", FileMode.OpenOrCreate);
        formatter.Serialize(s, o);
        s.Close();
    }

    public Player PlayerLoad(ulong id)
    {
        Player player = new Player();
        Dictionary<ulong, Player> players = FetchPlayerDict();
        if (players.ContainsKey(id))
        {
            player = players[id];
        }
        else
        {
            player.name = null;
        }
        return player;
    }

    public void PlayerSave(Player player)
    {
        Dictionary<ulong, Player> players = FetchPlayerDict();
        if (players.ContainsKey(player.id))
        {
            players[player.id] = player;
        }
        else
        {
            players.Add(player.id, player);
        }
        Serialize(players, "PlayerDictionary");
    }

    public Dictionary<ulong, Player> FetchPlayerDict()
    {
        Dictionary<ulong, Player> players = new Dictionary<ulong, Player>();
        BinaryFormatter formatter = new BinaryFormatter();
        object lck = new object();
        lock (lck)
        {
            Stream s = File.Open("D:\\PokeParadise Files\\PlayerDictionary.xml", FileMode.OpenOrCreate);
            if (s.Length != 0)
            {
                players = (Dictionary<ulong, Player>)formatter.Deserialize(s);
            }
            s.Close();

            return players;
        }
    }

    public bool XpHandler(Player player, ulong serverId)
    {
        bool hasLeveled = false;
        int xpToNextLevel = Convert.ToInt32(Math.Floor(Math.Pow(player.level + 1, 3)));
        Random rand = new Random();
        double xpGainChoice = rand.Next(0, 5);
        if (xpGainChoice != 0)
        {
            xpGainChoice = xpGainChoice / 10;
        }
        int xpGain = Convert.ToInt32(Math.Floor(Math.Pow(player.level, 1.5 + xpGainChoice)));
        player.xp += xpGain;
        if (player.xp >= xpToNextLevel && player.level + 1 != 101) { hasLeveled = true; player.skillPoints++; player.level++; }
        PlayerSave(player);
        return hasLeveled;
    }

}
