using System;
using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using PokeParadise.Services;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Discord.Addons.Interactive;
using PokeParadise.Modules;

namespace PokeParadise
{
    class PokeParadise
    {
        private readonly IConfiguration _config;
        private DiscordSocketClient _client;

        static void Main(string[] args)
        {
            new PokeParadise().MainAsync().GetAwaiter().GetResult();
        }

        public PokeParadise()
        {
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");
      
            _config = _builder.Build();
        }

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                _client = client;

                client.Log += LogAsync;
                client.Ready += ReadyAsync;
                client.ReactionAdded += HandleReactionAddedAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                await client.LoginAsync(TokenType.Bot, _config["Token"]);
                await client.StartAsync();

                await services.GetRequiredService<CommandHandler>().InitializeAsync();
                await Task.Delay(-1);
            }
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"Connected as -> [PokeParadise]! :)");
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            ServiceCollection x = new ServiceCollection();
            return new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<InteractiveService>()
                .BuildServiceProvider();
        }

        public async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            ContestModule cm = new ContestModule();
            if (cm.reactionMessageIds.Count > 0)
            {
                foreach (ulong messageId in cm.reactionMessageIds)
                {
                    if (messageId == cachedMessage.Id)
                    {
                        foreach (ulong playerId in cm.contestPlayerIds.Keys)
                        {
                            if (playerId == reaction.UserId)
                            {
                                cm.PlayerMoveChoice(reaction, cachedMessage.Value);
                            }
                        }
                    }
                }
            }
        }
    }
}