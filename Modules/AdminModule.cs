using Discord;
using Discord.Commands;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic.CompilerServices;
using PokeApiNet;
using PokeParadise.Attributes;
using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokeParadise.Modules
{
    [Group("Admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        /*

        This template is to easily create new commands. Keep it commented out, and copy and paste it where needed.

        [Command("", RunMode = RunMode.Async)]
        [Summary("")]
        [RequireBegan()]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
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

        [Command("addegg", RunMode = RunMode.Async)]
        [Summary("Adds an egg of a given pokemon species. Usage: >addegg {species}")]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
        [RequireBegan()]
        public async Task AddEggCommand(string pkmnName, ulong userId = 0)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            string species = pkmnName.ToLower();
            Player player;
            Pokemon p = new Pokemon(species);
            if (userId == 0)
            {
                player = bm.PlayerLoad(user.Id);
            }
            else
            {
                player = bm.PlayerLoad(userId);
            }
            List<int> ids = new List<int>();
            foreach (Egg x in player.eggs)
            {
                ids.Add(x.eggId);
            }
            int largestId = 0;
            if (ids.Count > 0)
            {
                largestId = ids.Max();
            }
            int id = largestId + 1;
            Egg e = new Egg(p, DateTimeOffset.Now, id);
            player.eggs.Add(e);
            bm.PlayerSave(player);
            sb.AppendLine($"Egg of species {pkmnName} added.");
            embed.Title = "Added egg to inventory.";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("givemeitem", RunMode = RunMode.Async)]
        [Summary("Gives an item to the player who ran the command. WARNING: No null checker, so if the item doesn't actually exist you'll still have it.")]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
        [RequireBegan()]
        public async Task GiveMeItemCommand(string name, string name2 = null, string name3 = null, string name4 = null, string name5 = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            name = name.Substring(0, 1).ToUpper() + name.Substring(1);
            if (name2 != null) { name += " " + name2.Substring(0, 1).ToUpper() + name2.Substring(1); }
            if (name3 != null) { name += " " + name3.Substring(0, 1).ToUpper() + name3.Substring(1); }
            if (name4 != null) { name += " " + name4.Substring(0, 1).ToUpper() + name4.Substring(1); }
            if (name5 != null) { name += " " + name5.Substring(0, 1).ToUpper() + name5.Substring(1); }
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            bool hasItem = false;
            foreach (Classes.Item i in player.inventory) { if (i.name == name) { i.qty++; hasItem = true; } }
            if (!hasItem) { Classes.Item i = new Classes.Item(name, 1); player.inventory.Add(i); }
            bm.PlayerSave(player);
            sb.AppendLine($"Success");
            embed.Title = "Item Added";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("updatepokemon", RunMode = RunMode.Async)]
        [Summary("Updates all pokemon to the current data structure.")]
        [RequireBegan()]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
        public async Task UpdatePokemonCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            Dictionary<ulong, Player> playerDict = bm.FetchPlayerDict();
            List<Pokemon> toRemove = new List<Pokemon>();
            foreach (KeyValuePair<ulong, Player> kvp in playerDict)
            {
                Random rand = new Random();
                Player player = kvp.Value;
                foreach (Pokemon p in player.pokemon)
                {
                    Pokemon x = new Pokemon();
                    if (p.pkmnName == null)
                    {
                        toRemove.Add(p);
                    }
                }
                foreach (Pokemon p in toRemove)
                {
                    kvp.Value.pokemon.Remove(p);
                }
                foreach (Egg e in player.eggs)
                {
                    Egg x = new Egg(e.pkmn.dexNo, DateTimeOffset.Now);
                    x.pkmn.trainer = player;
                }
                bm.PlayerSave(player);
            }
            sb.AppendLine($"Success!");
            embed.Title = "All Pokemon updated to newest format.";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("updateplayers", RunMode = RunMode.Async)]
        [Summary("Updates player data to current data structure.")]
        [RequireBegan()]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
        public async Task UpdatePlayersCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            Player p = new Player();
            Dictionary<ulong, Player> playerDict = bm.FetchPlayerDict();
            foreach (KeyValuePair<ulong, Player> kvp in playerDict)
            {
                kvp.Value.isDoingTrivia = false;
                bm.PlayerSave(kvp.Value);
            }
            sb = new StringBuilder();
            sb.AppendLine($"Success!");
            embed.Title = "Players Updated";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("resettrivia", RunMode = RunMode.Async)]
        [Summary("Updates player trivia variables")]
        [RequireBegan()]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
        public async Task ResetTriviaCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            Player p = new Player();
            Dictionary<ulong, Player> playerDict = bm.FetchPlayerDict();
            foreach (KeyValuePair<ulong, Player> kvp in playerDict)
            {
                kvp.Value.isDoingTrivia = false;
                bm.PlayerSave(kvp.Value);
            }
            sb = new StringBuilder();
            sb.AppendLine($"Success!");
            embed.Title = "Players Updated";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("clearbreedingeggs", RunMode = RunMode.Async)]
        [Summary("Removes all eggs currently being bred by the running player because of an error.")]
        [RequireBegan()]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
        public async Task ClearBreedingEggsCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            List<Egg> toRemove = new List<Egg>();
            foreach (Egg e in player.breedingEggs)
            {
                toRemove.Add(e);
            }
            foreach (Egg e in toRemove)
            {
                player.breedingEggs.Remove(e);
            }
            bm.PlayerSave(player);
            sb.AppendLine($"Done");
            embed.Title = "Done";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("clearbreedingstalls", RunMode = RunMode.Async)]
        [Summary("Sets all pokemon's breeding state to false.")]
        [RequireBegan()]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
        public async Task ClearStallsCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            foreach (Pokemon p in player.pokemon)
            {
                p.isBreeding = false;
            }
            bm.PlayerSave(player);
            sb.AppendLine($"Done");
            embed.Title = "Done";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("fixfoods", RunMode = RunMode.Async)]
        [Summary("Resets broken foods")]
        [RequireBegan()]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
        public async Task FixFoodsCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            foreach (Food f in player.lunchBox)
            {
                if (f.type == "" || f.type == " " || f.type == null) { f.type = "Rainbow"; }
            }
            bm.PlayerSave(player);
            sb.AppendLine($"Success");
            embed.Title = "Done";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("FetchData", RunMode = RunMode.Async)]
        [Summary("Command to grab all needed info from PokeAPI and write it to a CSV (for purposes of saving their bandwidth).")]
        [RequireOwner(Group = "Permission")]
        [RequireBotman(Group = "Permission")]
        [RequireBegan()]
        public async Task FetchDataCommand()
        {
            PokeApiClient pokeClient = new PokeApiClient();
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            List<string> linesToWrite = new List<string>();
            linesToWrite.Add($"type ; effect ; appeal ; jam ; flavorText"); 
            int cnt = 0;
            try
            {
                while (cnt <= 796)
                {
                    if (cnt > 0)
                    {
                        string moveNum = "";
                        string type = "";
                        string effect = "";
                        string appeal = "";
                        string jam = "";
                        string flavorText = "";
                        PokeApiNet.Move target = await pokeClient.GetResourceAsync<PokeApiNet.Move>(cnt);
                        if (target.ContestEffect != null)
                        {
                            moveNum = target.Id.ToString();
                            type = target.ContestType.Name;
                            ContestEffect x = await pokeClient.GetResourceAsync<ContestEffect>(target.ContestEffect);
                            effect = x.EffectEntries.FirstOrDefault().Effect;
                            appeal = x.Appeal.ToString();
                            jam = x.Jam.ToString();
                            flavorText = x.FlavorTextEntries.FirstOrDefault().FlavorText;
                        }
                        else if (target.SuperContestEffect != null)
                        {
                            moveNum = target.Id.ToString();
                            type = target.ContestType.Name;
                            SuperContestEffect x = await pokeClient.GetResourceAsync<SuperContestEffect>(target.SuperContestEffect);
                            effect = x.FlavorTextEntries.FirstOrDefault().FlavorText;
                            appeal = x.Appeal.ToString();
                            flavorText = x.FlavorTextEntries.FirstOrDefault().FlavorText;
                        }
                        linesToWrite.Add($"{moveNum};{type};{effect};{appeal};{jam};{flavorText}");
                    }
                    Console.WriteLine($"{cnt}/796");
                    cnt++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pokemon not found. {ex}");
            }

            using (StreamWriter file =
                new StreamWriter(@"D:\\DataToAddLog.txt"))
            {
                foreach (string line in linesToWrite)
                {
                    file.WriteLine(line);
                }
            }
            pokeClient.ClearCache();
            sb.AppendLine($"Data written.");
            embed.Title = "Finished writing data";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        public void Serialize(object o, string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream s = File.Open("D:\\" + fileName + ".xml", FileMode.OpenOrCreate);
            formatter.Serialize(s, o);
            s.Close();
        }
    }
}
/*
 *  PokemonSpecies target = await pokeClient.GetResourceAsync<PokemonSpecies>(cnt);
                        eReq = new EvolutionRequirement();
                        eReq.speciesName = target.Name;
                        eReq.dexNumber = target.Id;
                        bool isValid = false;
                        if (target.EvolvesFromSpecies != null)
                        {
                            eReq.evolvesFrom = target.EvolvesFromSpecies.Name;
                            EvolutionChain evoChain = await pokeClient.GetResourceAsync(target.EvolutionChain);
                            if (evoChain.Chain.Species.Name == eReq.evolvesFrom)
                            {
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().Trigger != null)
                                {
                                    eReq.evoMethod = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().Trigger.Name;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().Gender != null)
                                {
                                    eReq.gender = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().Gender.Value;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().HeldItem != null)
                                {
                                    eReq.heldItem = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().HeldItem.Name;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().Item != null)
                                {
                                    eReq.item = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().Item.Name;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().KnownMove != null)
                                {
                                    eReq.knownMove = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().KnownMove.Name;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().KnownMoveType != null)
                                {
                                    eReq.knownMoveType = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().KnownMoveType.Name;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().Location != null)
                                {
                                    eReq.location = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().Location.Name;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().MinAffection != null)
                                {
                                    eReq.minAffection = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().MinAffection.Value;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().MinBeauty != null)
                                {
                                    eReq.minBeauty = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().MinBeauty.Value;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().MinHappiness != null)
                                {
                                    eReq.minHappiness = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().MinHappiness.Value;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().MinLevel != null)
                                {
                                    eReq.minLevel = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().MinLevel.Value;
                                }
                                eReq.needsOverworldRain = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().NeedsOverworldRain;
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().PartySpecies != null)
                                {
                                    eReq.partySpecies = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().PartySpecies.Name;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().PartyType != null)
                                {
                                    eReq.partyType = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().PartyType.Name;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().RelativePhysicalStats != null)
                                {
                                    eReq.relativePhysicalStats = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().RelativePhysicalStats.Value;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().TimeOfDay != null)
                                {
                                    eReq.timeOfDay = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().TimeOfDay;
                                }
                                if (evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().TradeSpecies != null)
                                {
                                    eReq.tradeSpecies = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().TradeSpecies.Name;
                                }
                                eReq.turnUpsideDown = evoChain.Chain.EvolvesTo.FirstOrDefault().EvolutionDetails.FirstOrDefault().TurnUpsideDown;
                                lastPokemon = eReq.speciesName;
                                alreadyCovered.Add(lastPokemon);
                                isValid = true;
                                linesToWrite.Add($"{eReq.dexNumber};{eReq.speciesName};{eReq.evolvesFrom};{eReq.evoMethod};{eReq.minLevel};{eReq.gender};{eReq.minAffection};{eReq.minBeauty};{eReq.minHappiness};{eReq.location};{eReq.item};" +
                                    $"{eReq.heldItem};{eReq.knownMove};{eReq.knownMoveType};{eReq.needsOverworldRain};{eReq.partySpecies};{eReq.partyType};{eReq.relativePhysicalStats};{eReq.tradeSpecies};" +
                                    $"{eReq.turnUpsideDown};{eReq.timeOfDay}");
                            }
                            else
                            {
                                foreach (ChainLink link in evoChain.Chain.EvolvesTo)
                                {
                                    if (link.Species.Name == eReq.speciesName)
                                    {
                                        if (link.EvolutionDetails.FirstOrDefault().Trigger != null)
                                        {
                                            eReq.evoMethod = link.EvolutionDetails.FirstOrDefault().Trigger.Name;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().Gender != null)
                                        {
                                            eReq.gender = link.EvolutionDetails.FirstOrDefault().Gender.Value;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().HeldItem != null)
                                        {
                                            eReq.heldItem = link.EvolutionDetails.FirstOrDefault().HeldItem.Name;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().Item != null)
                                        {
                                            eReq.item = link.EvolutionDetails.FirstOrDefault().Item.Name;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().KnownMove != null)
                                        {
                                            eReq.knownMove = link.EvolutionDetails.FirstOrDefault().KnownMove.Name;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().KnownMoveType != null)
                                        {
                                            eReq.knownMoveType = link.EvolutionDetails.FirstOrDefault().KnownMoveType.Name;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().Location != null)
                                        {
                                            eReq.location = link.EvolutionDetails.FirstOrDefault().Location.Name;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().MinAffection != null)
                                        {
                                            eReq.minAffection = link.EvolutionDetails.FirstOrDefault().MinAffection.Value;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().MinBeauty != null)
                                        {
                                            eReq.minBeauty = link.EvolutionDetails.FirstOrDefault().MinBeauty.Value;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().MinHappiness != null)
                                        {
                                            eReq.minHappiness = link.EvolutionDetails.FirstOrDefault().MinHappiness.Value;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().MinLevel != null)
                                        {
                                            eReq.minLevel = link.EvolutionDetails.FirstOrDefault().MinLevel.Value;
                                        }
                                        eReq.needsOverworldRain = link.EvolutionDetails.FirstOrDefault().NeedsOverworldRain;
                                        if (link.EvolutionDetails.FirstOrDefault().PartySpecies != null)
                                        {
                                            eReq.partySpecies = link.EvolutionDetails.FirstOrDefault().PartySpecies.Name;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().PartyType != null)
                                        {
                                            eReq.partyType = link.EvolutionDetails.FirstOrDefault().PartyType.Name;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().RelativePhysicalStats != null)
                                        {
                                            eReq.relativePhysicalStats = link.EvolutionDetails.FirstOrDefault().RelativePhysicalStats.Value;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().TimeOfDay != null)
                                        {
                                            eReq.timeOfDay = link.EvolutionDetails.FirstOrDefault().TimeOfDay;
                                        }
                                        if (link.EvolutionDetails.FirstOrDefault().TradeSpecies != null)
                                        {
                                            eReq.tradeSpecies = link.EvolutionDetails.FirstOrDefault().TradeSpecies.Name;
                                        }
                                        eReq.turnUpsideDown = link.EvolutionDetails.FirstOrDefault().TurnUpsideDown;
                                        lastPokemon = eReq.speciesName;
                                        isValid = true;
                                        alreadyCovered.Add(lastPokemon);
                                        linesToWrite.Add($"{eReq.dexNumber};{eReq.speciesName};{eReq.evolvesFrom};{eReq.evoMethod};{eReq.minLevel};{eReq.gender};{eReq.minAffection};{eReq.minBeauty};{eReq.minHappiness};{eReq.location};{eReq.item};" +
                                            $"{eReq.heldItem};{eReq.knownMove};{eReq.knownMoveType};{eReq.needsOverworldRain};{eReq.partySpecies};{eReq.partyType};{eReq.relativePhysicalStats};{eReq.tradeSpecies};" +
                                            $"{eReq.turnUpsideDown};{eReq.timeOfDay}");
                                    }
                                    else
                                    {
                                        foreach (ChainLink link2 in link.EvolvesTo)
                                        {
                                            if (link2.Species.Name == eReq.speciesName)
                                            {
                                                if (link2.EvolutionDetails.FirstOrDefault().Trigger != null)
                                                {
                                                    eReq.evoMethod = link2.EvolutionDetails.FirstOrDefault().Trigger.Name;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().Gender != null)
                                                {
                                                    eReq.gender = link2.EvolutionDetails.FirstOrDefault().Gender.Value;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().HeldItem != null)
                                                {
                                                    eReq.heldItem = link2.EvolutionDetails.FirstOrDefault().HeldItem.Name;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().Item != null)
                                                {
                                                    eReq.item = link2.EvolutionDetails.FirstOrDefault().Item.Name;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().KnownMove != null)
                                                {
                                                    eReq.knownMove = link2.EvolutionDetails.FirstOrDefault().KnownMove.Name;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().KnownMoveType != null)
                                                {
                                                    eReq.knownMoveType = link2.EvolutionDetails.FirstOrDefault().KnownMoveType.Name;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().Location != null)
                                                {
                                                    eReq.location = link2.EvolutionDetails.FirstOrDefault().Location.Name;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().MinAffection != null)
                                                {
                                                    eReq.minAffection = link2.EvolutionDetails.FirstOrDefault().MinAffection.Value;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().MinBeauty != null)
                                                {
                                                    eReq.minBeauty = link2.EvolutionDetails.FirstOrDefault().MinBeauty.Value;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().MinHappiness != null)
                                                {
                                                    eReq.minHappiness = link2.EvolutionDetails.FirstOrDefault().MinHappiness.Value;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().MinLevel != null)
                                                {
                                                    eReq.minLevel = link2.EvolutionDetails.FirstOrDefault().MinLevel.Value;
                                                }
                                                eReq.needsOverworldRain = link2.EvolutionDetails.FirstOrDefault().NeedsOverworldRain;
                                                if (link2.EvolutionDetails.FirstOrDefault().PartySpecies != null)
                                                {
                                                    eReq.partySpecies = link2.EvolutionDetails.FirstOrDefault().PartySpecies.Name;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().PartyType != null)
                                                {
                                                    eReq.partyType = link2.EvolutionDetails.FirstOrDefault().PartyType.Name;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().RelativePhysicalStats != null)
                                                {
                                                    eReq.relativePhysicalStats = link2.EvolutionDetails.FirstOrDefault().RelativePhysicalStats.Value;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().TimeOfDay != null)
                                                {
                                                    eReq.timeOfDay = link2.EvolutionDetails.FirstOrDefault().TimeOfDay;
                                                }
                                                if (link2.EvolutionDetails.FirstOrDefault().TradeSpecies != null)
                                                {
                                                    eReq.tradeSpecies = link2.EvolutionDetails.FirstOrDefault().TradeSpecies.Name;
                                                }
                                                eReq.turnUpsideDown = link2.EvolutionDetails.FirstOrDefault().TurnUpsideDown;
                                                lastPokemon = eReq.speciesName;
                                                alreadyCovered.Add(lastPokemon);
                                                isValid = true;
                                                linesToWrite.Add($"{eReq.dexNumber};{eReq.speciesName};{eReq.evolvesFrom};{eReq.evoMethod};{eReq.minLevel};{eReq.gender};{eReq.minAffection};{eReq.minBeauty};{eReq.minHappiness};{eReq.location};{eReq.item};" +
                                                    $"{eReq.heldItem};{eReq.knownMove};{eReq.knownMoveType};{eReq.needsOverworldRain};{eReq.partySpecies};{eReq.partyType};{eReq.relativePhysicalStats};{eReq.tradeSpecies};" +
                                                    $"{eReq.turnUpsideDown};{eReq.timeOfDay}");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!isValid)
                        {
                            linesToWrite.Add($"{eReq.dexNumber};{eReq.speciesName};;;;;;;;;;;;;;;;;;");
                        }
*/