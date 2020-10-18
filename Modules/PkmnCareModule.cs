using Discord;
using Discord.Commands;
using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static PokeParadise.Classes.EvolutionHandler;

namespace PokeParadise.Modules
{
    [Group("Care")]
    [Alias("c")]
    public class PkmnCareModule : ModuleBase<SocketCommandContext>
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

        [Command("Pet", RunMode = RunMode.Async)]
        [Summary("Allows you to pet your Pokemon! This will raise their experience, and maybe even level them up! Usage: >care pet [Pokemon ID]")]
        [RequireBegan()]
        public async Task PetCommand(string inputId)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            bool isId = true;
            Player player = bm.PlayerLoad(user.Id);
            bool validInput = Int32.TryParse(inputId, out int id);
            if (!validInput)
            {
                inputId = inputId.ToLower();
                foreach (Pokemon p in player.pokemon)
                {
                    if (p.nickname.ToLower() == inputId)
                    {
                        validInput = true;
                        isId = false;
                    }
                }
            }
            if (validInput)
            {
                bool validMatch = false;
                Pokemon p = new Pokemon();
                List<Pokemon> toRemove = new List<Pokemon>();
                foreach (Pokemon x in player.pokemon)
                {
                    if (isId)
                    {
                        if (x.id == id)
                        {
                            p = x;
                            toRemove.Add(p);
                            validMatch = true;
                        }
                    }
                    else
                    {
                        if (x.nickname.ToLower() == inputId)
                        {
                            p = x;
                            toRemove.Add(p);
                            validMatch = true;
                        }
                    }
                }
                if (toRemove.Count == 1)
                {
                    if (validMatch)
                    {
                        PokemonXpCalc pxc = new PokemonXpCalc(p);
                        int xpGain = pxc.baseXpGained;
                        p.xp += xpGain;
                        int xpAtNextLevel = pxc.xpAtNextLevel;
                        bool hasLeveled = false;
                        bool hasEvolved = false;
                        EvolvePkg pkg = new EvolvePkg();
                        if (p.xp >= xpAtNextLevel)
                        {
                            p.level++;
                            hasLeveled = true;
                            EvolutionHandler eh = new EvolutionHandler();
                            pkg = eh.EvolveCheck(p, player);
                            hasEvolved = pkg.hasEvolved;
                        }
                        p.friendship++;
                        string nickname = p.nickname;
                        if (p.nickname == null)
                        {
                            nickname = p.pkmnName;
                        }
                        sb.AppendLine($"{nickname} really seems to enjoy it! They let out a happy noise and nuzzle into {player.name}'s hand.");
                        if (hasLeveled)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"{nickname} has leveled up!");
                        }
                        if (hasEvolved)
                        {
                            p.dexNo = pkg.evolvedTo.dexNo;
                            p.pkmnName = pkg.evolvedTo.pkmnName;
                            p.type1 = pkg.evolvedTo.type1;
                            p.type2 = pkg.evolvedTo.type2;
                            p.speciesAbility1 = pkg.evolvedTo.speciesAbility1;
                            p.speciesAbility2 = pkg.evolvedTo.speciesAbility2;
                            p.baseStatTotal = pkg.evolvedTo.baseStatTotal;
                            p.baseHP = pkg.evolvedTo.baseHP;
                            p.baseAtk = pkg.evolvedTo.baseAtk;
                            p.baseDef = pkg.evolvedTo.baseDef;
                            p.baseSpAtk = pkg.evolvedTo.baseSpAtk;
                            p.baseSpDef = pkg.evolvedTo.baseSpDef;
                            p.baseSpd = pkg.evolvedTo.baseSpd;
                            p.evoLevel = pkg.evolvedTo.evoLevel;
                            p.legendStatus = pkg.evolvedTo.legendStatus;
                            p.mythicStatus = pkg.evolvedTo.mythicStatus;
                            p.nextEvo = pkg.evolvedTo.nextEvo;
                            p.prevEvo = pkg.evolvedTo.prevEvo;
                            p.eggGroup1 = pkg.evolvedTo.eggGroup1;
                            p.eggGroup2 = pkg.evolvedTo.eggGroup2;
                            p.levelSpeed = pkg.evolvedTo.levelSpeed;
                            p.genderThreshhold = pkg.evolvedTo.genderThreshhold;
                            p.blurb = pkg.evolvedTo.blurb;
                            sb.AppendLine($"{nickname} has evolved! They're now a {p.pkmnName}!");
                        }
                        foreach (Pokemon x in toRemove)
                        {
                            player.pokemon.Remove(x);
                        }
                        player.pokemon.Add(p);
                        bm.PlayerSave(player);
                        embed.Title = $"{player.name} pets their {p.pkmnName}.";
                    }
                    else
                    {
                        sb.AppendLine($"You don't have a Pokemon with that ID number!");
                        embed.Title = "No Pokemon with ID # " + id + " Found";
                    }
                }
                else
                {
                    sb.AppendLine($"Please either specify them by ID or re-nickname one to something else!");
                    embed.Title = $"You have multiple Pokemon with that nickname.";
                }
            }
            else
            {
                sb.AppendLine($"Please only enter digits (0-9) for your Pokemon's ID!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Walk", RunMode = RunMode.Async)]
        [Summary("You go for a walk with your Pokemon! You could find some money, or maybe even a new Egg! You'll also earn experience for your Pokemon! Usage: >care walk [Pokemon ID]")]
        [RequireBegan()]
        public async Task WalkCommand(string inputId)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            bool isId = true;
            Player player = bm.PlayerLoad(user.Id);
            bool validInput = Int32.TryParse(inputId, out int id);
            if (!validInput)
            {
                inputId = inputId.ToLower();
                foreach (Pokemon p in player.pokemon)
                {
                    if (p.nickname.ToLower() == inputId)
                    {
                        validInput = true;
                        isId = false;
                    }
                }
            }
            if (validInput)
            {
                bool validMatch = false;
                Pokemon p = new Pokemon();
                List<Pokemon> toRemove = new List<Pokemon>();
                foreach (Pokemon x in player.pokemon)
                {
                    if (isId)
                    {
                        if (x.id == id)
                        {
                            p = x;
                            toRemove.Add(p);
                            validMatch = true;
                        }
                    }
                    else
                    {
                        if (x.nickname.ToLower() == inputId)
                        {
                            p = x;
                            toRemove.Add(p);
                            validMatch = true;
                        }
                    }
                }
                if (toRemove.Count == 1)
                {
                    if (validMatch)
                    {
                        PokemonXpCalc pxc = new PokemonXpCalc(p);
                        int xpGain = pxc.baseXpGained;
                        p.xp += xpGain;
                        p.friendship++;
                        int xpAtNextLevel = pxc.xpAtNextLevel;
                        bool hasLeveled = false;
                        bool hasEvolved = false;
                        EvolvePkg pkg = new EvolvePkg();
                        if (p.xp >= xpAtNextLevel)
                        {
                            p.level++;
                            EvolutionHandler eh = new EvolutionHandler();
                            pkg = eh.EvolveCheck(p, player);
                            hasLeveled = true;
                            hasEvolved = pkg.hasEvolved;
                        }
                        string nickname = p.nickname;
                        if (p.nickname == null)
                        {
                            nickname = p.pkmnName;
                        }
                        sb.AppendLine($"{nickname} loves the fresh air. They stop to play in a pile of leaves, rolling around in them for a good ten minutes.");
                        sb.AppendLine($"When they get up from the leaf pile, they run back to you holding something.");
                        Random rand = new Random();
                        int lootRoll = rand.Next(1, 20);
                        if (lootRoll <= 2)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"They're holding a **sturdy-looking stick**, and they look very proud of their treasure.");
                            sb.AppendLine($"You pat {nickname} gently on the head and start the walk home as they swing the stick around happily.");
                            sb.AppendLine($"");
                        }
                        else if (lootRoll <= 4)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"They're holding a little **pebble**! They coo at it in happiness, and cradle it like an egg.");
                            sb.AppendLine($"You take it home with you and put it on a shelf, next to a dozen other rocks {nickname} has brought home.");
                            sb.AppendLine($"");
                        }
                        else if (lootRoll <= 6)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"They're holding a **handful of leaves**! As you get close, they throw the leaves onto you.");
                            sb.AppendLine($"You laugh and grab some leaves too, throwing them on {nickname}. You two have a leaf-fight for nearly 20 minutes before you start the walk home.");
                            sb.AppendLine($"");
                        }
                        else if (lootRoll <= 8)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"They're holding a little **Caterpie**! They giggle as it crawls up their arm.");
                            sb.AppendLine($"You gently pluck the Caterpie off of {nickname}, and return it to the leaf pile. You both wave goodbye to the wild Caterpie!");
                            sb.AppendLine($"");
                        }
                        else if (lootRoll <= 10)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"They're holding a **coin**! You whistle, a bit surprised that they found it.");
                            sb.AppendLine($"{nickname} looks at that coin all the way home, and puts it on display when they get back.");
                            sb.AppendLine($"");
                        }
                        else if (lootRoll <= 11)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"They're holding a **broken wristwatch**. They're trying to wrap it around their wrist, but it's too small.");
                            sb.AppendLine($"You take it home with you and sew a few extra inches onto the band. {nickname} loves their new accessory!");
                            sb.AppendLine($"");
                        }
                        else if (lootRoll <= 12)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"They're holding a **Pokedex**! You're very surprised to see it; someone must have lost it!");
                            sb.AppendLine($"You turn it in to Officer Jenny, who gives you a 155 coin reward for it! Being a good guy pays off sometimes.");
                            sb.AppendLine($"To reward {nickname}, you spend 5 of those coins on a nice, big Poffin for their good work.");
                            sb.AppendLine($"");
                            player.coins += 150;
                            bm.PlayerSave(player);
                        }
                        else if (lootRoll <= 15)
                        {
                            int berryOrEvoStone = rand.Next(1, 10);
                            if (berryOrEvoStone >= 9)
                            {
                                sb.AppendLine($"");
                                sb.AppendLine($"They're holding a **strange shimmering object**! You take it, thinking you might know what it is...");
                                string stone = null;
                                int stoneChoice = rand.Next(1, 35);
                                switch (stoneChoice)
                                {
                                    case 1: stone = "Dawn Stone"; break;
                                    case 2: stone = "Dusk Stone"; break;
                                    case 3: stone = "Fire Stone"; break;
                                    case 4: stone = "Ice Stone"; break;
                                    case 5: stone = "Leaf Stone"; break;
                                    case 6: stone = "Moon Stone"; break;
                                    case 7: stone = "Shiny Stone"; break;
                                    case 8: stone = "Sun Stone"; break;
                                    case 9: stone = "Thunder Stone"; break;
                                    case 10: stone = "Water Stone"; break;
                                    case 11: stone = "Prism Scale"; break;
                                    case 12: stone = "Chipped Pot"; break;
                                    case 13: stone = "Cracked Pot"; break;
                                    case 14: stone = "Deep Sea Scale"; break;
                                    case 15: stone = "Deep Sea Tooth"; break;
                                    case 16: stone = "Dragon Scale"; break;
                                    case 17: stone = "Dubious Disc"; break;
                                    case 18: stone = "Electirizer"; break;
                                    case 19: stone = "Galarica Cuff"; break;
                                    case 20: stone = "King's Rock"; break;
                                    case 21: stone = "Magmarizer"; break;
                                    case 22: stone = "Metal Coat"; break;
                                    case 23: stone = "Oval Stone"; break;
                                    case 24: stone = "Prism Scale"; break;
                                    case 25: stone = "Protector"; break;
                                    case 26: stone = "Razor Claw"; break;
                                    case 27: stone = "Razor Fang"; break;
                                    case 28: stone = "Reaper Cloth"; break;
                                    case 29: stone = "Sachet"; break;
                                    case 30: stone = "Sweet Apple"; break;
                                    case 31: stone = "Tart Apple"; break;
                                    case 32: stone = "Upgrade"; break;
                                    case 33: stone = "Whipped Dream"; break;
                                    case 34: stone = "Upside-Down Button"; break;
                                    case 35: stone = "Rare Candy"; break;
                                }
                                Item i = new Item(stone, 1);
                                bool isMatch = false;
                                if (player.inventory == null)
                                { player.inventory = new List<Item>(); }
                                foreach (Item x in player.inventory)
                                {
                                    if (x.name == i.name)
                                    {
                                        x.qty += 1;
                                        isMatch = true;
                                    }
                                }
                                if (!isMatch)
                                {
                                    player.inventory.Add(i);
                                }
                                bm.PlayerSave(player);
                                sb.AppendLine($"Yes! It's a {stone}! These are hard to find! You pocket it, and pat your {nickname} affectionately.");
                                sb.AppendLine($"");
                            }
                            else
                            {
                                sb.AppendLine($"");
                                sb.AppendLine($"They're holding a **small handfull of berries**! You pick through the pile to see what {nickname} has found for you.");
                                int berryVarieties = rand.Next(1, 3);
                                int cnt = 1;
                                List<Berry> berriesGathered = new List<Berry>();
                                while (cnt <= berryVarieties)
                                {
                                    int berryChoice = rand.Next(1, 64);
                                    int amtOfBerry = rand.Next(1, 4);
                                    Berry chosen = new Berry(berryChoice + 1)
                                    {
                                        qty = amtOfBerry
                                    };
                                    berriesGathered.Add(chosen);
                                    cnt++;
                                }
                                bool hasBerry = false;
                                foreach (Berry b in berriesGathered)
                                {
                                    if (player.berryPouch != null)
                                    {
                                        foreach (Berry x in player.berryPouch)
                                        {
                                            if (b.number == x.number) { x.qty += b.qty; hasBerry = true; }
                                        }
                                        if (!hasBerry)
                                        {
                                            player.berryPouch.Add(b);
                                        }
                                        else { hasBerry = false; }
                                    }
                                    else
                                    {
                                        player.berryPouch = new List<Berry>
                                        {
                                            b
                                        };
                                        hasBerry = true;
                                    }
                                }
                                if (berryVarieties == 1) { sb.AppendLine($"{nickname} gathered {berriesGathered[0].qty} {berriesGathered[0].name} berries for you!"); }
                                else if (berryVarieties == 2) { sb.AppendLine($"{nickname} gathered {berriesGathered[0].qty} {berriesGathered[0].name} berries and {berriesGathered[1].qty} {berriesGathered[1].name} berries for you!"); }
                                else if (berryVarieties == 3) { sb.AppendLine($"{nickname} gathered {berriesGathered[0].qty} {berriesGathered[0].name} berries,  {berriesGathered[1].qty} {berriesGathered[1].name} berries, and {berriesGathered[2].qty} {berriesGathered[2].name} berries for you!"); }
                                sb.AppendLine($"");
                                sb.AppendLine($"They're licking their lips thinking of the tasty treats you can cook with those berries!");
                            }
                        }
                        else if (lootRoll <= 17)
                        {
                            int coin = rand.Next(1, 20);
                            int totalCoin = coin * 10;
                            sb.AppendLine($"");
                            sb.AppendLine($"They're holding a small sack of **coins**! There's about {totalCoin} coins there. You pat your {nickname} on the head and take the coins.");
                            sb.AppendLine($"{nickname} looks very proud of themselves.");
                            sb.AppendLine($"");
                            player.coins += totalCoin;
                        }
                        else if (lootRoll >= 18)
                        {
                            List<int> ids = new List<int>();
                            foreach (Egg x in player.eggs)
                            {
                                ids.Add(x.eggId);
                            }
                            int eggId;
                            if (ids.Count > 0)
                            {
                                eggId = ids.Max() + 1;
                            }
                            else
                            {
                                eggId = 1;
                            }
                            Egg e = new Egg(DateTimeOffset.Now, eggId);
                            player.eggs.Add(e);
                            sb.AppendLine($"");
                            sb.AppendLine($"They're holding an **Egg**! You don't know what species it belongs to, but you tuck it carefully into your bag to take care of it.");
                            sb.AppendLine($"Your {nickname} is looking at the **Egg** in a protective way. They look very happy!");
                            sb.AppendLine($"");
                        }
                        if (hasLeveled)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"{nickname} has leveled up!");
                        }
                        if (hasEvolved)
                        {
                            p.dexNo = pkg.evolvedTo.dexNo;
                            p.pkmnName = pkg.evolvedTo.pkmnName;
                            p.type1 = pkg.evolvedTo.type1;
                            p.type2 = pkg.evolvedTo.type2;
                            p.speciesAbility1 = pkg.evolvedTo.speciesAbility1;
                            p.speciesAbility2 = pkg.evolvedTo.speciesAbility2;
                            p.baseStatTotal = pkg.evolvedTo.baseStatTotal;
                            p.baseHP = pkg.evolvedTo.baseHP;
                            p.baseAtk = pkg.evolvedTo.baseAtk;
                            p.baseDef = pkg.evolvedTo.baseDef;
                            p.baseSpAtk = pkg.evolvedTo.baseSpAtk;
                            p.baseSpDef = pkg.evolvedTo.baseSpDef;
                            p.baseSpd = pkg.evolvedTo.baseSpd;
                            p.evoLevel = pkg.evolvedTo.evoLevel;
                            p.legendStatus = pkg.evolvedTo.legendStatus;
                            p.mythicStatus = pkg.evolvedTo.mythicStatus;
                            p.nextEvo = pkg.evolvedTo.nextEvo;
                            p.prevEvo = pkg.evolvedTo.prevEvo;
                            p.eggGroup1 = pkg.evolvedTo.eggGroup1;
                            p.eggGroup2 = pkg.evolvedTo.eggGroup2;
                            p.levelSpeed = pkg.evolvedTo.levelSpeed;
                            p.genderThreshhold = pkg.evolvedTo.genderThreshhold;
                            p.blurb = pkg.evolvedTo.blurb;
                            sb.AppendLine($"{nickname} has evolved! They're now a {p.pkmnName}!");
                        }
                        foreach (Pokemon x in toRemove)
                        {
                            player.pokemon.Remove(x);
                        }
                        player.pokemon.Add(p);
                        bm.PlayerSave(player);
                        embed.Title = $"{player.name} takes their {p.pkmnName} for a walk.";
                    }
                    else
                    {
                        sb.AppendLine($"You don't have a Pokemon with that ID number!");
                        embed.Title = "No Pokemon with ID # " + id + " Found";
                    }
                }
                else
                {
                    sb.AppendLine($"Please either specify them by ID or re-nickname one to something else!");
                    embed.Title = $"You have multiple Pokemon with that nickname.";
                }
            }
            else
            {
                sb.AppendLine($"Please only enter digits (0-9) for your Pokemon's ID!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Train", RunMode = RunMode.Async)]
        [Summary("You train your Pokemon to level them up! Usage: >care train [Pokemon ID]")]
        [RequireBegan()]
        public async Task TrainCommand(string inputId)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            bool isId = true;
            Player player = bm.PlayerLoad(user.Id);
            bool validInput = Int32.TryParse(inputId, out int id);
            if (!validInput)
            {
                inputId = inputId.ToLower();
                foreach (Pokemon p in player.pokemon)
                {
                    if (p.nickname.ToLower() == inputId)
                    {
                        validInput = true;
                        isId = false;
                    }
                }
            }
            if (validInput)
            {
                bool validMatch = false;
                Pokemon p = new Pokemon();
                List<Pokemon> toRemove = new List<Pokemon>();
                foreach (Pokemon x in player.pokemon)
                {
                    if (isId)
                    {
                        if (x.id == id)
                        {
                            p = x;
                            toRemove.Add(p);
                            validMatch = true;
                        }
                    }
                    else
                    {
                        if (x.nickname.ToLower() == inputId)
                        {
                            p = x;
                            toRemove.Add(p);
                            validMatch = true;
                        }
                    }
                }
                if (toRemove.Count == 1)
                {
                    if (validMatch)
                    {
                        PokemonXpCalc pxc = new PokemonXpCalc(p);
                        int xpGain = pxc.baseXpGained * 2;
                        p.xp += xpGain;
                        int xpAtNextLevel = pxc.xpAtNextLevel;
                        bool hasLeveled = false;
                        bool hasEvolved = false;
                        EvolvePkg pkg = new EvolvePkg();
                        if (p.xp >= xpAtNextLevel)
                        {
                            p.level++;
                            EvolutionHandler eh = new EvolutionHandler();
                            pkg = eh.EvolveCheck(p, player);
                            hasLeveled = true;
                            hasEvolved = pkg.hasEvolved;
                        }
                        string nickname = p.nickname;
                        if (p.nickname == null)
                        {
                            nickname = p.pkmnName;
                        }
                        Random rand = new Random();
                        int trainChoice = rand.Next(1, 5);
                        string trainMessage = "";
                        switch (trainChoice)
                        {
                            case 1: trainMessage = $"{player.name} leads {nickname} through an obstacle course! {nickname} ducks and weaves over, under, and through all of the obstacles like a champ, and {player.name} gives them plenty of treats after."; break;
                            case 2: trainMessage = $"{player.name} teaches {nickname} how to shake hands/paws! {nickname} doesn't quite get it at first, but eventually, they're shaking every time. They're looking very proud of themselves, too!"; break;
                            case 3: trainMessage = $"{player.name} is showing {nickname} how to balance on a balance beam. {nickname} finds it a bit difficult, but they find their footing before too long. They look very serious, like they're concentraing very hard."; break;
                            case 4: trainMessage = $"{player.name} is exercising with {nickname}! They're both lifting weights. {nickname} is wanting to compete with {player.name}, a firey glint in their eye!"; break;
                            case 5: trainMessage = $"{player.name} and {nickname} are racing! They're neck and neck for most of the race, but at the last second, {nickname} pulls ahead!"; break;
                        }
                        sb.AppendLine($"{trainMessage}");
                        if (hasLeveled)
                        {
                            sb.AppendLine($"");
                            sb.AppendLine($"{nickname} has leveled up!");
                        }
                        if (hasEvolved)
                        {
                            p.dexNo = pkg.evolvedTo.dexNo;
                            p.pkmnName = pkg.evolvedTo.pkmnName;
                            p.type1 = pkg.evolvedTo.type1;
                            p.type2 = pkg.evolvedTo.type2;
                            p.speciesAbility1 = pkg.evolvedTo.speciesAbility1;
                            p.speciesAbility2 = pkg.evolvedTo.speciesAbility2;
                            p.baseStatTotal = pkg.evolvedTo.baseStatTotal;
                            p.baseHP = pkg.evolvedTo.baseHP;
                            p.baseAtk = pkg.evolvedTo.baseAtk;
                            p.baseDef = pkg.evolvedTo.baseDef;
                            p.baseSpAtk = pkg.evolvedTo.baseSpAtk;
                            p.baseSpDef = pkg.evolvedTo.baseSpDef;
                            p.baseSpd = pkg.evolvedTo.baseSpd;
                            p.evoLevel = pkg.evolvedTo.evoLevel;
                            p.legendStatus = pkg.evolvedTo.legendStatus;
                            p.mythicStatus = pkg.evolvedTo.mythicStatus;
                            p.nextEvo = pkg.evolvedTo.nextEvo;
                            p.prevEvo = pkg.evolvedTo.prevEvo;
                            p.eggGroup1 = pkg.evolvedTo.eggGroup1;
                            p.eggGroup2 = pkg.evolvedTo.eggGroup2;
                            p.levelSpeed = pkg.evolvedTo.levelSpeed;
                            p.genderThreshhold = pkg.evolvedTo.genderThreshhold;
                            p.blurb = pkg.evolvedTo.blurb;
                            sb.AppendLine($"{nickname} has evolved! They're now a {p.pkmnName}!");
                        }
                        foreach (Pokemon x in toRemove)
                        {
                            player.pokemon.Remove(x);
                        }
                        player.pokemon.Add(p);
                        bm.PlayerSave(player);
                        embed.Title = $"{player.name} decides to give their {p.pkmnName} some training.";
                    }
                    else
                    {
                        sb.AppendLine($"You don't have a Pokemon with that ID number!");
                        embed.Title = "No Pokemon with ID # " + id + " Found";
                    }
                }
                else
                {
                    sb.AppendLine($"Please either specify them by ID or re-nickname one to something else!");
                    embed.Title = $"You have multiple Pokemon with that nickname.";
                }
            }
            else
            {
                sb.AppendLine($"Please only enter digits (0-9) for your Pokemon's ID!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Feed", RunMode = RunMode.Async)]
        [Summary("Allows you to feed your Pokemon a treat cooked with the >cook command! Usage: >c feed [Pokemon ID] [name of food]")]
        [RequireBegan()]
        public async Task FeedCommand(string inputId, string name, string name2 = null, string name3 = null, string name4 = null, string name5 = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            if (name2 != null) { if (!name.Contains("-")) name += " " + name2; } else { name += name2; }
            if (name3 != null) { if (!name2.Contains("-")) name += " " + name3; } else { name += name3; }
            if (name4 != null) { if (!name3.Contains("-")) name += " " + name4; } else { name += name4; }
            if (name5 != null) { if (!name4.Contains("-")) name += " " + name5; } else { name += name5; }
            bool isId = true;
            Player player = bm.PlayerLoad(user.Id);
            bool validInput = Int32.TryParse(inputId, out int id);
            if (!validInput)
            {
                inputId = inputId.ToLower();
                foreach (Pokemon p in player.pokemon)
                {
                    if (p.nickname.ToLower() == inputId)
                    {
                        validInput = true;
                        isId = false;
                    }
                }
            }
            if (validInput)
            {
                bool validMatch = false;
                Pokemon p = new Pokemon();
                List<Pokemon> toRemove = new List<Pokemon>();
                foreach (Pokemon x in player.pokemon)
                {
                    if (isId)
                    {
                        if (x.id == id)
                        {
                            p = x;
                            toRemove.Add(p);
                            validMatch = true;
                        }
                    }
                    else
                    {
                        if (x.nickname.ToLower() == inputId)
                        {
                            p = x;
                            toRemove.Add(p);
                            validMatch = true;
                        }
                    }
                }
                if (toRemove.Count == 1)
                {
                    if (validMatch)
                    {
                        bool hasFood = false;
                        Food chosen = new Food();
                        foreach (Food f in player.lunchBox)
                        {
                            string fullName = f.type + " " + f.category;
                            if ((fullName.ToLower() == name.ToLower() || f.type.ToLower() == name.ToLower()) && f.qty >= 1)
                            {
                                hasFood = true;
                                if (f.category != "Poffin")
                                {
                                    f.qty--;
                                }
                                chosen = f;
                            }
                        }
                        if (hasFood)
                        {
                            string nature = p.nature.Substring(0, 1).ToUpper() + p.nature.Substring(1);
                            string likedFlavor = null;
                            string dislikedFlavor = null;
                            switch (nature)
                            {
                                case "Lonely": likedFlavor = "Spicy"; dislikedFlavor = "Sour"; break;
                                case "Brave": likedFlavor = "Spicy"; dislikedFlavor = "Sweet"; break;
                                case "Adamant": likedFlavor = "Spicy"; dislikedFlavor = "Dry"; break;
                                case "Naughty": likedFlavor = "Spicy"; dislikedFlavor = "Bitter"; break;
                                case "Bold": likedFlavor = "Sour"; dislikedFlavor = "Spicy"; break;
                                case "Relaxed": likedFlavor = "Sour"; dislikedFlavor = "Sweet"; break;
                                case "Impish": likedFlavor = "Sour"; dislikedFlavor = "Dry"; break;
                                case "Lax": likedFlavor = "Sour"; dislikedFlavor = "Bitter"; break;
                                case "Timid": likedFlavor = "Sweet"; dislikedFlavor = "Spicy"; break;
                                case "Hasty": likedFlavor = "Sweet"; dislikedFlavor = "Sour"; break;
                                case "Jolly": likedFlavor = "Sweet"; dislikedFlavor = "Dry"; break;
                                case "Naive": likedFlavor = "Sweet"; dislikedFlavor = "Bitter"; break;
                                case "Modest": likedFlavor = "Dry"; dislikedFlavor = "Spicy"; break;
                                case "Mild": likedFlavor = "Dry"; dislikedFlavor = "Sour"; break;
                                case "Quiet": likedFlavor = "Dry"; dislikedFlavor = "Sweet"; break;
                                case "Rash": likedFlavor = "Dry"; dislikedFlavor = "Bitter"; break;
                                case "Calm": likedFlavor = "Bitter"; dislikedFlavor = "Spicy"; break;
                                case "Gentle": likedFlavor = "Bitter"; dislikedFlavor = "Sour"; break;
                                case "Sassy": likedFlavor = "Bitter"; dislikedFlavor = "Sweet"; break;
                                case "Careful": likedFlavor = "Bitter"; dislikedFlavor = "Dry"; break;
                            }
                            switch (chosen.category)
                            {
                                case "Poffin":
                                    p.hasPoffin = true;
                                    double mult = 1;
                                    if (p.sheen < 255)
                                    {
                                        foreach (KeyValuePair<string, int> kvp in chosen.flavorProfile)
                                        {
                                            if (kvp.Key == "Spicy")
                                            {
                                                if (kvp.Key == likedFlavor)
                                                {
                                                    mult = 1.1;
                                                }
                                                else if (kvp.Key == dislikedFlavor)
                                                {
                                                    mult = 0.9;
                                                }
                                                p.coolness += Convert.ToInt32((mult * kvp.Value));
                                            }
                                            else if (kvp.Key == "Sweet")
                                            {
                                                if (kvp.Key == likedFlavor)
                                                {
                                                    mult = 1.1;
                                                }
                                                else if (kvp.Key == dislikedFlavor)
                                                {
                                                    mult = 0.9;
                                                }
                                                p.cuteness += Convert.ToInt32((mult * kvp.Value));
                                            }
                                            else if (kvp.Key == "Sour")
                                            {
                                                if (kvp.Key == likedFlavor)
                                                {
                                                    mult = 1.1;
                                                }
                                                else if (kvp.Key == dislikedFlavor)
                                                {
                                                    mult = 0.9;
                                                }
                                                p.toughness += Convert.ToInt32((mult * kvp.Value));
                                            }
                                            else if (kvp.Key == "Bitter")
                                            {
                                                if (kvp.Key == likedFlavor)
                                                {
                                                    mult = 1.1;
                                                }
                                                else if (kvp.Key == dislikedFlavor)
                                                {
                                                    mult = 0.9;
                                                }
                                                p.cleverness += Convert.ToInt32((mult * kvp.Value));
                                            }
                                            else if (kvp.Key == "Dry")
                                            {
                                                if (kvp.Key == likedFlavor)
                                                {
                                                    mult = 1.1;
                                                }
                                                else if (kvp.Key == dislikedFlavor)
                                                {
                                                    mult = 0.9;
                                                }
                                                p.beauty += Convert.ToInt32((mult * kvp.Value));
                                            }
                                        }
                                        p.sheen += chosen.smoothness;
                                        if (p.sheen > 255) { p.sheen = 255; }
                                    }
                                    sb.AppendLine($"Your {p.nickname} enjoys the tasty Poffin! They look content and relaxed; breeding might be easier now!");
                                    break;
                                case "Pokeblock":
                                    PokemonXpCalc x = new PokemonXpCalc(p);
                                    int xpMult = 1;
                                    switch (chosen.type)
                                    {
                                        case "Red":
                                            p.statAtk++;
                                            break;
                                        case "Blue":
                                            p.statDef++;
                                            break;
                                        case "Pink":
                                            p.statSpAtk++;
                                            break;
                                        case "Green":
                                            p.statSpDef++;
                                            break;
                                        case "Yellow":
                                            p.statSpd++;
                                            break;
                                        case "Rainbow":
                                            p.statAtk++;
                                            p.statDef++;
                                            p.statSpAtk++;
                                            p.statSpDef++;
                                            p.statSpd++;
                                            break;
                                        case "Red+":
                                            p.statAtk+= 2;
                                            xpMult = 2;
                                            break;
                                        case "Blue+":
                                            p.statDef+= 2;
                                            xpMult = 2;
                                            break;
                                        case "Pink+":
                                            p.statSpAtk+= 2;
                                            xpMult = 2;
                                            break;
                                        case "Green+":
                                            p.statSpDef+= 2;
                                            xpMult = 2;
                                            break;
                                        case "Yellow+":
                                            p.statSpd+= 2;
                                            xpMult = 2;
                                            break;
                                        case "Rainbow+":
                                            p.statAtk+= 2;
                                            p.statDef+= 2;
                                            p.statSpAtk+= 2;
                                            p.statSpDef+= 2;
                                            p.statSpd+= 2;
                                            xpMult = 2;
                                            break;
                                    }
                                    p.xp += x.baseXpGained * xpMult;
                                    bool hasLeveled = false;
                                    bool hasEvolved = false;
                                    EvolvePkg pkg = new EvolvePkg();
                                    if (p.xp >= x.xpAtNextLevel) 
                                    {
                                        p.level++;
                                        EvolutionHandler eh = new EvolutionHandler();
                                        pkg = eh.EvolveCheck(p, player);
                                        hasLeveled = true;
                                        hasEvolved = pkg.hasEvolved;
                                    }
                                    if (p.sheen < 255)
                                    {
                                        switch (chosen.type)
                                        {
                                            case "Red":
                                                p.coolness += 8;
                                                break;
                                            case "Blue":
                                                p.beauty += 8;
                                                break;
                                            case "Pink":
                                                p.cuteness += 8;
                                                break;
                                            case "Green":
                                                p.cleverness += 8;
                                                break;
                                            case "Yellow":
                                                p.toughness += 8;
                                                break;
                                            case "Rainbow":
                                                p.coolness += 8;
                                                p.beauty += 8;
                                                p.cuteness += 8;
                                                p.cleverness += 8;
                                                p.toughness += 8;
                                                break;
                                            case "Red+":
                                                p.coolness += 16;
                                                break;
                                            case "Blue+":
                                                p.beauty += 16;
                                                break;
                                            case "Pink+":
                                                p.cuteness += 16;
                                                break;
                                            case "Green+":
                                                p.cleverness += 16;
                                                break;
                                            case "Yellow+":
                                                p.toughness += 16;
                                                break;
                                            case "Rainbow+":
                                                p.coolness += 16;
                                                p.cuteness += 16;
                                                p.toughness += 16;
                                                p.cleverness += 16;
                                                p.beauty += 16;
                                                break;
                                        }
                                        p.sheen += chosen.smoothness;
                                    }
                                    if (p.sheen > 255) { p.sheen = 255; }
                                    sb.AppendLine($"You toss a {chosen.type} Pokeblock to your {p.nickname}, who gobbles it down gleefully. They seem to grow stronger before your eyes!");
                                    if (hasLeveled)
                                    {
                                        sb.AppendLine($"");
                                        sb.AppendLine($"{p.nickname} has leveled up!");
                                    }
                                    if (hasEvolved)
                                    {
                                        p.dexNo = pkg.evolvedTo.dexNo;
                                        p.pkmnName = pkg.evolvedTo.pkmnName;
                                        p.type1 = pkg.evolvedTo.type1;
                                        p.type2 = pkg.evolvedTo.type2;
                                        p.speciesAbility1 = pkg.evolvedTo.speciesAbility1;
                                        p.speciesAbility2 = pkg.evolvedTo.speciesAbility2;
                                        p.baseStatTotal = pkg.evolvedTo.baseStatTotal;
                                        p.baseHP = pkg.evolvedTo.baseHP;
                                        p.baseAtk = pkg.evolvedTo.baseAtk;
                                        p.baseDef = pkg.evolvedTo.baseDef;
                                        p.baseSpAtk = pkg.evolvedTo.baseSpAtk;
                                        p.baseSpDef = pkg.evolvedTo.baseSpDef;
                                        p.baseSpd = pkg.evolvedTo.baseSpd;
                                        p.evoLevel = pkg.evolvedTo.evoLevel;
                                        p.legendStatus = pkg.evolvedTo.legendStatus;
                                        p.mythicStatus = pkg.evolvedTo.mythicStatus;
                                        p.nextEvo = pkg.evolvedTo.nextEvo;
                                        p.prevEvo = pkg.evolvedTo.prevEvo;
                                        p.eggGroup1 = pkg.evolvedTo.eggGroup1;
                                        p.eggGroup2 = pkg.evolvedTo.eggGroup2;
                                        p.levelSpeed = pkg.evolvedTo.levelSpeed;
                                        p.genderThreshhold = pkg.evolvedTo.genderThreshhold;
                                        p.blurb = pkg.evolvedTo.blurb;
                                        sb.AppendLine($"{p.nickname} has evolved! They're now a {p.pkmnName}!");
                                    }
                                    break;
                                case "Pokepuff":
                                    int likedAmount = 0;
                                    if (chosen.flavorProfile.ContainsKey(likedFlavor)) { likedAmount = chosen.flavorProfile[likedFlavor]; }
                                    int dislikedAmount = 0;
                                    if (chosen.flavorProfile.ContainsKey(dislikedFlavor)) { dislikedAmount = chosen.flavorProfile[dislikedFlavor]; }
                                    int affectionChange = likedAmount;
                                    affectionChange -= dislikedAmount;
                                    if (affectionChange > 0)
                                    {
                                        p.affection += affectionChange;
                                    }
                                    if (affectionChange >= 5)
                                    {
                                        sb.AppendLine($"You hold out a {chosen.type} Pokepuff in your hand, and {p.nickname} gobbles it down happily. They seem to glow with happiness and affection toward you!");
                                    }
                                    else if (affectionChange >= 1)
                                    {
                                        sb.AppendLine($"You hold out a {chosen.type} Pokepuff in your hand, and {p.nickname} sniffs at it tentatively. They take a bite, and seem to settle for it. They still seem to be more affectionate toward you!");
                                    }
                                    else if (affectionChange < 0)
                                    {
                                        sb.AppendLine($"You hold out a {chosen.type} Pokepuff in your hand, and {p.nickname} sniffs at it. They stick their tongue out in disgust, and refuse to eat it! They don't seem to like you any less, but they really don't like the flavor profile...");
                                        foreach (Food f in player.lunchBox)
                                        {
                                            if (f.type == chosen.type && f.category == chosen.category)
                                            {
                                                f.qty++;
                                            }
                                        }
                                    }
                                    break;
                            }
                            foreach (Pokemon x in toRemove)
                            {
                                player.pokemon.Remove(x);
                            }
                            player.pokemon.Add(p);
                            bm.PlayerSave(player);
                            embed.Title = $"{player.name} decides to give their {p.pkmnName} a snack!";
                        }
                        else
                        {
                            sb.AppendLine($"You don't seem to have a food called {name} in your lunchbox! Please try again.");
                            embed.Title = $"Food Not Found.";
                        }
                    }
                    else
                    {
                        sb.AppendLine($"You don't have a Pokemon with that ID number!");
                        embed.Title = "No Pokemon with ID # " + id + " Found";
                    }
                }
                else
                {
                    sb.AppendLine($"Please either specify them by ID or re-nickname one to something else!");
                    embed.Title = $"You have multiple Pokemon with that nickname.";
                }
            }
            else
            {
                sb.AppendLine($"Please only enter digits (0-9) for your Pokemon's ID!");
                embed.Title = "Invalid Input";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }
    }
}

