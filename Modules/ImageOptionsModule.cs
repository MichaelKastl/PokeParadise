using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    [Group("Image")]
    [Alias("img", "i")]
    public class ImageOptionsModule : InteractiveBase<SocketCommandContext>
    {
        readonly BaseModule bm = new BaseModule();
        [Command("Preference", RunMode = RunMode.Async)]
        [Summary("Lets you pick which Pokedex source you want to use! To see the available options, use >image options. Usage: >image preference [preference code]")]
        [RequireBegan()]
        public async Task ImagePreferenceCommand(string choice)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            bool validChoice = Int32.TryParse(choice, out int intChoice);
            if (validChoice)
            {
                string source = null;
                switch (intChoice)
                {
                    case 1: source = $"Official artwork!"; break;
                    case 2: source = $"Pokemon Home!"; break;
                    case 3: source = $"Pokemon Anime!"; break;
                    case 4: source = $"Pokemon Dream World/Global Link!"; break;
                    case 5: source = $"Pokemon Mystery Dungeon!"; break;
                    case 6: source = $"Pokemon Sun/Moon!"; break;
                    case 7: source = $"Pokemon X/Y/OR/AS Gifs! Note: Take longer to load!"; break;
                    case 8: source = $"Pokemon D/P/P Gifs! Note: Take longer to load!"; break;
                    case 9: source = $"Pokemon 8-Bit Sprites! Note: These will be the sprites found in your party menu in main-line games."; break;
                    case 10: source = $"Pokemon HG/SS Overworld Sprites! Note: These are the sprites that Pokemon had when walking around in the overworld."; break;
                    case 11: source = $"Random! This option will choose a random theme for each time you enter >pkmn info."; break;
                    default: validChoice = false; break;
                }
                if (validChoice)
                {
                    sb.AppendLine($"Source chosen: {source}");
                    embed.Title = player.name + "'s Image Source Preference";
                    player.imgPref = intChoice - 1;
                    bm.PlayerSave(player);
                }
                else
                {
                    sb.AppendLine($"Please enter a digit 1-11!");
                    embed.Title = "Invalid Input";
                }
            }
            else
            {
                sb.AppendLine($"Please enter a digit 1-11!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Options", RunMode = RunMode.Async)]
        [Summary("Shows you all of your options regarding image sources! Usage: >image options")]
        [RequireBegan()]
        public async Task ImagePreferenceOptionsCommand()
        {
            var embed = new EmbedBuilder();
            embed.AddField($"**Option 1**", $"Official artwork!");
            embed.AddField($"**Option 2**", $"Pokemon Home!");
            embed.AddField($"**Option 3**", $"Pokemon Anime!");
            embed.AddField($"**Option 4**", $"Pokemon Dream World/Global Link!");
            embed.AddField($"**Option 5**", $"Pokemon Mystery Dungeon!");
            embed.AddField($"**Option 6**", $"Pokemon Sun/Moon!");
            embed.AddField($"**Option 7**", $"Pokemon X/Y/OR/AS Gifs! Note: Take longer to load!");
            embed.AddField($"**Option 8**", $"Pokemon D/P/P Gifs! Note: Take longer to load!");
            embed.AddField($"**Option 9**", $"Pokemon 8-Bit Sprites! Note: These will be the sprites found in your party menu in main-line games.");
            embed.AddField($"**Option 10**", $"Pokemon HG/SS Overworld Sprites! Note: These are the sprites that Pokemon had when walking around in the overworld.");
            embed.AddField($"**Option 11**", $"Random! This option will choose a random theme for each time you enter >pkmn info.");
            embed.AddField($"**Other Notes**", "Not all Pokemon are available in each style. Only options 1-3 have representations for all Pokemon up to Eternatus. If your chosen source does not " +
                " have a picture to represent the Pokemon you're viewing, the source you've chosen as your secondary preference (>img backup) will be used instead.");
            embed.Title = "Image Preference Options";
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("BackupOptions", RunMode = RunMode.Async)]
        [Alias("buopt", "bo")]
        [Summary("Shows you your options for a backup image source! This source will be used if your primary choice doesn't have a picture available for the Pokemon you're using. Usage: >image backupoptions")]
        [RequireBegan()]
        public async Task ImagePreferenceBackupOptionsCommand()
        {
            var embed = new EmbedBuilder();
            embed.AddField($"**Option 1**", $"Official artwork!");
            embed.AddField($"**Option 2**", $"Pokemon Home!");
            embed.AddField($"**Option 3**", $"Pokemon Anime!");
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Backup", RunMode = RunMode.Async)]
        [Alias("bu", "b")]
        [Summary("Allows you to select a backup image source! Usage: >image backup [preference code]")]
        [RequireBegan()]
        public async Task ImagePreferenceBackupCommand(string choice)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            bool validChoice = Int32.TryParse(choice, out int intChoice);
            if (validChoice)
            {
                string source = null;
                switch (intChoice)
                {
                    case 1: source = $"Official artwork!"; break;
                    case 2: source = $"Pokemon Home!"; break;
                    case 3: source = $"Pokemon Anime!"; break;
                    default: validChoice = false; break;
                }
                if (validChoice)
                {
                    sb.AppendLine($"Source chosen: {source}");
                    embed.Title = player.name + "'s Image Source Preference";
                    player.backupPref = intChoice - 1;
                    bm.PlayerSave(player);
                }
                else
                {
                    sb.AppendLine($"Please enter a digit 1-3!");
                    embed.Title = "Invalid Input";
                }
            }
            else
            {
                sb.AppendLine($"Please enter a digit 1-3!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }
    }
}
