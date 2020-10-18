using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using PokeParadise.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Modules
{
    [Group("Center")]
    [Alias("cntr", "ctr")]
    [RequireLevel10()]
    public class CenterModule : InteractiveBase<SocketCommandContext>
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
        [Command("Buy", RunMode = RunMode.Async)]
        [Summary("Allows you to buy the land for your Breeding Center! Usage: >center buy")]
        [RequireBegan()]
        public async Task CenterBuyCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            if (player.coins >= 2500 && player.breedingCenterUpgrades == 0)
            {
                sb.AppendLine($"You fish the flier out of your pocket and call the number along the bottom.");
                sb.AppendLine($"A warm, aged voice is on the other end. \"Hello? This is Joseph Arnold.\" You explain that you're interested in the farm, and his voice brightens up immensely.");
                sb.AppendLine($"\"Wonderful!\" he says. \"Tell me about your experience with Pokemon care.\" You spend the next 15 minutes explaining how you care for your eggs and Pokemon.");
                sb.AppendLine($"As you finish, he laughs. \"Absolutely wonderful, haha! Meet with me at the farm tomorrow, and I'll sign over the deed.\" He gives you directions, and you hang up, almost shaking with your excitement.");
                sb.AppendLine($"");
                sb.AppendLine($"*The Next Day...*");
                sb.AppendLine($"");
                sb.AppendLine($"You drive up to the farm and see Joseph waiting for you. He gives you a quick tour, and it seems that he can hardly contain his excitement, either. When it finally comes time, he hands the deed over to you.");
                sb.AppendLine($"\"Good luck,\" he says. \"I know you'll do great things with the place!\". You hand him the 2500 coins, and he goes off to enjoy his retirement.");
                sb.AppendLine($"");
                sb.AppendLine($"[Congratulations! You've bought the farm! You can now name it with >center name, and encourage compatible Pokemon to breed with >center breed!]");
                player.coins -= 2500;
                player.breedingCenterUpgrades += 1;
                bm.PlayerSave(player);
            }
            else if (player.coins < 2500 && player.breedingCenterUpgrades == 0)
            {
                sb.AppendLine($"You fish the flier out of your pocket and get your phone to call the number, but you realize before you do that you don't *have* 2500 coins. Where are you gonna get that much money?...");
            }
            else
            {
                sb.AppendLine($"You already bought the farm!");
            }
            embed.Title = "Center Purchase";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Name", RunMode = RunMode.Async)]
        [Summary("Allows you to name your Breeding Center! Usage: >center name [name]")]
        [RequireBegan()]
        [RequireCenter()]
        public async Task CenterNameCommand(string name, string name2 = null, string name3 = null, string name4 = null, string name5 = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            if (name2 != null) { name += " "; name += name2.Substring(0, 1).ToUpper() + name2.Substring(1); }
            if (name3 != null) { name += " "; name += name3.Substring(0, 1).ToUpper() + name3.Substring(1); }
            if (name4 != null) { name += " "; name += name4.Substring(0, 1).ToUpper() + name4.Substring(1); }
            if (name5 != null) { name += " "; name += name5.Substring(0, 1).ToUpper() + name5.Substring(1); }
            Player player = bm.PlayerLoad(user.Id);
            player.breedingCenterName = name;
            sb.AppendLine($"You named your Breeding Center {name} Breeding Center!");
            embed.Title = "Naming the Farm";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Breed", RunMode = RunMode.Async)]
        [Summary("Allows you to set your Pokemon to breed! Depending on their stats, it may take a while. Feel free to do other things, and check back in with >center check! Usage: >center breed [parent 1 ID] [parent 2 ID] [If you want to keep the parents breeding after they lay an egg, put \"true\" or \"yes\" here! Otherwise, leave it blank!]")]
        [RequireBegan()]
        [RequireCenter()]
        public async Task CenterBreedCommand(string input1, string input2, string stay = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            bool validP1 = Int32.TryParse(input1, out int parent1ID);
            bool validP2 = Int32.TryParse(input2, out int parent2ID);
            bool parentsStay = false;
            if (stay != null)
            {
                parentsStay = true;
            }
            Pokemon parent1 = new Pokemon();
            bool hasP1 = false;
            Pokemon parent2 = new Pokemon();
            bool hasP2 = false;
            if (validP1 && validP2)
            {
                if (parent1ID != parent2ID)
                {
                    foreach (Pokemon p in player.pokemon)
                    {
                        if (p.id == parent1ID)
                        {
                            parent1 = p;
                            hasP1 = true;
                        }
                        if (p.id == parent2ID)
                        {
                            parent2 = p;
                            hasP2 = true;
                        }
                    }
                    if (hasP1 && hasP2)
                    {
                        if (!parent1.isBreeding && !parent2.isBreeding)
                        {
                            if (parent1.eggGroup1 == parent2.eggGroup1 || parent1.eggGroup1 == parent2.eggGroup2 || parent1.eggGroup2 == parent2.eggGroup1 || parent1.eggGroup2 == parent2.eggGroup2)
                            {
                                Egg e = new Egg(player, 0, DateTimeOffset.Now, parent1, parent2, parentsStay);
                                if (player.breedingEggs == null)
                                {
                                    player.breedingEggs = new List<Egg>();
                                }
                                foreach (Pokemon p in player.pokemon)
                                {
                                    if (parent1.id == p.id || parent2.id == p.id)
                                    {
                                        p.isBreeding = true;
                                    }
                                }
                                player.breedingEggs.Add(e);
                                bm.PlayerSave(player);
                                sb.AppendLine($"{parent1.nickname} and {parent2.nickname} are breeding! They'll have offpsring ready for you in a little while. Check the breeding status with >center check!");
                                embed.Title = "Success!";
                            }
                            else
                            {
                                sb.AppendLine($"{parent1.nickname} and {parent2.nickname} don't share any egg groups!");
                                embed.Title = "No Matching Egg Groups!";
                            }
                        }
                        else
                        {
                            sb.AppendLine($"One or both of the breeding Pokemon selected are already breeding in another stall!");
                            embed.Title = "Pokemon Busy";
                        }
                    }
                    else
                    {
                        if (!hasP1)
                        {
                            sb.AppendLine($"No Pokemon with ID {parent1.id} found!");
                        }
                        if (!hasP2)
                        {
                            sb.AppendLine($"No Pokemon with ID {parent2.id} found!");
                        }
                        embed.Title = "Pokemon Not Owned";
                    }
                }
                else
                {
                    sb.AppendLine($"You can't breed a Pokemon with itself!");
                    embed.Title = "Invalid Inputs";
                }
            }
            else
            {
                if (!validP1)
                {
                    sb.AppendLine($"{input1} was not in a valid format! Please enter the ID in digits (0-9).");
                }
                if (!validP2)
                {
                    sb.AppendLine($"{input2} was not in a valid format! Please enter the ID in digits (0-9).");
                }
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Check", RunMode = RunMode.Async)]
        [Summary("Allows you to check your Breeding Center for Eggs! Make sure to do this to retrieve an Egg after breeding! Usage: >center check")]
        [RequireBegan()]
        [RequireCenter()]
        public async Task CenterCheckCommand()
        {
            var sb = new StringBuilder();
            EmbedFieldBuilder efb = new EmbedFieldBuilder();
            List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            if (player.breedingEggs == null)
            {
                player.breedingEggs = new List<Egg>();
            }
            bm.PlayerSave(player);
            List<Egg> toAdd = new List<Egg>();
            int cnt = 1;
            foreach (Egg e in player.breedingEggs)
            {
                TimeSpan diff = DateTimeOffset.Now - e.bred;
                TimeSpan timeRemaining = e.timeToBreed - diff;
                if (diff > e.timeToBreed)
                {
                    efb.Name = $"**Stall #{cnt}**";
                    if (!e.parentsStay)
                    {
                        efb.Value = $"Your {e.pkmn.parent1.nickname} and {e.pkmn.parent2.nickname} have laid an Egg! You gently take their egg and bring it to the hatchery. Your Pokemon then walk out to the grass to graze and relax!";
                        foreach (Pokemon p in player.pokemon)
                        {
                            if (p.id == e.pkmn.parent1.id || p.id == e.pkmn.parent2.id)
                            {
                                p.isBreeding = false;
                            }
                        }
                    }
                    else
                    {
                        efb.Value = $"Your {e.pkmn.parent1.nickname} and {e.pkmn.parent2.nickname} have laid an Egg! You gently take their egg and bring it to the hatchery. Your Pokemon coo and nuzzle each other as you leave.";
                        Egg x = new Egg(e.pkmn.parent1, e.pkmn.parent2, DateTimeOffset.Now, 0);
                        TimeSpan toBreed = x.timeToBreed;
                        x.parentsStay = e.parentsStay;
                        toAdd.Add(x);
                    }
                    List<int> allIds = new List<int>();
                    foreach (Egg x in player.eggs)
                    {
                        allIds.Add(x.eggId);
                    }
                    int largestId = 0;
                    if (allIds.Count > 0)
                    {
                        largestId = allIds.Max();
                    }
                    e.obtained = DateTimeOffset.Now;
                    e.eggId = largestId + 1;
                    player.eggs.Add(e);
                    bm.PlayerSave(player);
                    pages.Add(efb);
                    cnt++;
                }
                else { efb.Name = $"**Stall #{cnt}**"; efb.Value = $"Your {e.pkmn.parent1.nickname} and {e.pkmn.parent2.nickname} haven't laid an Egg yet. There's still **{timeRemaining.Minutes}m{timeRemaining.Seconds}s** left before they will!"; pages.Add(efb); }
            }
            if (player.breedingEggs.Count <= 0)
            {
                efb.Name = $"**None of your Pokemon are breeding!**";
                efb.Value = "Use the >center breed command to start the process!";
                pages.Add(efb);
            }
            foreach (Egg y in toAdd)
            {
                player.breedingEggs.Add(y);
            }
            bm.PlayerSave(player);
            PaginatedMessage msg = new PaginatedMessage();
            msg.Title = "Breeding Stalls";
            msg.Color =new Color(247, 89, 213);
            msg.Pages = pages;
            await PagedReplyAsync(msg);
        }

        [Command("RemoveParents", RunMode = RunMode.Async)]
        [Summary("This command takes all Pokemon out of the Breeding Center, or just the two you specify by ID! Note: They'll finish their current breeding session first! Usage: >center removeparents {parent 1 id} {parent 2 id}")]
        [RequireBegan()]
        [RequireCenter()]
        public async Task CenterRemoveParentsCommand(string input1 = null, string input2 = null)
        {
            var user = Context.User;
            EmbedFieldBuilder efb = new EmbedFieldBuilder();
            List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
            Player player = bm.PlayerLoad(user.Id);
            bool hasSpecifiedID1 = Int32.TryParse(input1, out int parent1ID);
            bool hasSpecifiedID2 = Int32.TryParse(input2, out int parent2ID);
            if (hasSpecifiedID1 && hasSpecifiedID2)
            {
                bool hasP1 = false;
                bool hasP2 = false;
                foreach (Pokemon p in player.pokemon)
                {
                    if (p.id == parent1ID)
                    {
                        hasP1 = true;
                    }
                    if (p.id == parent2ID)
                    {
                        hasP2 = true;
                    }
                }
                if (hasP1 && hasP2)
                {
                    bool parentsAreBreeding = false;
                    foreach (Egg e in player.eggs)
                    {
                        if (e.pkmn.parent1.id == parent1ID && e.pkmn.parent2.id == parent2ID)
                        {
                            e.parentsStay = false;
                            bm.PlayerSave(player);
                            efb.Name = $"Breeding Stall";
                            efb.Value = $"{e.pkmn.parent1.nickname} and {e.pkmn.parent2.nickname} will leave Breeding Center after they lay their next egg!";
                            parentsAreBreeding = true;
                            pages.Add(efb);
                            efb = new EmbedFieldBuilder();
                        }
                    }
                    if (!parentsAreBreeding)
                    {
                        efb.Name = $"Those Pokemon aren't breeding!";
                        efb.Value = $"Either those Pokemon aren't breeding together, or they're not in the Breeding Center at all! Please try again.";
                    }
                }
                else
                {
                    efb.Name = $"IDs not found!";
                    efb.Value = $"One or more of the IDs you specfied didn't match any of your Pokemon! Please try again.";
                }
            }
            else if (!hasSpecifiedID1 && !hasSpecifiedID2)
            {
                int cnt = 0;
                foreach (Egg e in player.eggs)
                {
                    efb.Name = $"Stall #{cnt}";
                    efb.Value = $"{e.pkmn.parent1.nickname} and {e.pkmn.parent2.nickname} will leave the Breeding Center after they lay their next egg!";
                    e.parentsStay = false;
                    bm.PlayerSave(player);
                    pages.Add(efb);
                    efb = new EmbedFieldBuilder();
                    cnt++;
                }
                if (cnt == 0)
                {
                    efb.Name = $"**No Pokemon Found**";
                    efb.Value = $"No Pokemon are currently breeding in the Center!";
                    pages.Add(efb);
                }
            }
            else if ((hasSpecifiedID1 && !hasSpecifiedID2) || (!hasSpecifiedID1 && hasSpecifiedID2))
            {
            }
            PaginatedMessage msg = new PaginatedMessage();
            msg.Color = new Color(247, 89, 213);
            msg.Pages = pages;
            await PagedReplyAsync(msg);
        }
    }
}
