using System;
using System.Collections.Generic;
using System.Text;

namespace PokeParadise.Classes
{
    public class TriviaQuestion
    {
        public string chosenStat;
        public Pokemon correctAnswer;
        public Pokemon wrongAnswer1;
        public Pokemon wrongAnswer2;
        public Pokemon wrongAnswer3;
        public bool wrongAnswer1Valid;
        public bool wrongAnswer2Valid;
        public bool wrongAnswer3Valid;

        public TriviaQuestion(Pokemon correctAnswer, string chosenStat)
        {
            this.chosenStat = chosenStat;
            this.correctAnswer = correctAnswer;
            Pokemon[] dex = correctAnswer.FetchPokedex();
            Random rand = new Random();
            bool allChosen = false;
            wrongAnswer1 = new Pokemon();
            wrongAnswer2 = new Pokemon();
            wrongAnswer3 = new Pokemon();
            wrongAnswer1Valid = false;
            wrongAnswer2Valid = false;
            wrongAnswer3Valid = false;
            while (!allChosen)
            {
                int dexNo = rand.Next(1, dex.Length - 1);
                if ((dexNo != correctAnswer.dexNo && dexNo != wrongAnswer1.dexNo && dexNo != wrongAnswer2.dexNo && dexNo != wrongAnswer3.dexNo) && (!wrongAnswer1Valid))
                {
                    wrongAnswer1 = new Pokemon(dexNo);
                }
                dexNo = rand.Next(1, dex.Length - 1);
                if ((dexNo != correctAnswer.dexNo && dexNo != wrongAnswer1.dexNo && dexNo != wrongAnswer2.dexNo && dexNo != wrongAnswer3.dexNo) && (!wrongAnswer2Valid))
                {
                    wrongAnswer2 = new Pokemon(dexNo);
                }
                dexNo = rand.Next(1, dex.Length - 1);
                if ((dexNo != correctAnswer.dexNo && dexNo != wrongAnswer1.dexNo && dexNo != wrongAnswer2.dexNo && dexNo != wrongAnswer3.dexNo) && (!wrongAnswer3Valid))
                {
                    wrongAnswer3 = new Pokemon(dexNo);
                }
                switch (chosenStat)
                    {
                        case "dexNo": 
                            if (wrongAnswer1.dexNo != correctAnswer.dexNo) { wrongAnswer1Valid = true; } if (wrongAnswer2.dexNo != correctAnswer.dexNo) { wrongAnswer2Valid = true; } if (wrongAnswer3.dexNo != correctAnswer.dexNo) { wrongAnswer3Valid = true; }
                                break;
                        case "type1and2":
                            if (wrongAnswer1.type1 != correctAnswer.type1) { wrongAnswer1Valid = true; } if (wrongAnswer2.type1 != correctAnswer.type1) { wrongAnswer2Valid = true; } if (wrongAnswer3.type1 != correctAnswer.type1) { wrongAnswer3Valid = true; }
                            break;
                        case "speciesAbility1and2":
                            if (wrongAnswer1.speciesAbility1 != correctAnswer.speciesAbility1) { wrongAnswer1Valid = true; } if (wrongAnswer2.speciesAbility1 != correctAnswer.speciesAbility1) { wrongAnswer2Valid = true; } if (wrongAnswer3.speciesAbility1 != correctAnswer.speciesAbility1) { wrongAnswer3Valid = true; }
                            break;
                        case "baseStatTotal":
                            if (wrongAnswer1.baseStatTotal < correctAnswer.baseStatTotal) { wrongAnswer1Valid = true; } if (wrongAnswer2.baseStatTotal < correctAnswer.baseStatTotal) { wrongAnswer2Valid = true; } if (wrongAnswer3.baseStatTotal < correctAnswer.baseStatTotal) { wrongAnswer3Valid = true; }
                            break;
                        case "baseHP":
                            if (wrongAnswer1.baseHP < correctAnswer.baseHP) { wrongAnswer1Valid = true; } if (wrongAnswer2.baseHP < correctAnswer.baseHP) { wrongAnswer2Valid = true; } if (wrongAnswer3.baseHP < correctAnswer.baseHP) { wrongAnswer3Valid = true; }
                            break;
                        case "baseAtk":
                            if (wrongAnswer1.baseAtk < correctAnswer.baseAtk) { wrongAnswer1Valid = true; } if (wrongAnswer2.baseAtk < correctAnswer.baseAtk) { wrongAnswer2Valid = true; } if (wrongAnswer3.baseAtk < correctAnswer.baseAtk) { wrongAnswer3Valid = true; }
                            break;
                        case "baseDef":
                            if (wrongAnswer1.baseDef < correctAnswer.baseDef) { wrongAnswer1Valid = true; } if (wrongAnswer2.baseDef < correctAnswer.baseDef) { wrongAnswer2Valid = true; } if (wrongAnswer3.baseDef < correctAnswer.baseDef) { wrongAnswer3Valid = true; }
                            break;
                        case "baseSpAtk":
                            if (wrongAnswer1.baseSpAtk < correctAnswer.baseSpAtk) { wrongAnswer1Valid = true; } if (wrongAnswer2.baseSpAtk < correctAnswer.baseSpAtk) { wrongAnswer2Valid = true; } if (wrongAnswer3.baseSpAtk < correctAnswer.baseSpAtk) { wrongAnswer3Valid = true; }
                            break;
                        case "baseSpDef":
                            if (wrongAnswer1.baseSpDef < correctAnswer.baseSpDef) { wrongAnswer1Valid = true; } if (wrongAnswer2.baseSpDef < correctAnswer.baseSpDef) { wrongAnswer2Valid = true; } if (wrongAnswer3.baseSpDef < correctAnswer.baseSpDef) { wrongAnswer3Valid = true; }
                            break;
                        case "baseSpd":
                            if (wrongAnswer1.baseSpd < correctAnswer.baseSpd) { wrongAnswer1Valid = true; } if (wrongAnswer2.baseSpd < correctAnswer.baseSpd) { wrongAnswer2Valid = true; } if (wrongAnswer3.baseSpd < correctAnswer.baseSpd) { wrongAnswer3Valid = true; }
                            break;
                        case "evoLevel":
                            if (wrongAnswer1.evoLevel != correctAnswer.evoLevel) { wrongAnswer1Valid = true; } if (wrongAnswer2.evoLevel != correctAnswer.evoLevel) { wrongAnswer2Valid = true; } if (wrongAnswer3.evoLevel != correctAnswer.evoLevel) { wrongAnswer3Valid = true; }
                            break;
                        case "legendStatus":
                            if (wrongAnswer1.legendStatus != correctAnswer.legendStatus) { wrongAnswer1Valid = true; } if (wrongAnswer2.legendStatus != correctAnswer.legendStatus) { wrongAnswer2Valid = true; } if (wrongAnswer3.legendStatus != correctAnswer.legendStatus) { wrongAnswer3Valid = true; }
                            break;
                        case "mythicStatus":
                            if (wrongAnswer1.mythicStatus != correctAnswer.mythicStatus) { wrongAnswer1Valid = true; } if (wrongAnswer2.mythicStatus != correctAnswer.mythicStatus) { wrongAnswer2Valid = true; } if (wrongAnswer3.mythicStatus != correctAnswer.mythicStatus) { wrongAnswer3Valid = true; }
                            break;
                        case "nextEvo":
                            if (wrongAnswer1.nextEvo != correctAnswer.nextEvo) { wrongAnswer1Valid = true; } if (wrongAnswer2.nextEvo != correctAnswer.nextEvo) { wrongAnswer2Valid = true; } if (wrongAnswer3.nextEvo != correctAnswer.nextEvo) { wrongAnswer3Valid = true; }
                            break;
                        case "prevEvo":
                            if (wrongAnswer1.prevEvo != correctAnswer.prevEvo) { wrongAnswer1Valid = true; } if (wrongAnswer2.prevEvo != correctAnswer.prevEvo) { wrongAnswer2Valid = true; } if (wrongAnswer3.prevEvo != correctAnswer.prevEvo) { wrongAnswer3Valid = true; }
                            break;
                        case "nextEvoNone":
                            if (wrongAnswer1.nextEvo != null && wrongAnswer1.nextEvo != "" && wrongAnswer1.nextEvo != " ") { wrongAnswer1Valid = true; } 
                            if (wrongAnswer2.nextEvo != null && wrongAnswer2.nextEvo != "" && wrongAnswer2.nextEvo != " ") { wrongAnswer2Valid = true; } 
                            if (wrongAnswer3.nextEvo != null && wrongAnswer3.nextEvo != "" && wrongAnswer3.nextEvo != " ") { wrongAnswer3Valid = true; }
                            break;
                        case "prevEvoNone":
                            if (wrongAnswer1.prevEvo != null && wrongAnswer1.prevEvo != "" && wrongAnswer1.prevEvo != " ") { wrongAnswer1Valid = true; }
                            if (wrongAnswer2.prevEvo != null && wrongAnswer2.prevEvo != "" && wrongAnswer2.prevEvo != " ") { wrongAnswer2Valid = true; }
                            if (wrongAnswer3.prevEvo != null && wrongAnswer3.prevEvo != "" && wrongAnswer3.prevEvo != " ") { wrongAnswer3Valid = true; }
                            break;
                        case "eggGroup1and2":
                            if (wrongAnswer1.eggGroup1 != correctAnswer.eggGroup1) { wrongAnswer1Valid = true; } if (wrongAnswer2.eggGroup1 != correctAnswer.eggGroup1) { wrongAnswer2Valid = true; } if (wrongAnswer3.eggGroup1 != correctAnswer.eggGroup1) { wrongAnswer3Valid = true; }
                            break;
                        case "levelSpeed":
                            if (wrongAnswer1.levelSpeed != correctAnswer.levelSpeed) { wrongAnswer1Valid = true; } if (wrongAnswer2.levelSpeed != correctAnswer.levelSpeed) { wrongAnswer2Valid = true; } if (wrongAnswer3.levelSpeed != correctAnswer.levelSpeed) { wrongAnswer3Valid = true; }
                            break;
                        case "blurb":
                            if (wrongAnswer1.blurb != correctAnswer.blurb) { wrongAnswer1Valid = true; } if (wrongAnswer2.blurb != correctAnswer.blurb) { wrongAnswer2Valid = true; } if (wrongAnswer3.blurb != correctAnswer.blurb) { wrongAnswer3Valid = true; }
                            break;
                    }
                if (wrongAnswer1Valid && wrongAnswer2Valid && wrongAnswer3Valid) 
                { 
                    allChosen = true; 
                }
            }
        }
        public TriviaQuestion() { }
    }
}
