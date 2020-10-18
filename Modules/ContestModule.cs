using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using PokeApiNet;
using PokeParadise.Classes;
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
    [Group("Contest")]
    [Alias("c")]
    public class ContestModule : InteractiveBase<SocketCommandContext>
    {
        public readonly List<ulong> reactionMessageIds = new List<ulong>();
        public readonly Dictionary<ulong, int> contestPlayerIds = new Dictionary<ulong, int>();
        BaseModule bm = new BaseModule();
        [Command("Pageant", RunMode = RunMode.Async)]
        [Summary("Begins a Pageant contest! You may challenge between one and four people to a contest. Usage: >contest pageant [ID of the Pokemon you'll use] [Ping up to four people to join you. The rest will of the slots be filled in by AI]")]
        [Alias("p")]
        [RequireBegan()]
        public async Task ContestPageantCommand(string pkmnId, string target1 = null, string target2 = null, string target3 = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            SocketUser user1 = Context.User;
            SocketUser user2;
            SocketUser user3;
            SocketUser user4;
            List<SocketUser> participants = new List<SocketUser>();
            participants.Add(user1);
            List<SocketUser> mentionedUsers = Context.Message.MentionedUsers.ToList();
            if (target1 != null && target2 == null)
            {
               user2 = mentionedUsers.First();
                participants.Add(user2);
            }
            if (target1 != null && target2 != null && target3 == null)
            {
                user2 = mentionedUsers.First();
                user3 = mentionedUsers.ElementAt(1);
                participants.Add(user2);
                participants.Add(user3);
            }
            if (target1 != null && target2 != null && target3 != null)
            {
                user2 = mentionedUsers.First();
                user3 = mentionedUsers.ElementAt(1);
                user4 = mentionedUsers.Last();
                participants.Add(user2);
                participants.Add(user3);
                participants.Add(user4);
            }
            bool isValidPkmnId = Int32.TryParse(pkmnId, out int pId);
            if (isValidPkmnId)
            {
                Dictionary<ulong, Player> players = new Dictionary<ulong, Player>();
                foreach (SocketUser part in participants)
                {
                    Player player = bm.PlayerLoad(part.Id);
                    players.Add(part.Id, player);
                    sb.AppendLine($"<@{part.Id}>, please respond with the ID of the Pokemon you'd like to use to join the Contest!");
                }
                sb.AppendLine($"If any of those invited wish to not join the Contest, simply don't respond to the message, and you won't join.");
                embed.Description = sb.ToString();
                embed.Title = "Contest Beginning!";
                IUserMessage msg = await ReplyAsync(null, false, embed.Build());
                bool allHaveAnswered = false;
                TimeSpan timeout = new TimeSpan(0, 0, 60);
                Dictionary<ulong, bool> hasResponded = new Dictionary<ulong, bool>();
                Dictionary<Player, Pokemon> ps = new Dictionary<Player, Pokemon>();
                Player original = players[user1.Id];
                Pokemon oChose = new Pokemon();
                foreach (Pokemon x in original.pokemon)
                {
                    if (x.id == pId) { oChose = x; }
                }
                if (oChose.pkmnName != null)
                {
                    ps.Add(original, oChose);
                    hasResponded.Add(original.id, true);
                    foreach (KeyValuePair<ulong, Player> player in players)
                    {
                        if (!hasResponded.ContainsKey(player.Key))
                        {
                            hasResponded.Add(player.Key, false);
                        }
                    }
                    while (!allHaveAnswered && DateTimeOffset.Now - msg.Timestamp <= timeout)
                    {
                        var response = await NextMessageAsync(false);
                        if (response != null)
                        {
                            Player responder = new Player();
                            if (players.ContainsKey(response.Author.Id)) { responder = players[response.Author.Id]; }
                            bool isInt = Int32.TryParse(response.Content, out int chosenInt);
                            if (isInt)
                            {
                                foreach (Pokemon p in responder.pokemon)
                                {
                                    if (p.id == chosenInt)
                                    {
                                        hasResponded[responder.id] = true;
                                        if (!ps.ContainsKey(responder))
                                        {
                                            ps.Add(responder, p);
                                        }
                                    }
                                }
                                if (!hasResponded[responder.id])
                                {
                                    embed = new EmbedBuilder();
                                    sb = new StringBuilder();
                                    sb.AppendLine($"Sorry, {responder.name}, you don't have a Pokemon with that ID! Please try again later.");
                                    embed.Description = sb.ToString();
                                    embed.WithColor(new Color(247, 89, 213));
                                    embed.Title = $"Pokemon Not Found!";
                                    await ReplyAsync(null, false, embed.Build());
                                }
                                else
                                {
                                    embed = new EmbedBuilder();
                                    sb = new StringBuilder();
                                    sb.AppendLine($"{responder.name} has selected {ps[responder].nickname} for the Contest!");
                                    embed.Description = sb.ToString();
                                    embed.WithColor(new Color(247, 89, 213));
                                    embed.Title = $"Pokemon Selected!";
                                    await ReplyAsync(null, false, embed.Build());
                                }
                            }
                            else
                            {
                                embed = new EmbedBuilder();
                                sb = new StringBuilder();
                                sb.AppendLine($"Sorry, {responder.name}, that's not a valid Pokemon ID! Please try again later.");
                                embed.Description = sb.ToString();
                                embed.WithColor(new Color(247, 89, 213));
                                embed.Title = $"Invalid ID!";
                                await ReplyAsync(null, false, embed.Build());
                            }
                        }
                        int responseCount = 0;
                        foreach (KeyValuePair<ulong, Player> player in players)
                        {
                            if (hasResponded[player.Key])
                            {
                                responseCount++;
                            }
                            if (responseCount == players.Count())
                            {
                                allHaveAnswered = true;
                            }
                        }
                    }
                    embed = new EmbedBuilder();
                    sb = new StringBuilder();
                    foreach (KeyValuePair<Player, Pokemon> p in ps)
                    {
                        sb.AppendLine($"{p.Key.name} is enrolled in the Contest with their {p.Value.nickname}, a formiddable {p.Value.pkmnName}!");
                    }
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    embed.Title = $"The Contest is about to Begin!";
                    await ReplyAsync(null, false, embed.Build());
                    int cnt = 1;
                    ContestAudience ca = new ContestAudience();
                    Random rand = new Random();
                    int aiPokemonChoice1 = rand.Next(1, 891);
                    int aiPokemonChoice2 = rand.Next(1, 891);
                    int aiPokemonChoice3 = rand.Next(1, 891);
                    ca.lastRound = new Classes.Move[4];
                    ContestWrapper victor = new ContestWrapper();
                    while (cnt <= 5)
                    {
                        Pokemon fastest = new Pokemon();
                        int fastestSpeed = 0;
                        foreach (KeyValuePair<Player, Pokemon> p in ps)
                        {
                            if (p.Value.statSpd > fastestSpeed) { fastestSpeed = p.Value.statSpd; }
                            fastest = p.Value;
                        }
                        embed = new EmbedBuilder();
                        sb = new StringBuilder();
                        sb.AppendLine($"React to this message with the slot of the move you want to use in this round! If you don't use a move, you'll be skipped!");
                        embed.Description = sb.ToString();
                        embed.WithColor(new Color(247, 89, 213));
                        embed.Title = $"Round {cnt}: BEGIN!";
                        msg = await ReplyAsync(null, false, embed.Build());
                        Emoji one = new Emoji("1\u20e3");
                        Emoji two = new Emoji("2\u20e3");
                        Emoji three = new Emoji("3\u20e3");
                        Emoji four = new Emoji("4\u20e3");
                        await msg.AddReactionAsync(one);
                        await msg.AddReactionAsync(two);
                        await msg.AddReactionAsync(three);
                        await msg.AddReactionAsync(four);
                        reactionMessageIds.Add(msg.Id);
                        int cpidCnt = 1;
                        foreach (KeyValuePair<Player, Pokemon> x in ps)
                        {
                            if (!contestPlayerIds.ContainsKey(x.Key.id))
                            {
                                contestPlayerIds.Add(x.Key.id, cpidCnt);
                            }
                            cpidCnt++;
                        }
                        Dictionary<int, ContestWrapper> z = new Dictionary<int, ContestWrapper>();
                        int playerCounter = 1;
                        foreach (KeyValuePair<ulong, int> kvp in contestPlayerIds)
                        {
                            if (kvp.Value > 0)
                            {
                                foreach (KeyValuePair<Player, Pokemon> x in ps)
                                {
                                    if (x.Key.id == kvp.Key)
                                    {
                                        z.Add(playerCounter, new ContestWrapper(x.Key, x.Value, x.Value.moveSet[kvp.Value - 1]));
                                        playerCounter++;
                                    }
                                }
                            }
                        }
                        if (z.Count() < 4)
                        {
                            if (z.Count() == 1)
                            {
                                Player a = new Player("Brendan", 0);
                                Player b = new Player("Dawn", 0);
                                Player c = new Player("Brock", 0);
                                Pokemon aPoke = new Pokemon(aiPokemonChoice1);
                                Pokemon bPoke = new Pokemon(aiPokemonChoice2);
                                Pokemon cPoke = new Pokemon(aiPokemonChoice3);
                                aPoke.moveSet = await aPoke.GenerateMovesetAsync(aPoke.dexNo);
                                bPoke.moveSet = await bPoke.GenerateMovesetAsync(bPoke.dexNo);
                                cPoke.moveSet = await cPoke.GenerateMovesetAsync(cPoke.dexNo);
                                z.Add(2, new ContestWrapper(a, aPoke, aPoke.moveSet[rand.Next(0, 3)]));
                                z.Add(3, new ContestWrapper(b, bPoke, bPoke.moveSet[rand.Next(0, 3)]));
                                z.Add(4, new ContestWrapper(c, cPoke, cPoke.moveSet[rand.Next(0, 3)]));
                            }
                            if (z.Count() == 2)
                            {
                                Player a = new Player("Brendan", 0);
                                Player b = new Player("Dawn", 0);
                                Pokemon aPoke = new Pokemon(aiPokemonChoice1);
                                Pokemon bPoke = new Pokemon(aiPokemonChoice2);
                                aPoke.moveSet = await aPoke.GenerateMovesetAsync(aPoke.dexNo);
                                bPoke.moveSet = await bPoke.GenerateMovesetAsync(bPoke.dexNo);
                                z.Add(3, new ContestWrapper(a, aPoke, aPoke.moveSet[rand.Next(0, 3)]));
                                z.Add(4, new ContestWrapper(b, bPoke, bPoke.moveSet[rand.Next(0, 3)]));
                            }
                            if (z.Count() == 3)
                            {
                                Player a = new Player("Brendan", 0);
                                Pokemon aPoke = new Pokemon(aiPokemonChoice1);
                                aPoke.moveSet = await aPoke.GenerateMovesetAsync(aPoke.dexNo);
                                z.Add(4, new ContestWrapper(a, aPoke, aPoke.moveSet[rand.Next(0, 3)]));
                            }
                        }
                        timeout = new TimeSpan(0, 0, 30);
                        bool timeToMoveOn = false;
                        while (!timeToMoveOn)
                        {
                            if (DateTimeOffset.Now - msg.Timestamp >= timeout) { timeToMoveOn = true; }
                        }
                        if (timeToMoveOn)
                        {
                            IEnumerable<ContestWrapper> bySpeed = z.Values.OrderBy(x => x.pokemon.statSpd);
                            foreach (ContestWrapper cwx in z.Values)
                            {
                                int count = 0;
                                foreach (ContestWrapper cwy in bySpeed)
                                {
                                    if (cwy == cwx) { cwx.turnOrder = count; }
                                    count++;
                                }
                            }
                            embed = new EmbedBuilder();
                            Interactive.RemoveReactionCallback(msg);
                            embed = await MoveHandlerAsync(z, ca);
                            await ReplyAsync(null, false, embed.Build());
                            if (cnt == 5)
                            {
                                if (ca.appeal1 > ca.appeal2 && ca.appeal1 > ca.appeal3 && ca.appeal1 > ca.appeal4) { victor = z[0]; }
                                else if (ca.appeal2 > ca.appeal1 && ca.appeal2 > ca.appeal3 && ca.appeal2 > ca.appeal4) { victor = z[1]; }
                                else if (ca.appeal3 > ca.appeal1 && ca.appeal3 > ca.appeal2 && ca.appeal3 > ca.appeal4) { victor = z[2]; }
                                else if (ca.appeal4 > ca.appeal1 && ca.appeal4 > ca.appeal2 && ca.appeal4 > ca.appeal3) { victor = z[3]; }
                            }
                            cnt++;
                        }
                    }
                    embed = new EmbedBuilder();
                    sb = new StringBuilder();
                    sb.AppendLine($"The winner is {victor.owner.name} and their {victor.pokemon.pkmnName}, {victor.pokemon.nickname}!");
                    embed.Title = "Contest End!";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());

                }
                else
                {
                    embed = new EmbedBuilder();
                    sb = new StringBuilder();
                    sb.AppendLine($"You don't have a Pokemon with that ID!");
                    embed.Title = "Pokemon Not Found";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());
                }
            }
            else
            {
                sb.AppendLine($"The Pokemon ID you specified is invalid! Please enter a digit (0-9)!");
                embed.Title = "Invalid Pokemon ID";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }

        public void PlayerMoveChoice(SocketReaction reaction, IUserMessage msg)
        {
            int moveChoice = 0;
            var emote = reaction.Emote;
            IEmote one = new Emoji("\u0031");
            IEmote two = new Emoji("\u0031");
            IEmote three = new Emoji("\u0031");
            IEmote four = new Emoji("\u0031");
            if (emote.Equals(one))
            {
                moveChoice = 1;
            }
            else if (emote.Equals(two))
            {
                moveChoice = 2;
            }
            else if (emote.Equals(three))
            {
                moveChoice = 3;
            }
            else if (emote.Equals(four))
            {
                moveChoice = 4;
            }
            if (moveChoice > 0)
            {
                contestPlayerIds[reaction.UserId] = moveChoice;
            }
            _ = msg.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
        }

        public async Task<EmbedBuilder> MoveHandlerAsync(Dictionary<int, ContestWrapper> dcw, ContestAudience ca)
        {
            foreach (ContestWrapper cwz in dcw.Values)
            {
                if (cwz.nervous) { cwz.nervous = false; }
                if (cwz.moveLockRound) { cwz.moveLockRound = false; }
                if (cwz.startlePreventRound) { cwz.startlePreventRound = false; }
            }
            ca.exciteLock = false;
            PokeApiClient pokeClient = new PokeApiClient();
            int turnOrder1 = 0;
            int turnOrder2 = 0;
            int turnOrder3 = 0;
            int turnOrder4 = 0;
            foreach (ContestWrapper y in dcw.Values)
            {
                if (y.turnOrder == 1) { turnOrder1 = y.turnOrder; }
                else if (y.turnOrder == 2) { turnOrder2 = y.turnOrder; }
                else if (y.turnOrder == 3) { turnOrder3 = y.turnOrder; }
                else if (y.turnOrder == 4) { turnOrder4 = y.turnOrder; }
            }           
            EmbedBuilder embed = new EmbedBuilder();
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            int totalAppeal = 0;
            int totalMoves = 0;
            int cnt = 1;
            while (cnt <= 4)
            {
                foreach (KeyValuePair<int, ContestWrapper> kvp in dcw)
                {
                    if (kvp.Value.turnOrder == cnt)
                    {
                        if (kvp.Value.pumpLevel > 0) { kvp.Value.move.appeal += kvp.Value.pumpLevel; }
                        switch (kvp.Value.move.effect)
                        {
                            case "A very appealing move, but after using this move, the user is more easily startled.":
                                kvp.Value.startleChance--;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! They look more prone to startling now, and their move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "An appealing move that can be used repeatedly without boring the audience.":
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! They look like they're in a groove now, and their move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Badly startles Pokémon that the audience has high expectations of.":
                                foreach (KeyValuePair<int, ContestWrapper> x in dcw)
                                {
                                    if (x.Key == 1) { if (ca.appeal1 >= 5) { x.Value.move.appeal -= kvp.Value.move.jam; } }
                                    if (x.Key == 2) { if (ca.appeal2 >= 5) { x.Value.move.appeal -= kvp.Value.move.jam; } }
                                    if (x.Key == 3) { if (ca.appeal3 >= 5) { x.Value.move.appeal -= kvp.Value.move.jam; } }
                                    if (x.Key == 4) { if (ca.appeal4 >= 5) { x.Value.move.appeal -= kvp.Value.move.jam; } }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! Pokemon that have performed well so far may be startled, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Badly startles Pokémon that used a move of the same type.":
                                PokeApiNet.Move kvpMove = await pokeClient.GetResourceAsync<PokeApiNet.Move>(kvp.Value.move.name);
                                string moveType = kvpMove.Type.Name;
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    PokeApiNet.Move xMove = await pokeClient.GetResourceAsync<PokeApiNet.Move>(x.move.name);
                                    string xType = xMove.Type.Name;
                                    if (moveType == xType)
                                    {
                                        x.move.appeal -= kvp.Value.move.jam;
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                break;
                            case "Brings down the energy of any Pokémon that have already used a move this turn.":
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (x.turnOrder < kvp.Value.turnOrder) { x.startleChance--; x.nervousChance--; }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! Pokemon that have gone before now look more tired, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Causes the user to move earlier on the next turn.":
                                if (kvp.Key == 1) { turnOrder1++; turnOrder2--; turnOrder3--; turnOrder4--; }
                                else if (kvp.Key == 2) { turnOrder1--; turnOrder2++; turnOrder3--; turnOrder4--; }
                                else if (kvp.Key == 3) { turnOrder1--; turnOrder2--; turnOrder3++; turnOrder4--; }
                                else if (kvp.Key == 4) { turnOrder1--; turnOrder2--; turnOrder3--; turnOrder4++; }
                                else if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! They look more alert, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Causes the user to move later on the next turn.":
                                if (kvp.Key == 1) { turnOrder1--; turnOrder2++; turnOrder3++; turnOrder4++; }
                                else if (kvp.Key == 2) { turnOrder1++; turnOrder2--; turnOrder3++; turnOrder4++; }
                                else if (kvp.Key == 3) { turnOrder1++; turnOrder2++; turnOrder3--; turnOrder4++; }
                                else if (kvp.Key == 4) { turnOrder1++; turnOrder2++; turnOrder3++; turnOrder4--; }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! They look like they're waiting for something, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Effectiveness varies depending on when it is used.":
                                if (kvp.Value.turnOrder == 1) { kvp.Value.move.appeal++; }
                                if (kvp.Value.turnOrder == 3) { kvp.Value.move.appeal += 2; ; }
                                if (kvp.Value.turnOrder == 4) { kvp.Value.move.appeal--; }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Excites the audience a lot if used last.":
                                bool isLast = true;
                                foreach (KeyValuePair<int, ContestWrapper> kvp2 in dcw)
                                {
                                    if (kvp2.Value.turnOrder > kvp.Value.turnOrder) { isLast = false; }
                                }
                                int appeal = kvp.Value.move.appeal;
                                if (isLast)
                                {
                                    appeal += 3;
                                    sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The audience is roaring in appreciation, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                }
                                else
                                {
                                    sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += appeal; }
                                    totalAppeal += appeal;
                                }
                                break;
                            case "Excites the audience in any kind of contest.":
                                ca.excitement++;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The audience looks much more excited, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Gets the Pokémon pumped up. Helps prevent nervousness, too.":
                                kvp.Value.pumpLevel++;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! They look pumped up, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Makes audience expect little of other contestants.":
                                if (kvp.Key == 1) { ca.expect2 /= 2; ca.expect3 /= 2; ca.expect4 /= 2; }
                                if (kvp.Key == 2) { ca.expect3 /= 2; ca.expect4 /= 2; ca.expect1 /= 2; }
                                if (kvp.Key == 3) { ca.expect4 /= 2; ca.expect1 /= 2; ca.expect2 /= 2; }
                                if (kvp.Key == 4) { ca.expect1 /= 2; ca.expect2 /= 2; ca.expect3 /= 2; }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The audience looks unimpressed by the other Pokemon, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Makes the audience quickly grow bored when an appeal move has little effect.":
                                ca.isPicky = true;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The audience looks more picky now, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Makes the remaining Pokémon nervous.":
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    x.nervous = true;
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The other Pokemon now look nervous, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Prevents the user from being startled one time this turn.":
                                kvp.Value.startlePrevent1 = true;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! They look like they're very focused, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Prevents the user from being startled until the turn ends.":
                                kvp.Value.startlePreventRound = true;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                    sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! They look like they're ignoring all other Pokemon, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                }
                                break;
                            case "Quite an appealing move.":
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}!  {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Shows off the Pokémon's appeal about as well as all the moves before it this turn.":
                                kvp.Value.move.appeal = totalAppeal / totalMoves;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Shows off the Pokémon's appeal about as well as the move used just before it.":
                                ContestWrapper selected = new ContestWrapper();
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (x.turnOrder - 1 == kvp.Value.turnOrder)
                                    {
                                        kvp.Value.move.appeal = x.move.appeal;
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Startles all other Pokémon. User cannot act in the next turn.":
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (x != kvp.Value)
                                    {
                                        int startleRoll = rand.Next(1, 10);
                                        startleRoll -= x.startleChance;
                                        if (startleRoll > 5) { x.move.appeal -= kvp.Value.move.jam; }
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                    kvp.Value.moveLockRound = true;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! They look like they need to rest for the next turn. {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Startles the last Pokémon to act before the user.":
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (x.turnOrder - 1 == kvp.Value.turnOrder)
                                    {
                                        int startleRoll = rand.Next(1, 10);
                                        startleRoll -= x.startleChance;
                                        if (startleRoll > 5) { x.move.appeal -= kvp.Value.move.jam; }
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The Pokemon that went just before now looks startled, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Temporarily stops the crowd from growing excited.":
                                ca.exciteLock = true;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The crowd is watching in suspense, and {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Works better the later it is used in a turn.":
                                if (kvp.Value.turnOrder == 1) { kvp.Value.move.appeal--; }
                                if (kvp.Value.turnOrder == 3) { kvp.Value.move.appeal++; }
                                if (kvp.Value.turnOrder == 4) { kvp.Value.move.appeal += 2; }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Works better the more the crowd is excited.":
                                kvp.Value.move.appeal += ca.excitement;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Works great if the user goes first this turn.":
                                if (kvp.Value.turnOrder == 1) { kvp.Value.move.appeal += 3; }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Works great if the user goes last this turn.":
                                if (kvp.Value.turnOrder == dcw.Count()) { kvp.Value.move.appeal += 3; }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Works well if the user is pumped up.":
                                if (kvp.Value.pumpLevel > 0) { kvp.Value.move.appeal += 2; }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "A move of huge appeal, but using it prevents the user from taking further contest moves.":
                                kvp.Value.moveLockContest = true;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! They look like they're completely exhausted, and can't use any more moves! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Badly startles all of the Pokémon to act before the user.":
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (x.turnOrder < kvp.Value.turnOrder)
                                    {
                                        int startleRoll = rand.Next(1, 10);
                                        startleRoll -= x.startleChance;
                                        if (startleRoll > 5) { x.move.appeal -= kvp.Value.move.jam; }
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! All Pokemon who went before now look startled! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Badly startles all Pokémon that successfully showed their appeal.":
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (!x.nervous && !x.moveLockContest && !x.moveLockRound)
                                    {
                                        int startleRoll = rand.Next(1, 10);
                                        startleRoll -= x.startleChance;
                                        if (startleRoll > 5) { x.move.appeal -= kvp.Value.move.jam; }
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The Pokemon who showed off the most this round look startled! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Badly startles the last Pokémon to act before the user.":
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (x.turnOrder - 1 == kvp.Value.turnOrder)
                                    {
                                        int startleRoll = rand.Next(1, 10);
                                        startleRoll -= x.startleChance;
                                        if (startleRoll > 5) { x.move.appeal -= kvp.Value.move.jam; }
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The Pokemon who just went before them looks startled! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Excites the audience a lot if used first.":
                                if (kvp.Value.turnOrder == 1)
                                {
                                    ca.excitement += 2;
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Scrambles the order in which Pokémon will move on the next turn.":
                                turnOrder1 = 3;
                                turnOrder2 = 1;
                                turnOrder3 = 4;
                                turnOrder4 = 2;
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! The Pokemon's move order switched! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Works well if it is the same type as the move used by the last Pokémon.":
                                PokeApiNet.Move thisMove = await pokeClient.GetResourceAsync<PokeApiNet.Move>(kvp.Value.move.name);
                                string thisType = thisMove.Type.Name;
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (x.turnOrder == kvp.Value.turnOrder - 1)
                                    {
                                        PokeApiNet.Move xMove = await pokeClient.GetResourceAsync<PokeApiNet.Move>(x.move.name);
                                        string xType = xMove.Type.Name;
                                        if (thisType == xType)
                                        {
                                            kvp.Value.move.appeal += 2;
                                        }
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Affected by how well the previous Pokémon's move went.":
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (x.turnOrder - 1 == kvp.Value.turnOrder)
                                    {
                                        if (x.move.appeal >= 2) { kvp.Value.move.appeal++; } else { kvp.Value.move.appeal--; }
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                            case "Startles all of the Pokémon to act before the user.":
                                foreach (ContestWrapper x in dcw.Values)
                                {
                                    if (x.turnOrder < kvp.Value.turnOrder)
                                    {
                                        int startleRoll = rand.Next(1, 10);
                                        startleRoll -= x.startleChance;
                                        if (startleRoll > 5) { x.move.appeal -= kvp.Value.move.jam; }
                                    }
                                }
                                if (!kvp.Value.nervous && !kvp.Value.moveLockContest && !kvp.Value.moveLockRound)
                                {
                                    if (kvp.Key == 1) { ca.appeal1 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 2) { ca.appeal2 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 3) { ca.appeal3 += kvp.Value.move.appeal; }
                                    else if (kvp.Key == 4) { ca.appeal4 += kvp.Value.move.appeal; }
                                    totalAppeal += kvp.Value.move.appeal;
                                }
                                sb.AppendLine($"{kvp.Value.pokemon.nickname} used {kvp.Value.move.name}! All Pokemon who went before them in this round look startled! {kvp.Value.pokemon.nickname}'s move had {kvp.Value.move.appeal} appeal!");
                                break;
                        }
                        totalMoves++;
                        cnt++;
                    }
                }
            }
            ca.expect1 = ca.appeal1 / 2 + ca.excitement;
            ca.expect2 = ca.appeal2 / 2 + ca.excitement;
            ca.expect3 = ca.appeal3 / 2 + ca.excitement;
            ca.expect4 = ca.appeal4 / 2 + ca.excitement;
            if (ca.lastRound[0] != null)
            {
                if (ca.lastRound[0].name == dcw[0].move.name) { if (ca.isPicky) { ca.expect1 /= 3; ca.appeal1 /= 3; } else { ca.expect1 /= 2; ca.appeal1 /= 2; } }
            }
            if (ca.lastRound[1] != null)
            {
                if (ca.lastRound[1].name == dcw[1].move.name) { if (ca.isPicky) { ca.expect2 /= 3; ca.appeal2 /= 3; } else { ca.expect2 /= 2; ca.appeal2 /= 2; } }
            }
            if (ca.lastRound[2] != null)
            {
                if (ca.lastRound[2].name == dcw[2].move.name) { if (ca.isPicky) { ca.expect3 /= 3; ca.appeal3 /= 3; } else { ca.expect3 /= 2; ca.appeal3 /= 2; } }
            }
            if (ca.lastRound[3] != null)
            {
                if (ca.lastRound[3].name == dcw[3].move.name) { if (ca.isPicky) { ca.expect4 /= 3; ca.appeal4 /= 3; } else { ca.expect4 /= 2; ca.appeal4 /= 2; } }
            }
            dcw[1].turnOrder = turnOrder1;
            dcw[2].turnOrder = turnOrder2;
            dcw[3].turnOrder = turnOrder3;
            dcw[4].turnOrder = turnOrder4;
            if (!ca.exciteLock) { ca.excitement++; }
            ca.lastRound[0] = dcw[1].move;
            ca.lastRound[1] = dcw[2].move;
            ca.lastRound[2] = dcw[3].move;
            ca.lastRound[3] = dcw[4].move;
            embed.Title = "Results";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            return embed;
        }
    }
}
