using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using PokeParadise.Attributes;
using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PokeParadise.Modules
{
    [Group("Trivia")]
    [Alias("t")]
    public class TriviaModule : InteractiveBase<SocketCommandContext>
    {
        readonly BaseModule bm = new BaseModule();

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

        [Command("", RunMode = RunMode.Async)]
        [Summary("Starts a Pokemon Trivia round! You'll be given a piece of information about a random Pokemon. If you get the question right, you could get its first form's egg! " +
            "You can specify the difficulty by adding \"easy\", \"medium\", or \"hard\" at the end. Easy gives you a 25% chance of getting the egg, Medium gives you 50%, and Hard gives you 75%. " +
            "If you specify any value besides easy, medium, or hard, the bot will pick a random difficulty! Usage: >trivia {optional difficulty, default is easy}")]
        [RequireBegan()]
        [RequireTriviaCooldown()]
        public async Task TriviaBaseCommand(string difficulty = null)
        {
            Random rand = new Random();
            if (difficulty != null)
            {
                difficulty = difficulty.ToLower();
            }
            if (difficulty != "easy" && difficulty != "medium" && difficulty != "hard")
            {
                int diffChoice = rand.Next(1, 99);
                if (diffChoice <= 33) { difficulty = "easy"; }
                else if (diffChoice <= 66) { difficulty = "medium"; }
                else if (diffChoice <= 99) { difficulty = "hard"; }
            }
            int eggChance = 0;
            switch (difficulty)
            {
                case "easy": eggChance = 25; break;
                case "medium": eggChance = 5; break;
                case "hard": eggChance = 75; break;
            }
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            var user = Context.User;
            Player player = bm.PlayerLoad(user.Id);
            if (!player.isDoingTrivia)
            {
                player.isDoingTrivia = true;
                bm.PlayerSave(player);
                sb.AppendLine($"Remember to either answer with the name next to the correct answer, or the name of the correct Pokemon! You'll have 60 seconds to answer!");
                sb.AppendLine($"");
                sb.AppendLine($"Difficulty: **{difficulty.Substring(0,1).ToUpper() + difficulty.Substring(1)}**");
                Pokemon pm = new Pokemon();
                Pokemon[] dex = pm.FetchPokedex();
                int dexNo = rand.Next(1, dex.Length - 1);
                Pokemon trivia = dex[dexNo];
                if (trivia.legendStatus)
                {
                    int legendConfirm = rand.Next(1, 3);
                    if (legendConfirm == 1)
                    {
                        trivia = new Pokemon(dexNo);
                    }
                    else
                    {
                        dexNo = rand.Next(1, 891);
                        trivia = new Pokemon(dexNo);
                    }
                }
                else if (trivia.mythicStatus)
                {
                    int mythicConfirm = rand.Next(1, 2);
                    if (mythicConfirm == 1)
                    {
                        trivia = new Pokemon(dexNo);
                    }
                    else
                    {
                        dexNo = rand.Next(1, 891);
                        trivia = new Pokemon(dexNo);
                    }
                }
                TriviaQuestion tq = new TriviaQuestion();
                switch (difficulty)
                {
                    case "easy":
                        int easyChoice = rand.Next(1, 4);
                        switch (easyChoice)
                        {
                            case 1: if (trivia.legendStatus) { sb.AppendLine($"**Question: Which of these Pokemon is a Legendary**?"); } else { sb.AppendLine($"**Question: Which of these Pokemon is NOT a Legendary**?"); } tq = new TriviaQuestion(trivia, "legendStatus"); break;
                            case 2: if (trivia.mythicStatus) { sb.AppendLine($"**Question: Which of these Pokemon is a Mythical**?"); } else { sb.AppendLine($"**Question: Which of these Pokemon is NOT a Mythical**?"); } tq = new TriviaQuestion(trivia, "mythicStatus"); break;
                            case 3: 
                                if (trivia.nextEvo != null && trivia.nextEvo != "" && trivia.nextEvo != " ")
                                {
                                    sb.AppendLine($"**Question: Which of these Pokemon evolves to {trivia.nextEvo.Substring(0, 1).ToUpper() + trivia.nextEvo.Substring(1)}**?");
                                    tq = new TriviaQuestion(trivia, "nextEvo"); break;
                                }
                                else
                                {
                                    sb.AppendLine($"**Question: Which of these Pokemon DOES NOT evolve to another species**?");
                                    tq = new TriviaQuestion(trivia, "nextEvoNone"); break;
                                }
                            case 4:
                                if (trivia.prevEvo != null || trivia.prevEvo != "" || trivia.prevEvo != " ")
                                { 
                                    sb.AppendLine($"**Question: Which of these Pokemon evolves from {trivia.prevEvo.Substring(0, 1).ToUpper() + trivia.prevEvo.Substring(1)}**?");
                                    tq = new TriviaQuestion(trivia, "nextEvo"); break;
                                }
                                else
                                {
                                    sb.AppendLine($"**Question: Which of these Pokemon DOES NOT evolve from another species**?");
                                    tq = new TriviaQuestion(trivia, "nextEvoNone"); break;
                                }
                        }
                        break;
                    case "medium":
                        int mediumChoice = rand.Next(1, 4);
                        string blurb = trivia.blurb.Replace(trivia.pkmnName, "[POKEMON NAME]");
                        string eggGroup2 = trivia.eggGroup2;
                        if (trivia.eggGroup2 == null || trivia.eggGroup2 == "" || trivia.eggGroup2 == " ") { eggGroup2 = "None"; }
                        string speciesAbility2 = trivia.speciesAbility2;
                        if (trivia.speciesAbility2 == null || trivia.speciesAbility2 == "" || trivia.speciesAbility2 == " ") { speciesAbility2 = "None"; }
                        switch (mediumChoice)
                        {
                            case 1: sb.AppendLine($"**Question: Which of these Pokemon can be found with both the Ability {trivia.speciesAbility1} and the Ability {speciesAbility2}**?"); tq = new TriviaQuestion(trivia, "speciesAbility1and2"); break;
                            case 2: sb.AppendLine($"**Question: Which of these Pokemon has the Egg Groups {trivia.eggGroup1} and {eggGroup2}**?"); tq = new TriviaQuestion(trivia, "eggGroup1and2"); break;
                            case 3: sb.AppendLine($"**Question: Which of these Pokemon has following Pokedex Entry?"); sb.AppendLine($"{blurb}"); tq = new TriviaQuestion(trivia, "blurb"); break;
                            case 4: sb.AppendLine($"**Question: Which of these Pokemon evolves at level {trivia.evoLevel}**?"); tq = new TriviaQuestion(trivia, "evoLevel"); break;
                        }
                        break;
                    case "hard":
                        int hardChoice = rand.Next(1, 10);
                        string type2 = trivia.type2;
                        if (trivia.type2 == null || trivia.type2 == "" || trivia.type2 == " ") { type2 = "None"; }
                        switch (hardChoice)
                        {
                            case 1: sb.AppendLine($"**Question: Which of these Pokemon has the National Pokedex number {trivia.dexNo}**?"); tq = new TriviaQuestion(trivia, "dexNo"); break;
                            case 2: sb.AppendLine($"**Question: Which of these Pokemon has the fastest Experience Gain Rate**?"); tq = new TriviaQuestion(trivia, "levelSpeed"); break;
                            case 3: sb.AppendLine($"**Question: Which of these Pokemon has the highest Base Stat Total**?"); tq = new TriviaQuestion(trivia, "baseStatTotal"); break;
                            case 4: sb.AppendLine($"**Question: Which of these Pokemon has the highest Base HP Stat**?"); tq = new TriviaQuestion(trivia, "baseHP"); break;
                            case 5: sb.AppendLine($"**Question: Which of these Pokemon has the highest Base Attack Stat**?"); tq = new TriviaQuestion(trivia, "baseAtk"); break;
                            case 6: sb.AppendLine($"**Question: Which of these Pokemon has the highest Base Defense Stat**?"); tq = new TriviaQuestion(trivia, "baseDef"); break;
                            case 7: sb.AppendLine($"**Question: Which of these Pokemon has the highest Base Sp. Attack Stat**?"); tq = new TriviaQuestion(trivia, "baseSpAtk"); break;
                            case 8: sb.AppendLine($"**Question: Which of these Pokemon has the highest Base Sp. Defense Stat**?"); tq = new TriviaQuestion(trivia, "baseSpDef"); break;
                            case 9: sb.AppendLine($"**Question: Which of these Pokemon has the highest Base Speed Stat**?"); tq = new TriviaQuestion(trivia, "baseSpd"); break;
                            case 10: sb.AppendLine($"**Question: Which of these Pokemon has the Primary Type {trivia.type1} and the Secondary Type {type2}**?"); tq = new TriviaQuestion(trivia, "type1and2"); break;
                        }
                        break;
                }
                int rightAnswerChoice = rand.Next(0, 3);
                Pokemon[] choices = new Pokemon[4];
                choices[rightAnswerChoice] = tq.correctAnswer;
                List<int> indexes = new List<int>
                {
                    0,
                    1,
                    2,
                    3
                };
                List<int> toRemove = new List<int>();
                foreach (int index in indexes)
                {
                    if (index == rightAnswerChoice) { toRemove.Add(index); }
                }
                foreach (int i in toRemove)
                {
                    indexes.Remove(i);
                }
                int cnt = 1;
                foreach (int index in indexes)
                {
                    switch (cnt)
                    {
                        case 1: choices[index] = tq.wrongAnswer1; break;
                        case 2: choices[index] = tq.wrongAnswer2; break;
                        case 3: choices[index] = tq.wrongAnswer3; break;
                    }
                    cnt++;
                }
                sb.AppendLine($"");
                sb.AppendLine($"**1. {choices[0].pkmnName}; 2. {choices[1].pkmnName}; 3. {choices[2].pkmnName}; 4. {choices[3].pkmnName}**");
                embed.Title = "Pokemon Trivia!";
                embed.WithColor(new Color(247, 89, 213));
                embed.Description = sb.ToString();
                IUserMessage msg = await ReplyAsync(null, false, embed.Build());
                bool userHasAnswered = false;
                bool correctAnswer = false;
                TimeSpan timeout = new TimeSpan(0, 0, 60);
                while (!userHasAnswered && DateTimeOffset.Now - msg.Timestamp <= timeout)
                {
                    var response = await NextMessageAsync();
                    if (response != null)
                    {
                        if (response.Author.Id == user.Id)
                        {
                            bool isInt = Int32.TryParse(response.Content, out int chosenInt);
                            if ((isInt && chosenInt - 1 == rightAnswerChoice) || response.Content == tq.correctAnswer.pkmnName)
                            {
                                userHasAnswered = true;
                                correctAnswer = true;
                            }
                            else
                            {
                                embed = new EmbedBuilder();
                                sb = new StringBuilder();
                                sb.AppendLine($"Sorry, that's the wrong answer. Make sure to try again later!");
                                embed.Description = sb.ToString();
                                embed.WithColor(new Color(247, 89, 213));
                                embed.Title = $"Wrong Answer!";
                                await ReplyAsync(null, false, embed.Build());
                                userHasAnswered = true;
                                correctAnswer = false;
                                player.isDoingTrivia = false;
                                bm.PlayerSave(player);
                                break;
                            }
                        }
                    }
                    else
                    {
                        embed = new EmbedBuilder();
                        sb = new StringBuilder();
                        sb.AppendLine($"Sorry, you're out of time! Try again later!");
                        embed.Description = sb.ToString();
                        embed.WithColor(new Color(247, 89, 213));
                        embed.Title = $"Trivia Failed";
                        await ReplyAsync(null, false, embed.Build());
                        player.isDoingTrivia = false;
                        bm.PlayerSave(player);
                    }
                }
                if (correctAnswer)
                {
                    player.isDoingTrivia = false;
                    embed = new EmbedBuilder();
                    sb = new StringBuilder();
                    sb.AppendLine($"Yes! You got the right answer!");
                    sb.AppendLine($"");
                    int eggRoll = rand.Next(1, 100);
                    if (eggRoll <= eggChance)
                    {
                        player.lastTrivia = DateTimeOffset.Now;
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
                        Pokemon received;
                        if (tq.correctAnswer.prevEvo != null && tq.correctAnswer.prevEvo != "" && tq.correctAnswer.prevEvo != " ")
                        {
                            Pokemon prevo = new Pokemon(tq.correctAnswer.prevEvo);
                            if (prevo.prevEvo != null && prevo.prevEvo != "" && prevo.prevEvo != " ")
                            {
                                received = new Pokemon(prevo.prevEvo);
                            }
                            else
                            {
                                received = prevo;
                            }
                        }
                        else
                        {
                            received = new Pokemon(tq.correctAnswer.dexNo);
                        }
                        Egg e = new Egg(received, DateTimeOffset.Now, eggId);
                        player.eggs.Add(e);
                        sb.AppendLine($"And... You also gained an Egg!");
                    }
                    else
                    {
                        sb.AppendLine($"And... You didn't get an Egg this time! Try again later!");
                    }
                    bm.PlayerSave(player);
                    embed.Title = "Winner!";
                    embed.Description = sb.ToString();
                    embed.WithColor(new Color(247, 89, 213));
                    await ReplyAsync(null, false, embed.Build());
                }
            }
            else
            {
                sb.AppendLine($"Finish your other Trivia Game first!");
                embed.Title = "You're Already Playing!";
                embed.Description = sb.ToString();
                embed.WithColor(new Color(247, 89, 213));
                await ReplyAsync(null, false, embed.Build());
            }
        }
    }
}
