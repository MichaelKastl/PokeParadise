using Discord;
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
    [Group("Shop")]
    public class ShopModuleModule : ModuleBase<SocketCommandContext>
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

        [Command("", RunMode = RunMode.Async)]
        [Summary("Use this command to view everything on sale at the shop! Usage: >shop")]
        [RequireBegan()]
        public async Task ShopCommand()
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            Dusty dusty = FetchDusty();
            sb.AppendLine($"When you walk in, Dusty is cleaning off the counter in front of you. She's an older woman, with a green ribbon holding her white hair back and a blue apron tied around her neck.");
            sb.AppendLine($"As you walk in, she looks up from her cleaning. \"Oh,\" she says, \"hello there! Welcome to Dusty's Odds and Ends. I've got some food, some Berries, and some other, well, odds and ends.\"");
            sb.AppendLine($"She laughs to herself and puts her hands on her hips. \"Well, feel free to browse. If ya want anythin', let me know!\" And with that, she goes back to cleaning.");
            sb.AppendLine();
            embed.Description = sb.ToString();
            foreach (Item i in dusty.inventory)
            {
                if (i.food == null && i.berry == null)
                {
                    embed.AddField($"**{i.shopId}: {i.name}**", $"*Price: {i.price} coins*");
                }
                else if (i.food != null && i.berry == null)
                {
                    if (i.food.level == 0)
                    {
                        embed.AddField($"**{i.shopId}: {i.food.type} {i.food.category}**", $"*Price: {i.price} coins*");
                    }
                    else
                    {
                        embed.AddField($"**{i.shopId}: {i.food.type} {i.food.category}**", $"*Level: {i.food.level}, Price: {i.price} coins*");
                    }
                }
                else if (i.food == null && i.berry != null)
                {
                    embed.AddField($"**{i.shopId}: {i.berry.name} Berry**", $"*Price: {i.price} coins*");
                }
            }
            embed.Title = $"**Dusty's Inventory**";
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Buy", RunMode = RunMode.Async)]
        [Summary("Use this command to buy things from the shop! Usage: >shop buy [shop ID number of item] [amount]")]
        [RequireBegan()]
        public async Task ShopBuyCommand(string shopNumberInput, string amountInput)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            Dusty dusty = FetchDusty();
            bool validShopNumber = Int32.TryParse(shopNumberInput, out int shopNumber);
            bool validAmount = Int32.TryParse(amountInput, out int amount);
            if (validAmount && validShopNumber)
            {
                if (shopNumber <= dusty.inventory.Count && shopNumber > 0)
                {
                    Item i = new Item();
                    foreach (Item x in dusty.inventory)
                    {
                        if (x.shopId == shopNumber)
                        {
                            i = x;
                        }
                    }
                    if (player.coins >= i.price * amount)
                    {
                        player.coins -= i.price * amount;
                        if (i.food == null && i.berry == null)
                        {
                            bool alreadyHas = false;
                            foreach (Item x in player.inventory)
                            {
                                if (x.name == i.name)
                                {
                                    alreadyHas = true;
                                    x.qty += amount;
                                }
                            }
                            if (!alreadyHas) { i.qty += amount; player.inventory.Add(i); }
                        }
                        else if (i.food != null && i.berry == null)
                        {
                            bool alreadyHas = false;
                            foreach (Food f in player.lunchBox)
                            {
                                if (f.type == i.food.type)
                                {
                                    alreadyHas = true;
                                    f.qty += amount;
                                }
                            }
                            if (!alreadyHas)
                            {
                                i.food.qty += amount;
                                player.lunchBox.Add(i.food);
                            }
                        }
                        else if (i.food == null && i.berry != null)
                        {
                            bool alreadyHas = false;
                            foreach (Berry b in player.berryPouch)
                            {
                                if (b.name == i.berry.name)
                                {
                                    alreadyHas = true;
                                    b.qty += amount;
                                }
                            }
                            if (!alreadyHas)
                            {
                                i.berry.qty += amount;
                                player.berryPouch.Add(i.berry);
                            }
                        }
                        player.coins += 900;
                        bm.PlayerSave(player);
                        sb.AppendLine($"Dusty gives you a warm smile and hands you your goods in a brown paper bag. \"It was good seein' ya, {player.name}. Come back soon!\"");
                        embed.Title = "Success!";
                    }
                    else
                    {
                        sb.AppendLine($"Dusty shakes her head and gives you a sympathetic look. \"I'm sorry, hon, but unless ya have the money, I can't give it to ya. Come back later " +
                            $"when you have the money, alright dear?\"");
                        embed.Title = "Insufficient Funds";
                    }
                }
                else
                {
                    sb.AppendLine($"Dusty furrows her brow as she looks around. She finally stands up and sighs. \"No, I'm sorry, {player.name}. I don't have anythin' like that in stock today.\"");
                    embed.Title = "Invalid Shop ID #";
                }
            }
            else
            {
                sb.AppendLine($"Dusty scratches her head, and then leans in a bit closer, tilting her head so that her right ear is closer to you. " +
                    $"\"I'm sorry, dear, please speak up. My hearin's not what it used to be.\"");
                embed.Title = "Invalid Input(s)";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Sell", RunMode = RunMode.Async)]
        [Summary("Use this command to sell unwanted things to the shop! Usage: >shop sell [qty] [item name]")]
        [RequireBegan()]
        public async Task ShopSellCommand(string amount, string name, string name2 = null, string name3 = null, string name4 = null, string name5 = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            bool validAmount = Int32.TryParse(amount, out int amt);
            name = name.Substring(0, 1).ToUpper() + name.Substring(1);
            if (validAmount)
            {
                if (name2 != null)
                {
                    if (name2.ToLower() == "berry") { name2 = null; }
                    else if (!name.Contains("-"))
                    {
                        name += " " + name2.Substring(0, 1).ToUpper() + name2.Substring(1);
                    }
                    else
                    {
                        name += name2.Substring(0, 1).ToUpper() + name2.Substring(1);
                    }
                }
                if (name3 != null)
                {
                    if (name3.ToLower() == "berry") { name3 = null; }
                    else if (!name2.Contains("-"))
                    {
                        name += " " + name3.Substring(0, 1).ToUpper() + name3.Substring(1);
                    }
                    else
                    {
                        name += name3.Substring(0, 1).ToUpper() + name3.Substring(1);
                    }
                }
                if (name4 != null)
                {
                    if (name4.ToLower() == "berry") { name4 = null; }
                    else if (!name3.Contains("-"))
                    {
                        name += " " + name4.Substring(0, 1).ToUpper() + name2.Substring(1);
                    }
                    else
                    {
                        name += name2.Substring(0, 1).ToUpper() + name2.Substring(1);
                    }
                }
                if (name5 != null)
                {
                    if (name5.ToLower() == "berry") { name5 = null; }
                    else if (!name4.Contains("-"))
                    {
                        name += " " + name2.Substring(0, 1).ToUpper() + name2.Substring(1);
                    }
                    else
                    {
                        name += name2.Substring(0, 1).ToUpper() + name2.Substring(1);
                    }
                }
                Player player = bm.PlayerLoad(user.Id);
                bool isItem = false;
                bool isBerry = false;
                bool isFood = false;
                foreach (Item i in player.inventory)
                {
                    if (i.name == name) { isItem = true; }
                }
                foreach (Berry b in player.berryPouch)
                {
                    if (b.name == name) { isBerry = true; }
                }
                foreach (Food f in player.lunchBox)
                {
                    string fullName = f.type + " " + f.category;
                    if (f.type == name || fullName == name) { isFood = true; }
                }
                if (isItem || isBerry || isFood)
                {
                    if (isItem)
                    {
                        foreach (Item i in player.inventory)
                        {
                            if (i.name == name)
                            {
                                if (i.qty >= amt)
                                {
                                    i.qty -= amt;
                                    int price = 3000;
                                    player.coins += price;
                                    sb.AppendLine($"Dusty smiles and takes your item, and hands you a bag of coins in return. \"Pleasure doin' business with ya. Come back soon, now, {player.name}!");
                                    embed.Title = "Success!";
                                }
                                else
                                {
                                    sb.AppendLine($"Dusty scratches her head. \"Well, sure, I'd be willin' to buy some {name}s, but it doesn't look like you've got {amt} with ya. " +
                                        $"Come back with more, and I'll take 'em off your hands, alright?\"");
                                    embed.Title = "Insufficient Quantity";
                                }
                            }
                        }
                    }
                    else if (isBerry)
                    {
                        foreach (Berry b in player.berryPouch)
                        {
                            if (b.name == name)
                            {
                                if (b.qty >= amt)
                                {
                                    b.qty -= amt;
                                    List<int> flavorValues = new List<int>();
                                    flavorValues.Add(b.sourValue);
                                    flavorValues.Add(b.sweetValue);
                                    flavorValues.Add(b.dryValue);
                                    flavorValues.Add(b.bitterValue);
                                    flavorValues.Add(b.spicyValue);
                                    int price = 10 * flavorValues.Max();
                                    player.coins += price;
                                    sb.AppendLine($"Dusty smiles and takes your item, and hands you a bag of coins in return. \"Pleasure doin' business with ya. Come back soon, now, {player.name}!");
                                    embed.Title = "Success!";
                                }
                                else
                                {
                                    sb.AppendLine($"Dusty scratches her head. \"Well, sure, I'd be willin' to buy some {name} Berries, but it doesn't look like you've got {amt} with ya. " +
                                        $"Come back with more, and I'll take 'em off your hands, alright?\"");
                                    embed.Title = "Insufficient Quantity";
                                }
                            }
                        }
                    }
                    else if (isFood)
                    {
                        foreach (Food f in player.lunchBox)
                        {
                            string fullName = f.type + " " + f.category;
                            if (f.type == name || fullName == name)
                            {
                                if (f.qty >= amt)
                                {
                                    int price = 0;
                                    switch (f.ingredients.Count)
                                    {
                                        case 1:
                                            if (f.flavorProfile.Values.Max() > 0)
                                            {
                                                price = 10 * f.flavorProfile.Values.Max();
                                            }
                                            else
                                            { price = 10; }
                                            break;
                                        case 2:
                                            price = 15 * f.flavorProfile.Values.Max();
                                            break;
                                        case 3:
                                            price = 25 * f.flavorProfile.Values.Max();
                                            break;
                                        case 4:
                                            price = 50 * f.flavorProfile.Values.Max();
                                            break;
                                    }
                                    f.qty -= amt;
                                    player.coins -= price;
                                    sb.AppendLine($"Dusty smiles and takes your item, and hands you a bag of coins in return. \"Pleasure doin' business with ya. Come back soon, now, {player.name}!");
                                    embed.Title = "Success!";
                                }
                                else
                                {
                                    sb.AppendLine($"Dusty scratches her head. \"Well, sure, I'd be willin' to buy some {name} {f.category}s, but it doesn't look like you've got {amt} with ya. " +
                                        $"Come back with more, and I'll take 'em off your hands, alright?\"");
                                    embed.Title = "Insufficient Quantity";
                                }
                            }
                        }
                    }
                    bm.PlayerSave(player);
                }
                else if (!isItem && !isBerry && !isFood)
                {
                    sb.AppendLine($"Dusty looks at you in confusion and mild concern. \"Hon, are you feelin' okay? You don't have any {name} with ya.\"");
                    embed.Title = $"No Item Named {name} Found In Your Inventory";
                }
            }
            else
            {
                sb.AppendLine($"Dusty looks at you in confusion and mild concern. \"Hon, are you feeling okay? I have no idea what you're talkin' about.\"");
                embed.Title = $"Invalid Amount; Must Be A Digit (0-9)";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            await ReplyAsync(null, false, embed.Build());
        }

        public Dusty FetchDusty()
        {
            CookingModule cm = new CookingModule();
            Dusty dusty = new Dusty();
            BinaryFormatter formatter = new BinaryFormatter();
            object lck = new object();
            lock (lck)
            {
                Stream s = File.Open("D:\\PokeParadise Files\\Shop.xml", FileMode.OpenOrCreate);
                if (s.Length != 0)
                {
                    dusty = (Dusty)formatter.Deserialize(s);
                }
                s.Close();
                TimeSpan dustyCooldown = new TimeSpan(3, 0, 0);
                bool forceReset = false;
                if (dusty.inventory == null || DateTimeOffset.Now - dusty.lastRestock > dustyCooldown || forceReset)
                {
                    dusty.inventory = new List<Item>();
                    Random rand = new Random();
                    int amtBerries = rand.Next(1, 5);
                    int amtFood = rand.Next(1, 3);
                    int amtItems = rand.Next(1, 2);
                    int cnt = 1;
                    while (cnt <= amtBerries)
                    {
                        Berry b = new Berry(rand.Next(1, 64));
                        List<int> flavorValues = new List<int>();
                        flavorValues.Add(b.sourValue);
                        flavorValues.Add(b.sweetValue);
                        flavorValues.Add(b.bitterValue);
                        flavorValues.Add(b.dryValue);
                        flavorValues.Add(b.spicyValue);
                        int berryPrice = 10 * flavorValues.Max();
                        Item i = new Item(b, berryPrice, cnt);
                        dusty.inventory.Add(i);
                        cnt++;
                    }
                    cnt = 1;
                    while (cnt <= amtFood)
                    {
                        int price = 0;
                        int foodChoice = rand.Next(1, 3);
                        string foodType = null;
                        switch (foodChoice)
                        {
                            case 1:
                                foodType = "Poffin";
                                break;
                            case 2:
                                foodType = "Pokeblock";
                                break;
                            case 3:
                                foodType = "Pokepuff";
                                break;
                        }
                        int ingRoll = rand.Next(1, 12);
                        int ingAmt = 0;
                        switch (ingRoll)
                        {
                            case 1:
                                ingAmt = 1;
                                break;
                            case 2:
                                ingAmt = 1;
                                break;
                            case 3:
                                ingAmt = 1;
                                break;
                            case 4:
                                ingAmt = 1;
                                break;
                            case 5:
                                ingAmt = 1;
                                break;
                            case 6:
                                ingAmt = 1;
                                break;
                            case 7:
                                ingAmt = 2;
                                break;
                            case 8:
                                ingAmt = 2;
                                break;
                            case 9:
                                ingAmt = 2;
                                break;
                            case 10:
                                ingAmt = 3;
                                break;
                            case 11:
                                ingAmt = 3;
                                break;
                            case 12:
                                ingAmt = 1;
                                break;
                        }
                        int ingCnt = 1;
                        List<string> ings = new List<string>();
                        while (ingCnt <= ingAmt)
                        {
                            Berry b = new Berry(rand.Next(1, 64));
                            ings.Add(b.name);
                            ingCnt++;
                        }
                        Food food = new Food();
                        switch (ings.Count)
                        {
                            case 1:
                                food = cm.ExternalCookingHandler(foodType, ings[0]);
                                if (food.flavorProfile.Values.Max() > 0)
                                {
                                    price = 10 * food.flavorProfile.Values.Max();
                                }
                                else
                                { price = 10; }
                                break;
                            case 2:
                                food = cm.ExternalCookingHandler(foodType, ings[0], ings[1]);
                                price = 15 * food.flavorProfile.Values.Max();
                                break;
                            case 3:
                                food = cm.ExternalCookingHandler(foodType, ings[0], ings[1], ings[2]);
                                price = 25 * food.flavorProfile.Values.Max();
                                break;
                            case 4:
                                food = cm.ExternalCookingHandler(foodType, ings[0], ings[1], ings[2], ings[3]);
                                price = 50 * food.flavorProfile.Values.Max();
                                break;
                        }
                        int id = dusty.inventory.Count + cnt;
                        Item i = new Item(food, price, id);
                        dusty.inventory.Add(i);
                        cnt++;
                    }
                    cnt = 1;
                    while (cnt <= amtItems)
                    {
                        int stoneChoice = rand.Next(1, 35);
                        string stone = null;
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
                        int id = dusty.inventory.Count + cnt;
                        Item i = new Item(3000, stone, id);
                        dusty.inventory.Add(i);
                        cnt++;
                    }
                }
                dusty.lastRestock = DateTimeOffset.Now;
                bm.Serialize(dusty, "Shop");
                return dusty;
            }
        }
    }
}
