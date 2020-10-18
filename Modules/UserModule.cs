using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Modules
{
    [Group("Breeder")]
    [Alias("my", "user", "u")]
    public class UserModule : InteractiveBase<SocketCommandContext>
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

        [Command("Stats", RunMode = RunMode.Async)]
        [Summary("Checks your player stats! Usage: >breeder stats")]
        [RequireBegan()]
        public async Task PlayerStatsCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            sb.AppendLine($"**Level**: {player.level}");
            sb.AppendLine($"**Experience**: {player.xp}");
            sb.AppendLine($"**Coins**: {player.coins}");
            sb.AppendLine($"**Eggs**: {player.eggs.Count}");
            sb.AppendLine($"**Pokemon**: {player.pokemon.Count}");
            sb.AppendLine($"**Skill Points:** {player.skillPoints}");
            sb.AppendLine($"");
            sb.AppendLine($"**Skill Scores**");
            sb.AppendLine($"**Incubation**: {player.eggHatchSpeedMult}");
            sb.AppendLine($"**Appraisal**: {player.legendaryMult}");
            sb.AppendLine($"**Perception**: {player.eggMult}");
            sb.AppendLine($"**Coaching**: {player.trainingMult}");
            sb.AppendLine($"**Care**: {player.affectionMult}");
            sb.AppendLine($"**Husbandry**: {player.breedingTimerMult}");
            sb.AppendLine($"**Cooking**: {player.cookingSuccessMult}");
            embed.Title = $"{player.name}'s Stats";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Inventory", RunMode = RunMode.Async)]
        [Alias("inv")]
        [Summary("Shows you your inventory! Any items you've bought or earned will go here. Usage: >breeder inventory")]
        [RequireBegan()]
        public async Task InventoryCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            List<Item> toRemove = new List<Item>();
            foreach (Item f in player.inventory)
            {
                if (f.qty <= 0)
                {
                    toRemove.Add(f);
                }
            }
            foreach (Item f in toRemove)
            {
                player.inventory.Remove(f);
            }
            bm.PlayerSave(player);
            foreach (Item i in player.inventory)
            {
                sb.AppendLine($"{i.name} x {i.qty}");
            }
            embed.Title = $"{player.name}'s Inventory";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Lunchbox", RunMode = RunMode.Async)]
        [Alias("lb")]
        [Summary("Shows all the snacks you've cooked for your Pokemon! Usage >breeder lunchbox")]
        [RequireBegan()]
        public async Task LunchboxCommand()
        {
            EmbedFieldBuilder efb = new EmbedFieldBuilder();
            List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            List<Food> toRemove = new List<Food>();
            foreach (Food f in player.lunchBox)
            {
                if (f.qty <= 0)
                {
                    toRemove.Add(f);
                }
            }
            foreach (Food f in toRemove)
            {
                player.lunchBox.Remove(f);
            }
            bm.PlayerSave(player);
            foreach (Food f in player.lunchBox)
            {
                if (f.level > 0)
                {
                    efb = new EmbedFieldBuilder();
                    efb.Name = $"**{f.qty} x {f.type}**";
                    efb.Value = $"*{f.category} (Level {f.level})*";                    
                }
                else
                {
                    efb = new EmbedFieldBuilder();
                    efb.Name = $"**{f.qty} x {f.type}**";
                    efb.Value = $"*{f.category}*";
                }
                pages.Add(efb);
            }
            PaginatedMessage msg = new PaginatedMessage();
            msg.Pages = pages;
            msg.Title = $"{player.name}'s Lunchbox";
            msg.Color = new Color(247, 89, 213);
            msg.Options.Timeout = null;
            await PagedReplyAsync(msg);
        }

        [Command("BerryPouch", RunMode = RunMode.Async)]
        [Alias("bp")]
        [Summary("Shows all the berries you and your Pokemon have gathered! Usage >breeder berrypouch")]
        [RequireBegan()]
        public async Task BerryPouchCommand()
        {
            EmbedFieldBuilder efb = new EmbedFieldBuilder();
            List<EmbedFieldBuilder> pages = new List<EmbedFieldBuilder>();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            List<Berry> toRemove = new List<Berry>();
            foreach (Berry f in player.berryPouch)
            {
                if (f.qty <= 0)
                {
                    toRemove.Add(f);
                }
            }
            foreach (Berry f in toRemove)
            {
                player.berryPouch.Remove(f);
            }
            bm.PlayerSave(player);
            foreach (Berry b in player.berryPouch)
            {
                efb = new EmbedFieldBuilder();
                efb.Name = $"**{b.name} Berry**";
                string flavor = "None";
                Dictionary<string, int> flavorProfile = new Dictionary<string, int>();
                flavorProfile.Add("Sweet", b.sweetValue);
                flavorProfile.Add("Sour", b.sourValue);
                flavorProfile.Add("Spicy", b.spicyValue);
                flavorProfile.Add("Dry", b.dryValue);
                flavorProfile.Add("Bitter",b.bitterValue);
                int valueOfMaxFlavor = flavorProfile.Values.Max();
                if (valueOfMaxFlavor != 0)
                {
                    foreach (KeyValuePair<string, int> kvp in flavorProfile)
                    {
                        if (kvp.Value == valueOfMaxFlavor) { flavor = kvp.Key; }
                    }
                }
                efb.Value = $"*Quantity: {b.qty}. Prevailing Flavor: {flavor}. Color: {b.color}*";
                pages.Add(efb);
            }
            PaginatedMessage msg = new PaginatedMessage();
            msg.Pages = pages;
            msg.Title = $"{player.name}'s Berry Pouch";
            msg.Color = new Color(247, 89, 213);
            msg.Options.Timeout = null;
            await PagedReplyAsync(msg);
        }

        [Group("Skills")]
        public class UserSkillsModule : ModuleBase<SocketCommandContext>
        {
            readonly BaseModule bm = new BaseModule();
            [Command("Info", RunMode = RunMode.Async)]
            [Summary("Shows you information about the different player skills! Usage: >breeder skills info")]
            [RequireBegan()]
            public async Task SkillsInfoCommand()
            {
                var embed = new EmbedBuilder();
                embed.AddField($"**Incubation**", $"Breeders skilled in Incubation make Eggs hatch faster!");
                embed.AddField($"**Appraisal**", $"Breeders skilled in Appraisal find rare Pokemon more often!");
                embed.AddField($"**Perception**", $"Breeders skilled in Perception find Eggs more often!");
                embed.AddField($"**Coaching**", $"Breeders skilled in Coaching help their Pokemon grow quicker!");
                embed.AddField($"**Care**", $"Breeders skilled in Care get closer to their Pokemon faster!");
                embed.AddField($"**Husbandry**", $"Breeders skilled in Husbandry get their Pokemon to breed faster!");
                embed.AddField($"**Cooking**", $"Breeders skilled in Cooking have less trouble cooking treats for their Pokemon!");
                embed.Title = "Skills Info";
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }

            [Command("Spend", RunMode = RunMode.Async)]
            [Summary("Lets you spend skill points on different stats! Usage: >breeder skills spend [amount to spend] [skill to spend points on]")]
            [RequireBegan()]
            public async Task SkillSpendCommand(string amount, string skill)
            {
                var sb = new StringBuilder();
                var embed = new EmbedBuilder();
                var user = Context.User;
                bool validAmount = Int32.TryParse(amount, out int amt);
                if (validAmount)
                {
                    Player player = bm.PlayerLoad(user.Id);
                    if (player.skillPoints >= amt)
                    {
                        bool validOption = true;
                        int skillValue = 0;
                        bool hitLimit = false;
                        skill = skill.Substring(0, 1).ToUpper() + skill.Substring(1);
                        switch (skill)
                        {
                            case "Incubation":
                                if ((player.eggHatchSpeedMult + amt) <= 20)
                                {
                                    player.eggHatchSpeedMult += amt;
                                    skillValue = player.eggHatchSpeedMult;
                                }
                                else { hitLimit = true; }
                                break;
                            case "Appraisal":
                                if ((player.legendaryMult + amt) <= 20)
                                {
                                    player.legendaryMult += amt;
                                    skillValue = player.legendaryMult;
                                }
                                else { hitLimit = true; }
                                break;
                            case "Perception":
                                if ((player.eggMult + amt) <= 20)
                                {
                                    player.eggMult += amt;
                                    skillValue = player.eggMult;
                                }
                                else { hitLimit = true; }
                                break;
                            case "Coaching":
                                if ((player.trainingMult + amt) <= 20)
                                {
                                    player.trainingMult += amt;
                                    skillValue = player.trainingMult;
                                }
                                else { hitLimit = true; }
                                break;
                            case "Care":
                                if ((player.affectionMult + amt) <= 20)
                                {
                                    player.affectionMult += amt;
                                    skillValue = player.affectionMult;
                                }
                                else { hitLimit = true; }
                                break;
                            case "Husbandry":
                                if ((player.breedingTimerMult + amt) <= 20)
                                {
                                    player.breedingTimerMult += amt;
                                    skillValue = player.breedingTimerMult;
                                }
                                else { hitLimit = true; }
                                break;
                            case "Cooking":
                                if ((player.cookingSuccessMult + amt) <= 20)
                                {
                                    player.cookingSuccessMult += amt;
                                    skillValue = player.cookingSuccessMult;
                                }
                                else { hitLimit = true; }
                                break;
                            default: validOption = false; break;
                        }
                        if (validOption && !hitLimit)
                        {
                            sb.AppendLine($"{amt} added to {player.name}'s {skill} skill! New value: {skillValue}.");
                            embed.Title = $"Skill Improved";
                            player.skillPoints -= amt;
                            bm.PlayerSave(player);
                        }
                        else if (validOption && hitLimit)
                        {
                            sb.AppendLine($"Adding {amt} to {player.name}'s {skill} skill will put it over 20! The max amount for any skill is 20. Please try again!");
                            embed.Title = $"Limit hit!";
                        }
                        else
                        {
                            sb.AppendLine($"{skill} is not a valid skill! Please choose from the list found in >breeder skill info.");
                            embed.Title = "Invalid Choice";
                        }
                    }
                    else
                    {
                        sb.AppendLine($"You don't have {amt} points to spend! You currently have {player.skillPoints} point(s). Earn more by hatching Eggs, training Pokemon, and leveling up!");
                        embed.Title = "Insufficient Skill Points";
                    }
                }
                else
                {
                    sb.AppendLine($"Please enter the amount of skill points you'd like to spend in digits (0-9)!");
                    embed.Title = "Invalid Entry";
                }
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }
    }
}
