using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using PokeParadise.Classes;

namespace PokeParadise.Services
{
    public class CommandHandler
    {
        BaseModule bm = new BaseModule();
        // setup fields to be set later in the constructor
        private readonly IConfiguration _config;
        private readonly CommandService _commands;
        public static CommandService cmds;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        public Classes.Environment serverEnvironment;

        public CommandHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            _config = services.GetRequiredService<IConfiguration>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            // take action when we execute a command
            _commands.CommandExecuted += CommandExecutedAsync;

            // take action when we receive a message (so we can process it, and see if it is a valid command)
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            cmds = _commands;
        }

        // this class is where the magic starts, and takes actions upon receiving messages
        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // ensures we don't process system/other bot messages
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User)
            {
                return;
            }

            // sets the argument position away from the prefix we set
            var argPos = 0;

            // get prefix from the configuration file
            string prefix = _config["Prefix"];

            TimeSpan cd = new TimeSpan(2, 0, 0);
            TimeSpan checkCd = new TimeSpan(0, 10, 0);
            if (File.Exists(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\environment.txt"))
            {
                string[] lines = File.ReadAllLines(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\environment.txt");
                int cnt = 1;
                foreach (string line in lines)
                {
                    serverEnvironment = new Classes.Environment();
                    switch (cnt)
                    {
                        case 1: serverEnvironment.weather = line; break;
                        case 2: serverEnvironment.timeOfDay = line; break;
                        case 3: serverEnvironment.lastChecked = DateTimeOffset.Parse(line); break;
                        case 4: serverEnvironment.lastSet = DateTimeOffset.Parse(line); break;
                    }
                    cnt++;
                }
            }
            if (serverEnvironment == null) { serverEnvironment = new Classes.Environment(); }
            if (serverEnvironment.lastChecked == null) { serverEnvironment.lastChecked = DateTimeOffset.MinValue; }
            if (serverEnvironment.lastSet == null) { serverEnvironment.lastSet = DateTimeOffset.MinValue; }
            if (DateTimeOffset.Now - serverEnvironment.lastChecked >= checkCd)
            {
                if (DateTimeOffset.Now - serverEnvironment.lastSet >= cd)
                {
                    Random rand = new Random();
                    int timeFlip = rand.Next(1, 100);
                    if (timeFlip <= 50) { serverEnvironment.timeOfDay = "day"; } else { serverEnvironment.timeOfDay = "night"; }
                    int weatherFlip = rand.Next(1, 16);
                    switch (weatherFlip)
                    {
                        case 1: serverEnvironment.weather = "clear"; break;
                        case 2: serverEnvironment.weather = "clear"; break;
                        case 3: serverEnvironment.weather = "clear"; break;
                        case 4: serverEnvironment.weather = "clear"; break;
                        case 5: serverEnvironment.weather = "clear"; break;
                        case 6: serverEnvironment.weather = "clear"; break;
                        case 7: serverEnvironment.weather = "clear"; break;
                        case 8: serverEnvironment.weather = "clear"; break;
                        case 9: serverEnvironment.weather = "cloudy"; break;
                        case 10: serverEnvironment.weather = "rainy"; break;
                        case 11: serverEnvironment.weather = "thunderstorm"; break;
                        case 12: serverEnvironment.weather = "snow"; break;
                        case 13: serverEnvironment.weather = "blizzard"; break;
                        case 14: serverEnvironment.weather = "harsh sunlight"; break;
                        case 15: serverEnvironment.weather = "sandstorm"; break;
                        case 16: serverEnvironment.weather = "foggy"; break;
                    }
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.Description = $"The weather has shifted! The weather is now {serverEnvironment.weather}, and the time of day is {serverEnvironment.timeOfDay}!";
                    SocketGuildChannel x = rawMessage.Channel as SocketGuildChannel;
                    foreach (SocketGuildChannel channel in x.Guild.Channels)
                    {
                        if (channel.Id == 747450187760402523) 
                        {
                            ISocketMessageChannel chnl = channel as ISocketMessageChannel;
                            await chnl.SendMessageAsync(null, false, eb.Build()); 
                        }
                    }
                    serverEnvironment.lastSet = DateTimeOffset.Now;
                }
                serverEnvironment.lastChecked = DateTimeOffset.Now;
                using (StreamWriter file =
                    new StreamWriter(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\environment.txt"))
                {
                    file.WriteLine($"{serverEnvironment.weather}");
                    file.WriteLine($"{serverEnvironment.timeOfDay}");
                    file.WriteLine($"{serverEnvironment.lastChecked}");
                    file.WriteLine($"{serverEnvironment.lastSet}");
                }
            }

            StringBuilder stringb = new StringBuilder();
            EmbedBuilder emb = new EmbedBuilder();
            Dictionary<Pokemon, Player> toReturn = new Dictionary<Pokemon, Player>();
            Dictionary<ulong, Player> playerDict = bm.FetchPlayerDict();
            foreach (KeyValuePair<ulong, Player> kvp in playerDict)
            {
                foreach (Pokemon p in kvp.Value.pokemon)
                {
                    if (p.isLoaned)
                    {
                        if (DateTimeOffset.Now - p.whenLoaned >= p.timeToLoan)
                        {
                            toReturn.Add(p, kvp.Value);
                        }
                    }
                }
            }
            foreach (Pokemon p in toReturn.Keys)
            {
                Player loaned = toReturn[p];
                loaned.pokemon.Remove(p);
                p.isLoaned = false;
                p.timeToLoan = new TimeSpan(0, 0, 0);
                p.whenLoaned = DateTimeOffset.MaxValue;
                p.id = p.originalId;
                Player trainer = p.trainer;
                trainer.pokemon.Add(p);
                bm.PlayerSave(loaned);
                bm.PlayerSave(p.trainer);
                stringb.AppendLine($"{p.nickname} has left {loaned.name} and returned to their original trainer, {trainer.name}!");
            }
            if (toReturn.Count >= 1)
            {
                emb.Description = stringb.ToString();
                emb.Title = $"Loaned Pokemon returned!";
                SocketGuildChannel y = rawMessage.Channel as SocketGuildChannel;
                foreach (SocketGuildChannel channel in y.Guild.Channels)
                {
                    if (channel.Id == 747450187760402523)
                    {
                        ISocketMessageChannel chnl = channel as ISocketMessageChannel;
                        await chnl.SendMessageAsync(null, false, emb.Build());
                    }
                }
            }

            // determine if the message has a valid prefix, and adjust argPos based on prefix
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasStringPrefix(prefix, ref argPos)))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);
            if (!Directory.Exists("D:\\PokeParadise Files\\Backups"))
            {
                Directory.CreateDirectory("D:\\PokeParadise Files\\Backups");
            }
            ServerBackup sb = FetchServerBackup();
            TimeSpan cooldown = new TimeSpan(0, 30, 0);
            if (DateTimeOffset.Now - sb.lastBackup >= cooldown)
            {
                string fileName = $"D:\\PokeParadise Files\\Backups\\Backup{sb.amountOfBackups + 1}.xml";
                Dictionary<ulong, Player> playerData = bm.FetchPlayerDict();
                Serialize(playerData, fileName);
                sb.amountOfBackups++;
                sb.lastBackup = DateTimeOffset.Now;
                if (sb.amountOfBackups > 4)
                {
                    sb.amountOfBackups = 0;
                }
                Serialize(sb, "D:\\PokeParadise Files\\Backups\\BackupStats.xml");
            }
            // execute command if one is found that matches
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                System.Console.WriteLine($"Command failed to execute for [" + context.Message.Author + "] <-> [" + context.Message.Content + "]!");
                return;
            }


            // log success to the console and exit this method
            if (result.IsSuccess)
            {
                System.Console.WriteLine($"Command [" + context.Message.Content + "] executed for -> [" + context.User.Username + "]");
                return;
            }

            // failure scenario, let's let the user know
            await context.Channel.SendMessageAsync($"{result.ErrorReason}");
        }

        public ServerBackup FetchServerBackup()
        {
            ServerBackup sb = new ServerBackup();
            BinaryFormatter formatter = new BinaryFormatter();
            object lck = new object();
            lock (lck)
            {
                Stream s = File.Open("D:\\PokeParadise Files\\Backups\\BackupStats.xml", FileMode.OpenOrCreate);
                if (s.Length != 0)
                {
                    sb = (ServerBackup)formatter.Deserialize(s);
                }
                s.Close();

                return sb;
            }
        }

        public void Serialize(object o, string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream s = File.Open(fileName, FileMode.OpenOrCreate);
            formatter.Serialize(s, o);
            s.Close();
        }

        public async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message != null && reaction.User.IsSpecified)
            {

            }
        }
    }
}