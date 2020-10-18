using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeParadise.Modules
{
    [Group("Cook")]
    public class CookingModule : InteractiveBase<SocketCommandContext>
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

        BaseModule bm = new BaseModule();

        [Command("Poffin", RunMode = RunMode.Async)]
        [Summary("Helps you cook a Poffin! These treats make a Pokemon breed faster. This treat also increases a Pokemon's Sheen for Contests! To use this command, you may specify anywhere between 1 and 4 different Berry names for ingredients! Only use the name, not the word \"Berry\"! For example, a Cheri berry would just be named as \"cheri\". Usage: >cook poffin [ingredient 1] [ingredient 2] [ingredient 3] [ingredient 4]")]
        [RequireBegan()]
        public async Task CookPoffinCommand(string ingredient1, string ingredient2 = null, string ingredient3 = null, string ingredient4 = null)
        {
            var user = Context.User;
            var embed = CookingHandler(user, "Poffin", ingredient1, ingredient2, ingredient3, ingredient4);
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Pokeblock", RunMode = RunMode.Async)]
        [Summary("Helps you cook a Pokeblock! These treats raise a Pokemon's stat scores, as well as giving them an amount of experience. This treat also increases a Pokemon's Sheen for Contests! To use this command, you may specify anywhere between 1 and 4 different Berry names for ingredients! Only use the name, not the word \"Berry\"! For example, a Cheri berry would just be named as \"cheri\". Usage: >cook poffin [ingredient 1] [ingredient 2] [ingredient 3] [ingredient 4]")]
        [RequireBegan()]
        public async Task CookPokeblockCommand(string ingredient1, string ingredient2 = null, string ingredient3 = null, string ingredient4 = null)
        {
            var user = Context.User;
            var embed = CookingHandler(user, "Pokeblock", ingredient1, ingredient2, ingredient3, ingredient4);
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("Pokepuff", RunMode = RunMode.Async)]
        [Summary("Helps you cook a Pokepuff! These treats sharply raise a Pokemon's Affection. To use this command, you may specify anywhere between 1 and 4 different Berry names for ingredients! Only use the name, not the word \"Berry\"! For example, a Cheri berry would just be named as \"cheri\". Usage: >cook poffin [ingredient 1] [ingredient 2] [ingredient 3] [ingredient 4]")]
        [RequireBegan()]
        public async Task CookPokepuffCommand(string ingredient1, string ingredient2 = null, string ingredient3 = null, string ingredient4 = null)
        {
            var user = Context.User;
            var embed = CookingHandler(user, "Pokepuff", ingredient1, ingredient2, ingredient3, ingredient4);
            await ReplyAsync(null, false, embed.Build());
        }

        public EmbedBuilder CookingHandler(IUser user, string cookingType, string ingredient1, string ingredient2, string ingredient3, string ingredient4)
        {
            ingredient1 = ingredient1.Substring(0, 1).ToUpper() + ingredient1.Substring(1);
            if (ingredient2 != null)
            {
                ingredient2 = ingredient2.Substring(0, 1).ToUpper() + ingredient2.Substring(1);
            }
            if (ingredient3 != null)
            {
                ingredient3 = ingredient3.Substring(0, 1).ToUpper() + ingredient3.Substring(1);
            }
            if (ingredient4 != null)
            {
                ingredient4 = ingredient4.Substring(0, 1).ToUpper() + ingredient4.Substring(1);
            }
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            Player player = bm.PlayerLoad(user.Id);
            List<Berry> ingredients = new List<Berry>();
            List<string> invalidNames = new List<string>();
            Berry b = new Berry(ingredient1);
            bool allValid = true;
            if (b.number != 0)
            {
                ingredients.Add(b);
            }
            else { allValid = false; invalidNames.Add(ingredient1); }
            if (ingredient2 != null)
            {
                b = new Berry(ingredient2);
                if (b.number != 0)
                {
                    ingredients.Add(b);
                }
                else { allValid = false; invalidNames.Add(ingredient2); }
            }
            if (ingredient3 != null)
            {
                b = new Berry(ingredient3);
                if (b.number != 0)
                {
                    ingredients.Add(b);
                }
                else { allValid = false; invalidNames.Add(ingredient3); }
            }
            if (ingredient4 != null)
            {
                b = new Berry(ingredient4);
                if (b.number != 0)
                {
                    ingredients.Add(b);
                }
                else { allValid = false; invalidNames.Add(ingredient4); }
            }
            if (allValid)
            {
                List<Berry> noBerries = new List<Berry>();
                foreach (Berry x in ingredients)
                {
                    bool hasBerry = false;
                    foreach (Berry y in player.berryPouch)
                    {
                        if (x.name == y.name && y.qty >= 1) { hasBerry = true; }
                    }
                    if (!hasBerry) { noBerries.Add(x); }
                }
                if (noBerries.Count == 0)
                {
                    string cookingImplement = null;
                    switch (cookingType)
                    {
                        case "Poffin": cookingImplement = "Poffin batter"; break;
                        case "Pokeblock": cookingImplement = "Berry Blender"; break;
                        case "Pokepuff": cookingImplement = "Pokepuff batter"; break;
                    }
                    if (ingredients.Count == 1)
                    {
                        string berry = ingredients.ElementAt(0).name;
                        sb.AppendLine($"You throw the {berry} berry into the {cookingImplement}.");
                    }
                    else if (ingredients.Count == 2)
                    {
                        string berry1 = ingredients.ElementAt(0).name;
                        string berry2 = ingredients.ElementAt(1).name;
                        sb.AppendLine($"You throw the {berry1} and {berry2} berries into the {cookingImplement}.");
                    }
                    else if (ingredients.Count == 3)
                    {
                        string berry1 = ingredients.ElementAt(0).name;
                        string berry2 = ingredients.ElementAt(1).name;
                        string berry3 = ingredients.ElementAt(2).name;
                        sb.AppendLine($"You throw the {berry1}, {berry2}, and {berry3} berries into the {cookingImplement}.");
                    }
                    else
                    {
                        string berry1 = ingredients.ElementAt(0).name;
                        string berry2 = ingredients.ElementAt(1).name;
                        string berry3 = ingredients.ElementAt(2).name;
                        string berry4 = ingredients.ElementAt(3).name;
                        sb.AppendLine($"You throw the {berry1}, {berry2}, {berry3}, and {berry4} berries into the {cookingImplement}.");
                    }
                    foreach (Berry ing in ingredients)
                    {
                        foreach (Berry owned in player.berryPouch)
                        {
                            if (owned.number == ing.number)
                            {
                                owned.qty--;
                            }
                        }
                    }
                    sb.AppendLine($"");
                    Random rand = new Random();
                    int successTest = rand.Next(1, 100);
                    if (player.cookingSuccessMult > 0)
                    {
                        successTest += player.cookingSuccessMult * 2;
                    }
                    if (successTest >= 50)
                    {
                        if (cookingType == "Poffin")
                        {
                            string poffinType = null;
                            int sweetCount = 0;
                            int spicyCount = 0;
                            int bitterCount = 0;
                            int sourCount = 0;
                            int dryCount = 0;
                            int spicyTotal = 0;
                            int sourTotal = 0;
                            int bitterTotal = 0;
                            int sweetTotal = 0;
                            int dryTotal = 0;
                            int penalty = 0;
                            List<string> berryNames = new List<string>();
                            int smoothnessTotal = 0;
                            int ingTotal = ingredients.Count();
                            int foodSmoothness;
                            foreach (Berry ing in ingredients)
                            {
                                sweetCount += ing.sweetValue;
                                spicyCount += ing.spicyValue;
                                bitterCount += ing.bitterValue;
                                sourCount += ing.sourValue;
                                dryCount += ing.dryValue;
                                spicyTotal = spicyCount;
                                sourTotal = sourCount;
                                bitterTotal = bitterCount;
                                sweetTotal = sweetCount;
                                dryTotal = dryCount;
                                berryNames.Add(ing.name);
                                smoothnessTotal += ing.smoothness;
                            }
                            if (berryNames.Count() == berryNames.Distinct().Count())
                            {
                                poffinType = "Foul";
                            }
                            spicyTotal -= dryCount;
                            if (spicyTotal < 0) { penalty++; }
                            sourTotal -= spicyCount;
                            if (sourTotal < 0) { penalty++; }
                            bitterTotal -= sourCount;
                            if (bitterTotal < 0) { penalty++; }
                            sweetTotal -= bitterCount;
                            if (sweetTotal < 0) { penalty++; }
                            dryTotal -= sweetCount;
                            if (dryTotal < 0) { penalty++; }
                            spicyTotal -= penalty;
                            if (spicyTotal < 0) { spicyTotal = 0; }
                            sourTotal -= penalty;
                            if (sourTotal < 0) { sourTotal = 0; }
                            bitterTotal -= penalty;
                            if (bitterTotal < 0) { bitterTotal = 0; }
                            sweetTotal -= penalty;
                            if (sweetTotal < 0) { sweetTotal = 0; }
                            dryTotal -= penalty;
                            if (dryTotal < 0) { dryTotal = 0; }
                            Dictionary<string, int> flavorTotals = new Dictionary<string, int>();
                            Dictionary<string, int> flavorProfile = new Dictionary<string, int>();
                            flavorTotals.Add("Spicy", spicyTotal);
                            flavorTotals.Add("Sour",sourTotal);
                            flavorTotals.Add("Bitter", bitterTotal);
                            flavorTotals.Add("Sweet", sweetTotal);
                            flavorTotals.Add("Dry", dryTotal);
                            int flavorLevel = flavorTotals.Values.Max();
                            int flavors = 0;
                            foreach (KeyValuePair<string, int> flavorTotal in flavorTotals)
                            {
                                if (flavorTotal.Value > 0)
                                {
                                    flavors++;
                                    flavorProfile.Add(flavorTotal.Key, flavorTotal.Value);
                                }
                            }
                            string flavor = null;
                            if (flavors <= 2)
                            {
                                string primaryFlavor = null;
                                foreach (KeyValuePair<string, int> kvp in flavorTotals)
                                {
                                    if (kvp.Value == flavorLevel) { primaryFlavor = kvp.Key; }
                                }
                                flavorTotals.Remove(flavorTotals.Keys.Max());
                                flavor = primaryFlavor;
                                if (flavorTotals.Values.Max() > 0)
                                {
                                    foreach (KeyValuePair<string, int> kvp in flavorTotals)
                                    {
                                        if (kvp.Value == flavorLevel) { flavor = "-" + kvp.Key; }
                                    }
                                }
                            }
                            else if (flavors == 3)
                            {
                                flavor = "Rich";
                            }
                            else if (flavors == 4)
                            {
                                flavor = "Overripe";
                            }
                            if (flavorLevel >= 50)
                            {
                                flavor = "Mild";
                            }
                            if (poffinType == "Foul")
                            {
                                flavor = "Foul";
                            }
                            foodSmoothness = (smoothnessTotal / ingTotal) - ingTotal;
                            Food poffin = new Food("Poffin", flavor, flavorLevel, flavorProfile, foodSmoothness, ingredients, 1);
                            bool alreadyOwns = false;
                            foreach (Food food in player.lunchBox)
                            {
                                if (food.type == poffin.type && food.category == poffin.category)
                                {
                                    food.qty++;
                                    alreadyOwns = true;
                                }
                            }
                            if (!alreadyOwns) { player.lunchBox.Add(poffin); }
                            bm.PlayerSave(player);
                            sb.AppendLine($"You wipe the sweat off your brow as you pull a nice, tasty Level {flavorLevel} {flavor} Poffin out of the oven! Your Pokemon will love it!");
                            embed.Title = "Cooking Success!";
                        }
                        else if (cookingType == "Pokeblock")
                        {
                            Dictionary<string, int> flavorProfile = new Dictionary<string, int>();
                            string pokeblockType;
                            int redCount = 0;
                            int blueCount = 0;
                            int greenCount = 0;
                            int yellowCount = 0;
                            int pinkCount = 0;
                            int sweetCount = 0;
                            int sourCount = 0;
                            int dryCount = 0;
                            int bitterCount = 0;
                            int spicyCount = 0;
                            int smoothnessTotal = 0;
                            int ingTotal = ingredients.Count();
                            int foodSmoothness;
                            Dictionary<string, int> colorCounts = new Dictionary<string, int>();
                            foreach (Berry ing in ingredients)
                            {
                                switch (ing.color)
                                {
                                    case "Red": redCount++; break;
                                    case "Blue": blueCount++; break;
                                    case "Green": greenCount++; break;
                                    case "Yellow": yellowCount++; break;
                                    case "Pink": pinkCount++; break;
                                }
                                sweetCount += ing.sweetValue;
                                sourCount += ing.sourValue;
                                bitterCount += ing.bitterValue;
                                dryCount += ing.dryValue;
                                spicyCount += ing.spicyValue;
                                smoothnessTotal += ing.smoothness;
                            }
                            int spicyTotal = spicyCount;
                            int bitterTotal = bitterCount;
                            int dryTotal = dryCount;
                            int sourTotal = sourCount;
                            int sweetTotal = sweetCount;
                            int penalty = 0;
                            spicyTotal -= dryCount;
                            if (spicyTotal < 0) { penalty++; }
                            sourTotal -= spicyCount;
                            if (sourTotal < 0) { penalty++; }
                            bitterTotal -= sourCount;
                            if (bitterTotal < 0) { penalty++; }
                            sweetTotal -= bitterCount;
                            if (sweetTotal < 0) { penalty++; }
                            dryTotal -= sweetCount;
                            if (dryTotal < 0) { penalty++; }
                            int cnt = 1;
                            while (cnt <= penalty)
                            {
                                spicyTotal -= penalty;
                                if (spicyTotal < 0) { spicyTotal = 0; }
                                sweetTotal -= penalty;
                                if (sweetTotal < 0) { sweetTotal = 0; }
                                sourTotal -= penalty;
                                if (sourTotal < 0) { sourTotal = 0; }
                                dryTotal -= penalty;
                                if (dryTotal < 0) { dryTotal = 0; }
                                bitterTotal -= penalty;
                                if (bitterTotal < 0) { bitterTotal = 0; }
                                cnt++;
                            }
                            flavorProfile.Add("Sweet", sweetTotal);
                            flavorProfile.Add("Sour", sourTotal);
                            flavorProfile.Add("Bitter", bitterTotal);
                            flavorProfile.Add("Dry", dryTotal);
                            flavorProfile.Add("Spicy", spicyTotal);
                            colorCounts.Add("Red", redCount);
                            colorCounts.Add("Blue", blueCount);
                            colorCounts.Add("Green", greenCount);
                            colorCounts.Add("Yellow", yellowCount);
                            colorCounts.Add("Pink", pinkCount);
                            int distinctColors = 0;
                            foreach (KeyValuePair<string, int> kvp in colorCounts)
                            { if (kvp.Value > 0) { distinctColors++; } }
                            string color = null;
                            if (distinctColors <= 3)
                            {
                                Dictionary<string, int> mostColors = new Dictionary<string, int>();
                                foreach (KeyValuePair<string, int> kvp in colorCounts)
                                { if (kvp.Value == colorCounts.Values.Max()) { mostColors.Add(kvp.Key, kvp.Value); } }
                                if (mostColors.Count() == 1)
                                { color = mostColors.First().Key; }
                                else
                                {
                                    int selected = rand.Next(1, mostColors.Count());
                                    KeyValuePair<string, int> selection = mostColors.ElementAt(selected - 1);
                                    color = selection.Key;
                                }
                            }
                            else
                            {
                                color = "Rainbow";
                            }
                            decimal highestPercentage = 0;
                            foreach (Berry ing in ingredients)
                            {
                                if (ing.color == color)
                                {
                                    if (ing.plusChance > highestPercentage) { highestPercentage = ing.plusChance; }
                                }
                            }
                            pokeblockType = color;
                            foodSmoothness = (smoothnessTotal / ingTotal) - ingTotal;
                            decimal rollForPlus = Convert.ToDecimal(rand.NextDouble());
                            if (rollForPlus <= highestPercentage) { pokeblockType += "+"; }
                            Food pokeblock = new Food("Pokeblock", pokeblockType, 0, flavorProfile, foodSmoothness, ingredients, 1);
                            bool alreadyOwns = false;
                            foreach (Food food in player.lunchBox)
                            {
                                if (food.type == pokeblock.type && food.category == pokeblock.category)
                                {
                                    food.qty++;
                                    alreadyOwns = true;
                                }
                            }
                            if (!alreadyOwns) { player.lunchBox.Add(pokeblock); }
                            bm.PlayerSave(player);
                            sb.AppendLine($"You wipe the sweat off your brow as you pull a nice, tasty {pokeblockType} Pokeblock out of the Berry Blender! Your Pokemon will love it!");
                            embed.Title = "Cooking Success!";
                        }
                        else if (cookingType == "Pokepuff")
                        {
                            // Sweet = sweet, Mint = bitter, Citrus = sour, Mocha = dry, and Spice = spicy. Basic, frosted, fancy, deluxe, supreme = puff types.
                            string pokepuffType;
                            int sweetCount = 0;
                            int spicyCount = 0;
                            int bitterCount = 0;
                            int sourCount = 0;
                            int dryCount = 0;
                            int spicyTotal = 0;
                            int sourTotal = 0;
                            int bitterTotal = 0;
                            int sweetTotal = 0;
                            int dryTotal = 0;
                            int penalty = 0;
                            foreach (Berry ing in ingredients)
                            {
                                sweetCount += ing.sweetValue;
                                spicyCount += ing.spicyValue;
                                bitterCount += ing.bitterValue;
                                sourCount += ing.sourValue;
                                dryCount += ing.dryValue;
                                spicyTotal = spicyCount;
                                sourTotal = sourCount;
                                bitterTotal = bitterCount;
                                sweetTotal = sweetCount;
                                dryTotal = dryCount;
                            }
                            spicyTotal -= dryCount;
                            if (spicyTotal < 0) { penalty++; }
                            sourTotal -= spicyCount;
                            if (sourTotal < 0) { penalty++; }
                            bitterTotal -= sourCount;
                            if (bitterTotal < 0) { penalty++; }
                            sweetTotal -= bitterCount;
                            if (sweetTotal < 0) { penalty++; }
                            dryTotal -= sweetCount;
                            if (dryTotal < 0) { penalty++; }
                            spicyTotal -= penalty;
                            if (spicyTotal < 0) { spicyTotal = 0; }
                            sourTotal -= penalty;
                            if (sourTotal < 0) { sourTotal = 0; }
                            bitterTotal -= penalty;
                            if (bitterTotal < 0) { bitterTotal = 0; }
                            sweetTotal -= penalty;
                            if (sweetTotal < 0) { sweetTotal = 0; }
                            dryTotal -= penalty;
                            if (dryTotal < 0) { dryTotal = 0; }
                            Dictionary<string, int> flavorTotals = new Dictionary<string, int>();
                            Dictionary<string, int> flavorProfile = new Dictionary<string, int>();
                            flavorTotals.Add("Spicy", spicyTotal);
                            flavorTotals.Add("Sour", sourTotal);
                            flavorTotals.Add("Bitter", bitterTotal);
                            flavorTotals.Add("Sweet", sweetTotal);
                            flavorTotals.Add("Dry", dryTotal);
                            int flavorLevel = flavorTotals.Values.Max();
                            int flavors = 0;
                            foreach (KeyValuePair<string, int> flavorTotal in flavorTotals)
                            {
                                if (flavorTotal.Value > 0)
                                {
                                    flavors++;
                                    flavorProfile.Add(flavorTotal.Key, flavorTotal.Value);
                                }
                            }
                            string flavor = null;
                            if (flavors <= 2)
                            {
                                string primaryFlavor = null;
                                foreach (KeyValuePair<string, int> kvp in flavorTotals)
                                {
                                    if (kvp.Value == flavorLevel) { primaryFlavor = kvp.Key; }
                                }
                                flavorTotals.Remove(primaryFlavor);
                                flavor = primaryFlavor;
                                if (flavorTotals.Values.Max() > 0)
                                {
                                    foreach (KeyValuePair<string, int> kvp in flavorTotals)
                                        if (kvp.Value == flavorTotals.Values.Max()) { flavor += "-" + kvp.Key; }
                                }
                            }
                            else if (flavors == 3) { flavor = "Robust"; }
                            else if (flavors == 4) { flavor = "Overwhelming"; }
                            string level = null;
                            if (flavorLevel < 10) { level = "Basic"; }
                            else if (flavorLevel < 25) { level = "Frosted"; }
                            else if (flavorLevel < 35) { level = "Fancy"; }
                            else if (flavorLevel < 50) { level = "Deluxe"; }
                            else if (flavorLevel >= 50) { level = "Supreme"; }
                            pokepuffType = level + " " + flavor;
                            Food pokepuff = new Food("Pokepuff", pokepuffType, 0, flavorProfile, ingredients, 1);
                            bool alreadyOwns = false;
                            foreach (Food food in player.lunchBox)
                            {
                                if (food.type == pokepuff.type && food.category == pokepuff.category)
                                {
                                    food.qty++;
                                    alreadyOwns = true;
                                }
                            }
                            if (!alreadyOwns) { player.lunchBox.Add(pokepuff); }
                            bm.PlayerSave(player);
                            sb.AppendLine($"You wipe the sweat off your brow as you pull a Pokepuff tin with a nice, tasty {pokepuffType} Pokepuff on it out of the oven! Your Pokemon will love it!");
                            embed.Title = "Cooking Success!";
                        }
                    }
                    else
                    {
                        int failureReason = rand.Next(1, 3);
                        if (cookingType == "Poffin")
                        {
                            switch (failureReason)
                            {
                                case 1: sb.AppendLine($"You pull the charred remains of your Poffin out of the oven. Looks like you burnt it... No Pokemon is going to want to eat this, not even a Fire type!"); break;
                                case 2: sb.AppendLine($"You pull the tray with your Poffin on it out of the oven, but you quickly realize that something isn't right. It looks... Deflated. *Something* went wrong, somewhere. You're not sure where though."); break;
                                case 3: sb.AppendLine($"You pull the tray with your Poffin on it out of the oven, and go to remove it from the tray... Only to find that it's as hard as a rock. Oh no! It must have gotten overcooked..."); break;
                            }
                        }
                        else if (cookingType == "Pokeblock")
                        {
                            switch (failureReason)
                            {
                                case 1: sb.AppendLine($"You start the blending process, but you forgot to close the lid! Berry juice goes flying everywhere! Now you'll have to start over..."); break;
                                case 2: sb.AppendLine($"As you pull the finished product out of the Berry Blender, the potential Pokeblock falls apart in your hand! You must have not added enough milk..."); break;
                                case 3: sb.AppendLine($"The Pokeblock that comes out is of a questionable brown color... The berries must have gone bad. You throw it in the trash."); break;
                            }
                        }
                        else if (cookingType == "Pokepuff")
                        {
                            switch (failureReason)
                            {
                                case 1: sb.AppendLine($"You pull the Pokepuff tin out of the oven, only to find the bottom of the oven covered in Pokepuff batter. When you look at the tin, you find a hole in the bottom! The batter must have leaked through..."); break;
                                case 2: sb.AppendLine($"You open the oven to check on your Pokepuff, only to find that it overflowed out of the tin! You must have added too much batter! You'll have to start over."); break;
                                case 3: sb.AppendLine($"You pull the Pokepuff out of the oven, but it looks brown and cracked. It must have burnt! Not even a Fire type would like something this burnt..."); break;
                            }
                        }
                        embed.Title = "Cooking Failure!";
                    }
                }
                else
                {
                    foreach (Berry x in noBerries)
                    {
                        sb.AppendLine($"You don't have any {x.name} berries!");
                    }
                    embed.Title = "You don't have one or more of the ingredients you specified!";
                }
            }
            else
            {
                foreach (string name in invalidNames)
                {
                    sb.AppendLine($"\"{name}\" is not a valid berry name.");
                }
                embed.Title = "One or more of your berry names were invalid!";
            }
            embed.Description = sb.ToString();
            embed.WithColor(new Color(247, 89, 213));
            return embed;
        }

        public Food ExternalCookingHandler(string cookingType, string ingredient1, string ingredient2 = null, string ingredient3 = null, string ingredient4 = null)
        {
            Food food = new Food();
            ingredient1 = ingredient1.Substring(0, 1).ToUpper() + ingredient1.Substring(1);
            if (ingredient2 != null)
            {
                ingredient2 = ingredient2.Substring(0, 1).ToUpper() + ingredient2.Substring(1);
            }
            if (ingredient3 != null)
            {
                ingredient3 = ingredient3.Substring(0, 1).ToUpper() + ingredient3.Substring(1);
            }
            if (ingredient4 != null)
            {
                ingredient4 = ingredient4.Substring(0, 1).ToUpper() + ingredient4.Substring(1);
            }
            List<Berry> ingredients = new List<Berry>();
            Berry b = new Berry(ingredient1);
            ingredients.Add(b);
            if (ingredient2 != null)
            {
                b = new Berry(ingredient2);
                ingredients.Add(b);
            }
            if (ingredient3 != null)
            {
                b = new Berry(ingredient3);
                ingredients.Add(b);
            }
            if (ingredient4 != null)
            {
                b = new Berry(ingredient4);
                ingredients.Add(b);
            }
            Random rand = new Random();
            if (cookingType == "Poffin")
            {
                string poffinType = null;
                int sweetCount = 0;
                int spicyCount = 0;
                int bitterCount = 0;
                int sourCount = 0;
                int dryCount = 0;
                int spicyTotal = 0;
                int sourTotal = 0;
                int bitterTotal = 0;
                int sweetTotal = 0;
                int dryTotal = 0;
                int penalty = 0;
                List<string> berryNames = new List<string>();
                foreach (Berry ing in ingredients)
                {
                    sweetCount += ing.sweetValue;
                    spicyCount += ing.spicyValue;
                    bitterCount += ing.bitterValue;
                    sourCount += ing.sourValue;
                    dryCount += ing.dryValue;
                    spicyTotal = spicyCount;
                    sourTotal = sourCount;
                    bitterTotal = bitterCount;
                    sweetTotal = sweetCount;
                    dryTotal = dryCount;
                    berryNames.Add(ing.name);
                }
                if (berryNames.Count() == berryNames.Distinct().Count())
                {
                    poffinType = "Foul";
                }
                spicyTotal -= dryCount;
                if (spicyTotal < 0) { penalty++; }
                sourTotal -= spicyCount;
                if (sourTotal < 0) { penalty++; }
                bitterTotal -= sourCount;
                if (bitterTotal < 0) { penalty++; }
                sweetTotal -= bitterCount;
                if (sweetTotal < 0) { penalty++; }
                dryTotal -= sweetCount;
                if (dryTotal < 0) { penalty++; }
                spicyTotal -= penalty;
                if (spicyTotal < 0) { spicyTotal = 0; }
                sourTotal -= penalty;
                if (sourTotal < 0) { sourTotal = 0; }
                bitterTotal -= penalty;
                if (bitterTotal < 0) { bitterTotal = 0; }
                sweetTotal -= penalty;
                if (sweetTotal < 0) { sweetTotal = 0; }
                dryTotal -= penalty;
                if (dryTotal < 0) { dryTotal = 0; }
                Dictionary<string, int> flavorTotals = new Dictionary<string, int>();
                Dictionary<string, int> flavorProfile = new Dictionary<string, int>();
                flavorTotals.Add("Spicy", spicyTotal);
                flavorTotals.Add("Sour", sourTotal);
                flavorTotals.Add("Bitter", bitterTotal);
                flavorTotals.Add("Sweet", sweetTotal);
                flavorTotals.Add("Dry", dryTotal);
                int flavorLevel = flavorTotals.Values.Max();
                int flavors = 0;
                foreach (KeyValuePair<string, int> flavorTotal in flavorTotals)
                {
                    if (flavorTotal.Value > 0)
                    {
                        flavors++;
                    }
                    flavorProfile.Add(flavorTotal.Key, flavorTotal.Value);
                }
                string flavor = null;
                if (flavors <= 2)
                {
                    string primaryFlavor = null;
                    foreach (KeyValuePair<string, int> kvp in flavorTotals)
                    {
                        if (kvp.Value == flavorLevel) { primaryFlavor = kvp.Key; }
                    }
                    flavorTotals.Remove(flavorTotals.Keys.Max());
                    flavor = primaryFlavor;
                    if (flavorTotals.Values.Max() > 0)
                    {
                        foreach (KeyValuePair<string, int> kvp in flavorTotals)
                        {
                            if (kvp.Value == flavorLevel) { flavor = "-" + kvp.Key; }
                        }
                    }
                }
                else if (flavors == 3)
                {
                    flavor = "Rich";
                }
                else if (flavors == 4)
                {
                    flavor = "Overripe";
                }
                if (flavorLevel >= 50)
                {
                    flavor = "Mild";
                }
                if (poffinType == "Foul")
                {
                    flavor = "Foul";
                }
                food = new Food("Poffin", flavor, flavorLevel, flavorProfile, ingredients, 1);
            }
            else if (cookingType == "Pokeblock")
            {
                Dictionary<string, int> flavorProfile = new Dictionary<string, int>();
                string pokeblockType;
                int redCount = 0;
                int blueCount = 0;
                int greenCount = 0;
                int yellowCount = 0;
                int pinkCount = 0;
                int sweetCount = 0;
                int sourCount = 0;
                int dryCount = 0;
                int bitterCount = 0;
                int spicyCount = 0;
                Dictionary<string, int> colorCounts = new Dictionary<string, int>();
                foreach (Berry ing in ingredients)
                {
                    switch (ing.color)
                    {
                        case "Red": redCount++; break;
                        case "Blue": blueCount++; break;
                        case "Green": greenCount++; break;
                        case "Yellow": yellowCount++; break;
                        case "Pink": pinkCount++; break;
                    }
                    sweetCount += ing.sweetValue;
                    sourCount += ing.sourValue;
                    bitterCount += ing.bitterValue;
                    dryCount += ing.dryValue;
                    spicyCount += ing.spicyValue;
                }
                int spicyTotal = spicyCount;
                int bitterTotal = bitterCount;
                int dryTotal = dryCount;
                int sourTotal = sourCount;
                int sweetTotal = sweetCount;
                int penalty = 0;
                spicyTotal -= dryCount;
                if (spicyTotal < 0) { penalty++; }
                sourTotal -= spicyCount;
                if (sourTotal < 0) { penalty++; }
                bitterTotal -= sourCount;
                if (bitterTotal < 0) { penalty++; }
                sweetTotal -= bitterCount;
                if (sweetTotal < 0) { penalty++; }
                dryTotal -= sweetCount;
                if (dryTotal < 0) { penalty++; }
                int cnt = 1;
                while (cnt <= penalty)
                {
                    spicyTotal -= penalty;
                    if (spicyTotal < 0) { spicyTotal = 0; }
                    sweetTotal -= penalty;
                    if (sweetTotal < 0) { sweetTotal = 0; }
                    sourTotal -= penalty;
                    if (sourTotal < 0) { sourTotal = 0; }
                    dryTotal -= penalty;
                    if (dryTotal < 0) { dryTotal = 0; }
                    bitterTotal -= penalty;
                    if (bitterTotal < 0) { bitterTotal = 0; }
                    cnt++;
                }
                flavorProfile.Add("Sweet", sweetTotal);
                flavorProfile.Add("Sour", sourTotal);
                flavorProfile.Add("Bitter", bitterTotal);
                flavorProfile.Add("Dry", dryTotal);
                flavorProfile.Add("Spicy", spicyTotal);
                colorCounts.Add("Red", redCount);
                colorCounts.Add("Blue", blueCount);
                colorCounts.Add("Green", greenCount);
                colorCounts.Add("Yellow", yellowCount);
                colorCounts.Add("Pink", pinkCount);
                int distinctColors = 0;
                foreach (KeyValuePair<string, int> kvp in colorCounts)
                { if (kvp.Value > 0) { distinctColors++; } }
                string color = null;
                if (distinctColors <= 3)
                {
                    Dictionary<string, int> mostColors = new Dictionary<string, int>();
                    foreach (KeyValuePair<string, int> kvp in colorCounts)
                    { if (kvp.Value == colorCounts.Values.Max()) { mostColors.Add(kvp.Key, kvp.Value); } }
                    if (mostColors.Count() == 1)
                    { color = mostColors.First().Key; }
                    else
                    {
                        int selected = rand.Next(1, mostColors.Count());
                        KeyValuePair<string, int> selection = mostColors.ElementAt(selected - 1);
                        color = selection.Key;
                    }
                }
                decimal highestPercentage = 0;
                foreach (Berry ing in ingredients)
                {
                    if (ing.color == color)
                    {
                        if (ing.plusChance > highestPercentage) { highestPercentage = ing.plusChance; }
                    }
                }
                pokeblockType = color;
                decimal rollForPlus = Convert.ToDecimal(rand.NextDouble());
                if (rollForPlus <= highestPercentage) { pokeblockType += "+"; }
                food = new Food("Pokeblock", pokeblockType, 0, flavorProfile, ingredients, 1);
            }
            else if (cookingType == "Pokepuff")
            {
                string pokepuffType;
                int sweetCount = 0;
                int spicyCount = 0;
                int bitterCount = 0;
                int sourCount = 0;
                int dryCount = 0;
                int spicyTotal = 0;
                int sourTotal = 0;
                int bitterTotal = 0;
                int sweetTotal = 0;
                int dryTotal = 0;
                int penalty = 0;
                foreach (Berry ing in ingredients)
                {
                    sweetCount += ing.sweetValue;
                    spicyCount += ing.spicyValue;
                    bitterCount += ing.bitterValue;
                    sourCount += ing.sourValue;
                    dryCount += ing.dryValue;
                    spicyTotal = spicyCount;
                    sourTotal = sourCount;
                    bitterTotal = bitterCount;
                    sweetTotal = sweetCount;
                    dryTotal = dryCount;
                }
                spicyTotal -= dryCount;
                if (spicyTotal < 0) { penalty++; }
                sourTotal -= spicyCount;
                if (sourTotal < 0) { penalty++; }
                bitterTotal -= sourCount;
                if (bitterTotal < 0) { penalty++; }
                sweetTotal -= bitterCount;
                if (sweetTotal < 0) { penalty++; }
                dryTotal -= sweetCount;
                if (dryTotal < 0) { penalty++; }
                spicyTotal -= penalty;
                if (spicyTotal < 0) { spicyTotal = 0; }
                sourTotal -= penalty;
                if (sourTotal < 0) { sourTotal = 0; }
                bitterTotal -= penalty;
                if (bitterTotal < 0) { bitterTotal = 0; }
                sweetTotal -= penalty;
                if (sweetTotal < 0) { sweetTotal = 0; }
                dryTotal -= penalty;
                if (dryTotal < 0) { dryTotal = 0; }
                Dictionary<string, int> flavorTotals = new Dictionary<string, int>();
                Dictionary<string, int> flavorProfile = new Dictionary<string, int>();
                flavorTotals.Add("Spicy", spicyTotal);
                flavorTotals.Add("Sour", sourTotal);
                flavorTotals.Add("Bitter", bitterTotal);
                flavorTotals.Add("Sweet", sweetTotal);
                flavorTotals.Add("Dry", dryTotal);
                int flavorLevel = flavorTotals.Values.Max();
                int flavors = 0;
                foreach (KeyValuePair<string, int> flavorTotal in flavorTotals)
                {
                    if (flavorTotal.Value > 0)
                    {
                        flavors++;
                    }
                    flavorProfile.Add(flavorTotal.Key, flavorTotal.Value);
                }
                string flavor = null;
                if (flavors <= 2)
                {
                    string primaryFlavor = null;
                    foreach (KeyValuePair<string, int> kvp in flavorTotals)
                    {
                        if (kvp.Value == flavorLevel) { primaryFlavor = kvp.Key; }
                    }
                    flavorTotals.Remove(primaryFlavor);
                    flavor = primaryFlavor;
                    if (flavorTotals.Values.Max() > 0)
                    {
                        foreach (KeyValuePair<string, int> kvp in flavorTotals)
                            if (kvp.Value == flavorTotals.Values.Max()) { flavor += "-" + kvp.Key; }
                    }
                }
                else if (flavors == 3) { flavor = "Robust"; }
                else if (flavors == 4) { flavor = "Overwhelming"; }
                string level = null;
                if (flavorLevel < 10) { level = "Basic"; }
                else if (flavorLevel < 25) { level = "Frosted"; }
                else if (flavorLevel < 35) { level = "Fancy"; }
                else if (flavorLevel < 50) { level = "Deluxe"; }
                else if (flavorLevel >= 50) { level = "Supreme"; }
                pokepuffType = level + " " + flavor;
                food = new Food("Pokepuff", pokepuffType, 0, flavorProfile, ingredients, 1);
            }
            return food;
        }
    }
}
